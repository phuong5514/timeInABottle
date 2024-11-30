using System.Diagnostics;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Services;
using System;

using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using TimeInABottle.Core.Models.Tasks;


namespace TimeInABottle.Background.BackgroundTasks;

public sealed class NotificationBackgroundTasks : IBackgroundTask
{

    private BackgroundTaskDeferral? _taskDeferral;
    private static IDaoService? _dao;
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

        Debug.WriteLine("Background " + taskInstance.Task.Name + " Starting...");
        _dao = new MockDaoService();
        LoadTasksList();

        if (ShouldSendNotification())
        {
            SendToast();
        }

        Debug.WriteLine("Background " + taskInstance.Task.Name + " Completed.");
        _taskDeferral.Complete();
    }

    private static void SendToast()
    {
        if (_todayTasks == null || _todayTasks.Count == 0)
        {
            Debug.WriteLine("No tasks available to send notification.");
            return;
        }

        var taskToSend = _todayTasks[_index];

        var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);
        var textElements = toastXml.GetElementsByTagName("text");
        textElements[0].AppendChild(toastXml.CreateTextNode($"Up next at {taskToSend.Start}"));
        textElements[1].AppendChild(toastXml.CreateTextNode($"{taskToSend.Name}"));

        ToastNotification notification = new(toastXml);
        ToastNotificationManager.CreateToastNotifier().Show(notification);
    }

    private bool ShouldSendNotification()
    {
        CallibrateIndex();
        if (_todayTasks == null || _index >= _todayTasks.Count)
        {
            return false;
        }

        var now = DateTime.Now;
        var taskStartTime = _todayTasks[_index].Start;
        var currentTime = TimeOnly.FromDateTime(now);
        var notificationTime = currentTime.AddMinutes(60);

        return taskStartTime <= notificationTime;
    }

    private void LoadTasksList()
    {
        if (_dao == null)
        {
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

