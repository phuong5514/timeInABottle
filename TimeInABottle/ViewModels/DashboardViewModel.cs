using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models.Tasks;
using TimeInABottle.Core.Models.Weather;
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
    
    private WeatherInfoWrapper _weather;
    public WeatherInfoWrapper Weather
    {
        get => _weather;
        private set
        {
            if (_weather!= value)
            {
                _weather = value;
                OnPropertyChanged(nameof(Weather));
            }
        }
    }
    private void UpdateWeather(object sender, object e) => Weather = App.GetService<IWeatherService>().GetCurrentWeather();

    private void StartTimer()
    {
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(15) // TODO: config file / setting page options
        };
        _timer.Tick += UpdateTime;
        _timer.Tick += UpdateWeather;
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

    public int TaskTimeUnit = int.TryParse(ConfigHandler.GetConfigValue("TimeSlotIncrement"), out var result) ? result : 15;


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

    //public void ShowNextTaskNotification() {
    //    var notificationService = new NotificationService();
    //    notificationService.ShowNextTask(NextTask);
    //}


    public DateOnly Date
    {
        set; get;
    }
    private void UpdateDate() => Date = DateOnly.FromDateTime(DateTime.Now);


    public void Innit()
    {

        _dao = App.GetService<IDaoService>();
        UpdateDate();
        LoadData();
        Time = TimeOnly.FromDateTime(DateTime.Now);
        Weather = App.GetService<IWeatherService>().GetCurrentWeather();
        StartTimer();
    }

    private void getTodayTasks() {
        if (_dao == null) { 
            return;
        }
        var tasks = _dao.GetTodayTasks();
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



    public void LoadData()
    {
        getTodayTasks(); 
        getWeekTasks();
    }


    

    //private void 

    public DashboardViewModel()
    {
        Innit();
    }
}
