using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
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

    private DispatcherTimer _timer;
    private void UpdateTime(object sender, object e) => Time = TimeOnly.FromDateTime(DateTime.Now);

    private void StartTimer()
    {
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(15) // TODO: config file / setting page options
        };
        _timer.Tick += UpdateTime;
        _timer.Start();
    }

    


    public FullObservableCollection<ITask> TodayTasks
    {
        set; get;
    }
    public FullObservableCollection<ITask> ThisWeekTasks
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
        if (TodayTasks == null)
        {
            NextTask = null;
            return;
        }

        foreach (var task in TodayTasks)
        {
            // Get the first task that starts after the current time (assuming Tasks are sorted)
            if (task.Start > Time)
            {
                NextTask = task;
                
                return;
            }
        }

        NextTask = null;
    }

    public void ShowNextTaskNotification() {
        var notificationService = new NotificationService();
        notificationService.ShowNextTask(NextTask);
    }


    public DateOnly Date
    {
        set; get;
    }
    private void UpdateDate() => Date = DateOnly.FromDateTime(DateTime.Now);


    public void Innit()
    {
        _dao = new MockDaoService();
        getTodayTasks();
        getWeekTasks();
        UpdateDate();
        Time = TimeOnly.FromDateTime(DateTime.Now);
        StartTimer();
    }

    private void getTodayTasks() {
        if (_dao == null) { 
            return;
        }
        var tasks = _dao.GetAllTasks();
        TodayTasks = tasks;
    }

    private void getWeekTasks()
    {
        if (_dao == null)
        {
            return;
        }
        ThisWeekTasks = _dao.GetThisWeekTasks();
    }

    




    //private void 

    public DashboardViewModel()
    {
        Innit();
    }
}
