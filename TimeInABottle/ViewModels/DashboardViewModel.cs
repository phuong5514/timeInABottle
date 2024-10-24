using CommunityToolkit.Mvvm.ComponentModel;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models;
using TimeInABottle.Core.Services;
using TimeInABottle.Services;
namespace TimeInABottle.ViewModels;

public partial class DashboardViewModel : ObservableRecipient
{
    private IDaoService? _dao;

    private TimeOnly _time;
    public TimeOnly Time
    {
        get => _time;
        private set
        {
            if (_time != value)
            {
                _time = value;
                OnPropertyChanged(nameof(Time)); // Notify UI of changes

                UpdateNextTask();
            }
        }
    }

    private void UpdateTime() => Time = TimeOnly.FromDateTime(DateTime.Now);


    public FullObservableCollection<ITask> Tasks
    {
        set; get;
    }


    private ITask? _nextTask;
    public ITask? NextTask
    {
        get => _nextTask;
        private set
        {
            if (_nextTask != value)
            {
                _nextTask = value;
                OnPropertyChanged(nameof(NextTask)); // Notify UI of changes
            }
        }
    }



    private void UpdateNextTask()
    {
        if (Tasks == null)
        {
            NextTask = null;
            return;
        }

        foreach (var task in Tasks)
        {
            // Get the first task that starts after the current time (assuming Tasks are sorted)
            if (task.Start > Time)
            {
                NextTask = task;
                var notificationService = new NotificationService();
                notificationService.ShowNextTask(task);
                return;
            }
        }

        NextTask = null;
    }


    public DateOnly Date
    {
        set; get;
    }
    private void UpdateDate() => Date = DateOnly.FromDateTime(DateTime.Now);


    public void Innit()
    {
        _dao = new MockDaoService();
        getAllTasks();
        UpdateTime();
        UpdateDate();
    }

    private void getAllTasks() {
        if (_dao == null) { 
            return;
        }
        var tasks = _dao.GetAllTasks();
        Tasks = tasks;
    }

    




    //private void 

    public DashboardViewModel()
    {
        Innit();
        UpdateTime();
    }
}
