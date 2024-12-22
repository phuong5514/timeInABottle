using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using TimeInABottle.Contracts.Services;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models.Filters;
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

    private List<ITask> _tasksForScheduling;

    public FullObservableCollection<ITask> ThisWeekTasks
    {
        private set; get;
    }

    [ObservableProperty]
    public TaskWrapper? selectedTask;

    public bool IsTaskSelected => SelectedTask != null;
    public bool IsTaskNotSelected => !IsTaskSelected;


    public void Innit()
    {
        TasksForScheduling = new();
        _tasksForScheduling = new();
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
        try
        {
            if (_tasksForScheduling.Contains(task))
            {
                return;
            }

            var taskWrapper = new TaskWrapper(task);
            TasksForScheduling.Add(taskWrapper);
            _tasksForScheduling.Add(task);

          
            SelectedTask = taskWrapper;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public ICommand RemoveTaskForSchedulingCommand => new RelayCommand<TaskWrapper>(RemoveTaskForScheduling);

    private void RemoveTaskForScheduling(TaskWrapper taskwrapper)
    {
        //var taskWrapper = TasksForScheduling.First(t => t.Task == task);

        _tasksForScheduling.Remove(taskwrapper.Task);
        TasksForScheduling.Remove(taskwrapper);


        //SelectedTask = null;
        EnsureItemSelected();
    }

    //public ICommand ScheduleSelectedTaskCommand => new RelayCommand(ScheduleSelectedTaskExecute);

    public void ScheduleSelectedTaskExecute()
    {
        try
        {
            // clear previous derived tasks
            var derivedTasks = _dao?.CustomQuery(new DerivedTaskFilter());
            _dao.DeleteTasks(derivedTasks);
            //foreach (var task in derivedTasks)
            //{
            //    _dao?.DeleteTask(task);
            //}

            if (SelectedTask == null)
            {
                return;
            }
            List<DerivedTask> result = (List<DerivedTask>)_plannerService.ScheduleThisWeek(TasksForScheduling);

            _dao.AddTasks(result);
            //foreach (var task in result)
            //{
            //    _dao?.AddTask(task);
            //}
            LoadData();

            SelectedTask = null;
            TasksForScheduling.Clear();
            _tasksForScheduling.Clear();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.StackTrace);
        }

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
