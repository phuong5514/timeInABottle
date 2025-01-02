using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models.Tasks;
using TimeInABottle.Core.Models.Weather;
namespace TimeInABottle.ViewModels;

/// <summary>
/// ViewModel for the Dashboard, providing properties and methods to manage tasks and weather information.
/// </summary>
public partial class DashboardViewModel : ObservableRecipient
{
    private readonly IDaoService? _dao;

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

    private readonly DispatcherTimer _timer = new();
    private void UpdateTime(object? sender, object? e) => Time = TimeOnly.FromDateTime(DateTime.Now);

    private WeatherInfoWrapper _weather = null!;
    public WeatherInfoWrapper Weather
    {
        get => _weather;
        private set
        {
            if (_weather != value)
            {
                _weather = value;
                OnPropertyChanged(nameof(Weather));
            }
        }
    }
    private void UpdateWeather(object? sender, object? e) => Weather = App.GetService<IWeatherService>().GetCurrentWeather();

    private void StartTimer()
    {
        _timer.Interval = TimeSpan.FromSeconds(15); // TODO: config file / setting page options
        _timer.Tick += UpdateTime;
        _timer.Tick += UpdateWeather;
        _timer.Start();
    }

    public FullObservableCollection<ITask> TodayTasks { get; set; } = new FullObservableCollection<ITask>();
    public FullObservableCollection<ITask> ThisWeekTasks { get; set; } = new FullObservableCollection<ITask>();

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

    public DateOnly Date
    {
        set; get;
    }
    private void UpdateDate() => Date = DateOnly.FromDateTime(DateTime.Now);

    private void getTodayTasks()
    {
        if (_dao == null)
        {
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

    public DashboardViewModel()
    {
        _dao = App.GetService<IDaoService>();
        UpdateDate();
        LoadData();
        Time = TimeOnly.FromDateTime(DateTime.Now);
        Weather = App.GetService<IWeatherService>().GetCurrentWeather();
        StartTimer();
    }
}
