using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TimeInABottle.Contracts.Services;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models.Filters;
using TimeInABottle.Core.Models.Tasks;
using TimeInABottle.Models;

namespace TimeInABottle.ViewModels;

public partial class SchedularViewModel : ObservableRecipient
{
    private IDaoService? _dao;
    private IPlannerService _plannerService;

    private List<ITask> _tasksForScheduling;
    public FullObservableCollection<TaskWrapper> TasksForScheduling
    {
        private set;
        get;
        //get => _tasksWrapperForScheduling;
    }

    public FullObservableCollection<ITask> ThisWeekTasks
    {
        private set; get;
    }

    private TaskWrapper? _selectedTask;
    public TaskWrapper? SelectedTask
    {
        get => _selectedTask;
        set
        {
            if (_selectedTask != value)
            {
                _selectedTask = value;
                OnPropertyChanged(nameof(SelectedTask));

                // Update the editable copy when a new task is selected
                if (_selectedTask != null)
                {
                    EditableTask = new TaskWrapper(_selectedTask); // Assuming TaskWrapper supports cloning
                }
            }
        }
    }

    private TaskWrapper? _editableTask;
    public TaskWrapper? EditableTask
    {
        get => _editableTask;
        set
        {
            _editableTask = value;
            OnPropertyChanged(nameof(EditableTask));
        }
    }

    // Command to confirm changes
    public ICommand ConfirmChangesCommand => new RelayCommand(ConfirmChanges);

    private void ConfirmChanges()
    {
        if (EditableTask != null && SelectedTask != null)
        {
            SelectedTask.CopyFrom(EditableTask); // Assuming TaskWrapper has a CopyFrom method
            OnPropertyChanged(nameof(SelectedTask));
        }
    }


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
            if (_tasksForScheduling.Contains(task) || task is DerivedTask)
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

    public bool IsOnlyFromNow { set; get; }
    public bool PreserveOldTasks
    {
        set; get;
    }


    public void ScheduleSelectedTaskExecute()
    {
        try
        {
            // clear previous derived tasks
            if (!PreserveOldTasks)
            {
                var derivedTasks = _dao?.CustomQuery(new DerivedTaskFilter());
                _dao?.DeleteTasks(derivedTasks);
            }

            if (TasksForScheduling.Count <= 0)
            {
                return;
            }

            List<DerivedTask> result;
            if (IsOnlyFromNow)
            {
                result = (List<DerivedTask>)_plannerService.ScheduleThisWeekFromNow(TasksForScheduling);
            }
            else { 
                result = (List<DerivedTask>)_plannerService.ScheduleThisWeek(TasksForScheduling);
            }

            _dao?.AddTasks(result);
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
            SelectedTask = TasksForScheduling.First();
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
