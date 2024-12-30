using System.Reflection;
using System.Windows.Input;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;

using TimeInABottle.Contracts.Services;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Helpers;

using Windows.ApplicationModel;
using Windows.ApplicationModel.Background;

namespace TimeInABottle.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService _themeSelectorService;

    //private readonly string _filename = "settings.config";
    //private readonly string _configPath;

    [ObservableProperty]
    private ElementTheme _elementTheme;

    [ObservableProperty]
    private string _versionDescription;

    partial void OnTimeSlotIncrementChanged();
    partial void OnTimeSlotIncrementChanged() => ConfigHandler.SetConfigValue("TimeSlotIncrement", TimeSlotIncrement.ToString());
    [ObservableProperty]
    private int _timeSlotIncrement; // calendar time slot "size" in minutes (e.g. 15, 30, 60)

    [ObservableProperty]
    private List<int> _timeSlotIncrements; // possible time slot increments

    [ObservableProperty]
    private bool _isNotificationEnabled; // whether or not the notification (background task) is enabled

    [ObservableProperty]
    private int _backgroundTaskRefreshRate; // how long is the time between each background task runs in minutes

    [ObservableProperty]
    private int _notificationTime; // how long before the event the notification should be shown in minutes

    [ObservableProperty]
    private int _notificationDuration; // how long the notification should be shown in seconds

    [ObservableProperty]
    private TimeSpan _schedulingStartTime; // the earliest time an event can be scheduled

    [ObservableProperty]
    private TimeSpan _schedulingEndTime; // the latest time an event can be scheduled

    public ICommand SwitchThemeCommand
    {
        get;
    }

    public ICommand NotificationSaveCommand;
    public ICommand ScheduleSaveCommand;

    public SettingsViewModel(IThemeSelectorService themeSelectorService)
    {
        _themeSelectorService = themeSelectorService;
        _elementTheme = _themeSelectorService.Theme;
        _versionDescription = GetVersionDescription();

        //_configPath = Path.Combine(AppContext.BaseDirectory, _filename);
        ReadConfig();

        SwitchThemeCommand = new RelayCommand<ElementTheme>(
            async (param) =>
            {
                if (ElementTheme != param)
                {
                    ElementTheme = param;
                    await _themeSelectorService.SetThemeAsync(param);
                }
            });

        NotificationSaveCommand = new RelayCommand(SaveNotificationSettings);
        ScheduleSaveCommand = new RelayCommand(SaveScheduleSettings);
    }

    private void SaveNotificationSettings()
    {
        ConfigHandler.SetConfigValue("BackgroundTaskRefreshRate", BackgroundTaskRefreshRate.ToString());


        var registerService = App.GetService<IBackgroundTaskRegisterService>();
        if (IsNotificationEnabled)
        {
            registerService.RegisterBackgroundTask((uint)BackgroundTaskRefreshRate);
            var dictionary = new Dictionary<string, string>
            {
                ["BackgroundTaskRefreshRate"] = BackgroundTaskRefreshRate.ToString(),
                ["NotificationTime"] = NotificationTime.ToString(),
                ["NotificationDuration"] = NotificationDuration.ToString()
            };
            ConfigHandler.SetConfigValues(dictionary);
        }
        else { 
            registerService.CleanRegister();
        }
    }

    private void SaveScheduleSettings()
    {
        if (SchedulingEndTime < SchedulingStartTime)
        {
            // set error message
            return;
        }

        // save settings
        ConfigHandler.SetConfigValue("SchedulingStartTime", SchedulingStartTime.ToString());
        ConfigHandler.SetConfigValue("SchedulingEndTime", SchedulingEndTime.ToString());
    }

    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            var packageVersion = Package.Current.Id.Version;

            version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }

    private void ReadConfig()
    {
        var configurations = (List<string>)ConfigHandler.GetConfigValues(["TimeSlotIncrement", "IsNotificationEnabled", "BackgroundTaskRefreshRate", "NotificationTime", "NotificationDuration", "SchedulingStartTime", "SchedulingEndTime"]);

        // general
        TimeSlotIncrement = int.Parse(configurations[0]);
        var listString = ConfigHandler.GetConfigValue("TimeSlotIncrements");
        TimeSlotIncrements = listString.Split(',').Select(int.Parse).ToList(); // Parse the string into a list of integers

        // notification
        IsNotificationEnabled = bool.Parse(configurations[1]);
        BackgroundTaskRefreshRate = int.Parse(configurations[2]);
        NotificationTime = int.Parse(configurations[3]);
        NotificationDuration = int.Parse(configurations[4]);

        // scheduling
        SchedulingStartTime = TimeSpan.Parse(configurations[5]);
        SchedulingEndTime = TimeSpan.Parse(configurations[6]);

        var registerService = App.GetService<IBackgroundTaskRegisterService>();
        if (IsNotificationEnabled)
        {
            registerService.RegisterBackgroundTask((uint)BackgroundTaskRefreshRate);
        }
        else
        {
            registerService.CleanRegister();
        }


    }
}
