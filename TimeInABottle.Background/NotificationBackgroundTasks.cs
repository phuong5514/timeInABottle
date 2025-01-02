using System.Diagnostics;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Services;
using System;

using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using TimeInABottle.Core.Models.Tasks;
using System.Collections.Generic;


/// <summary>
/// Represents a background task that handles notifications.
/// </summary>
/// <remarks>
/// This class implements the <see cref="IBackgroundTask"/> interface and is responsible for
/// managing the lifecycle of a background task, including initialization, execution, and cancellation.
/// It interacts with various services to load tasks, read configurations, and send toast notifications.
/// </remarks>
namespace TimeInABottle.Background;

public sealed class NotificationBackgroundTasks : IBackgroundTask
{

    private BackgroundTaskDeferral? _taskDeferral;
    private static IDaoService? _dao;
    private static IBufferService? bufferService;
    private static FullObservableCollection<ITask>? _todayTasks;
    private static int _index;

    private int _notificationTime;
    private int _notificationDuration;
    private int _refreshRate;

    /// <summary>
    /// Executes the background task.
    /// </summary>
    /// <param name="taskInstance">The instance of the background task.</param>
    /// <exception cref="ArgumentNullException">Thrown when the task instance is null.</exception>
    public void Run(IBackgroundTaskInstance? taskInstance)
    {
        if (taskInstance == null)
        {
            throw new ArgumentNullException(nameof(taskInstance));
        }

        taskInstance.Canceled += TaskInstance_Canceled;
        //Debug.WriteLine("Background " + taskInstance.Task.Name + " Starting...");
        _taskDeferral = taskInstance.GetDeferral();

        //Debug.WriteLine("Background " + taskInstance.Task.Name + " Starting...");
        // cant use dependency injection in background tasks due to the way they are instantiated (out of process background task)
        // cant use sqliteDao because of unknown reason (cant find a dependency sqlite3.dll file) (hypothesis: how this project is compile)
        _dao = new LocalStorageDaoService();
        //_dao = new SqliteDaoService();

        readConfig();
        LoadTasksList();

        bufferService = new WeatherBasedBufferService();
        bufferService.LoadBuffer();

        if (ShouldSendNotification())
        {
            SendToast();
        }

        Debug.WriteLine("Background " + taskInstance.Task.Name + " Completed.");
        _taskDeferral.Complete();
    }

    /// <summary>
    /// Reads the configuration values for notification time, duration, and refresh rate.
    /// </summary>
    private void readConfig()
    {
        var configurations = (List<string>)ConfigHandler.GetConfigValues(["NotificationTime", "NotificationDuration", "BackgroundTaskRefreshRate"]);

        _notificationTime = int.Parse(configurations[0]);
        _notificationDuration = int.Parse(configurations[1]);
        _refreshRate = int.Parse(configurations[2]);
    }

    /// <summary>
    /// Sends a toast notification for the next task.
    /// </summary>
    private void SendToast()
    {
        if (_todayTasks == null || _todayTasks.Count == 0 || bufferService == null)
        {
            return;
        }

        var taskToSend = _todayTasks[_index];

        var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);
        var textElements = toastXml.GetElementsByTagName("text");

        var buffer = bufferService.BufferSize;
        var adjustedStartTime = taskToSend.Start.AddMinutes(-buffer);
        var notificationText = buffer > 0
            ? $"Up next at {taskToSend.Start}, should start {buffer}m earlier at {adjustedStartTime}"
            : $"Up next at {taskToSend.Start}";

        textElements[0].AppendChild(toastXml.CreateTextNode(notificationText));
        textElements[1].AppendChild(toastXml.CreateTextNode($"{taskToSend.Name}"));

        ToastNotification notification = new(toastXml)
        {
            ExpirationTime = DateTimeOffset.Now.AddSeconds(_notificationDuration)
        };

        ToastNotificationManager.CreateToastNotifier().Show(notification);
    }

    /// <summary>
    /// Determines whether a notification should be sent based on the current time and task schedule.
    /// </summary>
    /// <returns>True if a notification should be sent; otherwise, false.</returns>
    private bool ShouldSendNotification()
    {
        CallibrateIndex();
        if (_todayTasks == null || _index >= _todayTasks.Count || bufferService == null)
        {
            return false;
        }

        var now = DateTime.Now;

        var taskStartTime = _todayTasks[_index].Start;
        var adjustedStartTime = taskStartTime.AddMinutes(-bufferService.BufferSize);

        var currentTime = TimeOnly.FromDateTime(now);

        var total = _notificationTime + _refreshRate; // adding refresh rate to prevent some cases where the notification is not sent when it should due to "cooldown" of the background task
        var notificationTime = currentTime.AddMinutes(total);

        return adjustedStartTime <= notificationTime;
    }

    /// <summary>
    /// Loads the list of tasks scheduled for today.
    /// </summary>
    private void LoadTasksList()
    {
        if (_dao == null)
        {
            return;
        }
        _todayTasks = _dao.GetTodayTasks();
    }

    /// <summary>
    /// Calibrates the index to point to the next upcoming task.
    /// </summary>
    private void CallibrateIndex()
    {
        if (_todayTasks == null)
        {
            return;
        }

        for (_index = 0; _index < _todayTasks.Count; _index++)
        {
            if (_todayTasks[_index].Start > TimeOnly.FromDateTime(DateTime.Now))
            {
                break;
            }
        }
    }



    private void TaskInstance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
    {
        if (_taskDeferral != null)
        {
            _taskDeferral.Complete();
            Debug.WriteLine("Background task terminated");
        }
    }
}

