using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using TimeInABottle.Contracts.Services;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models.Tasks;
using TimeInABottle.Core.Models.Weather;
using TimeInABottle.Models;

namespace TimeInABottle.ViewModels;

public partial class SchedularViewModel : ObservableRecipient
{
    private IDaoService? _dao;
    private IPlannerService _plannerService;


    public FullObservableCollection<TaskWrapper> TasksForScheduling
    {
        private set; get;
    }

    private readonly List<ITask> _tasksForScheduling;

    public FullObservableCollection<ITask> ThisWeekTasks
    {
        private set; get;
    }

    [ObservableProperty]
    public TaskWrapper? selectedTask;


    public void Innit()
    {
        _dao = App.GetService<IDaoService>();
        _plannerService = App.GetService<IPlannerService>();
        LoadData();
    }

    private void getWeekTasks()
    {
        if (_dao == null)
        {
            return;
        }
        ThisWeekTasks = _dao.GetThisWeekTasks();
    }


    public ICommand AddTaskForSchedulingCommand => new RelayCommand<ITask>(AddTaskForScheduling);

    private void AddTaskForScheduling(ITask task)
    {
        var taskWrapper = new TaskWrapper(task);
        TasksForScheduling.Add(taskWrapper);
        _tasksForScheduling.Add(task);
        SelectedTask = taskWrapper;
    }

    public ICommand RemoveTaskForSchedulingCommand => new RelayCommand<ITask>(RemoveTaskForScheduling);

    private void RemoveTaskForScheduling(ITask task)
    {
        var taskWrapper = TasksForScheduling.First(t => t.Task == task);
        TasksForScheduling.Remove(taskWrapper);
        _tasksForScheduling.Remove(task);
        EnsureItemSelected();
    }

    public ICommand ScheduleSelectedTask => new RelayCommand(ScheduleSelectedTaskExecute);

    private void ScheduleSelectedTaskExecute()
    {
        if (SelectedTask == null)
        {
            return;
        }
        List<DerivedTask> result = (List<DerivedTask>)_plannerService.ScheduleThisWeek(TasksForScheduling);
        foreach (var task in result)
        {
            _dao?.UpdateTask(task);
        }
        LoadData();
    }


    private void LoadData()
    {
        getWeekTasks();
    }


    public SchedularViewModel()
    {
        Innit();
    }

    public void EnsureItemSelected()
    {
        try
        {
            SelectedTask ??= TasksForScheduling.First();
        }
        catch (InvalidOperationException)
        {
            SelectedTask = null;
        }
    }

    public void OnNavigatedTo(object parameter)
    {
        EnsureItemSelected();
    }
}
