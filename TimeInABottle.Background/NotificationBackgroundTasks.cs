using System.Diagnostics;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Services;
using System;

using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using TimeInABottle.Core.Models.Tasks;


namespace TimeInABottle.Background;

public sealed class NotificationBackgroundTasks : IBackgroundTask
{

    private BackgroundTaskDeferral? _taskDeferral;
    private static IDaoService? _dao;
    private static IBufferService? _bufferService;
    private static FullObservableCollection<ITask>? _todayTasks;
    private static int _index;

    public void Run(IBackgroundTaskInstance? taskInstance)
    {
        if (taskInstance == null)
        {
            throw new ArgumentNullException(nameof(taskInstance));
        }

        taskInstance.Canceled += TaskInstance_Canceled;

        Debug.WriteLine("Background " + taskInstance.Task.Name + " Starting...");
        _taskDeferral = taskInstance.GetDeferral();

        try
        {
            _dao = new MockDaoService();
            LoadTasksList();

            _bufferService = new WeatherBasedBufferService();
            _bufferService.LoadBuffer();

            if (ShouldSendNotification()) SendToast();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in background task: {ex.Message}");
        }
        finally
        {
            Debug.WriteLine($"Background {taskInstance.Task.Name} Completed.");
            _taskDeferral.Complete();
        }
    }

    private static void SendToast()
    {
        if (_todayTasks == null || _todayTasks.Count == 0 || _bufferService == null)
        {
            Debug.WriteLine("No tasks available to send notification or bufferService is null.");
            return;
        }

        var taskToSend = _todayTasks[_index];
        var buffer = _bufferService.BufferSize;
        if (buffer < 0) buffer = 0; // Ensure buffer size is non-negative

        var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);
        var textElements = toastXml.GetElementsByTagName("text");

        var adjustedStartTime = taskToSend.Start.AddMinutes(-buffer);
        textElements[0].AppendChild(toastXml.CreateTextNode($"Up next at {taskToSend.Start}, should start {buffer}m earlier at {adjustedStartTime}"));
        textElements[1].AppendChild(toastXml.CreateTextNode($"{taskToSend.Name}"));

        //var buffer = bufferService.BufferSize;
        //if (buffer > 0) {
        //    var adjustedStartTime = taskToSend.Start.AddMinutes(-bufferService.BufferSize);
        //    textElements[0].AppendChild(toastXml.CreateTextNode($"Up next at {taskToSend.Start}, should start {buffer}m earlier at {adjustedStartTime}"));
        //} else {
        //    textElements[0].AppendChild(toastXml.CreateTextNode($"Up next at {taskToSend.Start}"));
        //}
        //textElements[1].AppendChild(toastXml.CreateTextNode($"{taskToSend.Name}"));

        try
        {
            var notification = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(notification);
            Debug.WriteLine("Notification sent successfully.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error sending notification: {ex.Message}");
        }
    
    }

    private bool ShouldSendNotification()
    {
        CallibrateIndex();

        if (_todayTasks == null || _index >= _todayTasks.Count || _bufferService == null) return false;

        var now = DateTime.Now;
        var adjustedStartTime = _todayTasks[_index].Start.AddMinutes(-_bufferService.BufferSize);

        return adjustedStartTime <= TimeOnly.FromDateTime(now).AddMinutes(60);
    }

    private void LoadTasksList()
    {
        if (_dao == null)
        {
            Debug.WriteLine("DaoService is null. Cannot load tasks.");
            return;
        }
        _todayTasks = _dao.GetAllTasks();
    }

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

