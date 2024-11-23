using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models;
using TimeInABottle.Core.Services;
using System;


namespace TimeInABottle.Background;

public sealed class NotificationBackgroundTasks : IBackgroundTask
{

    private BackgroundTaskDeferral? _taskDeferral;
    private static IDaoService _dao;
    private static FullObservableCollection<ITask> _todayTasks;
    private static int _index;

    public async void Run(IBackgroundTaskInstance? taskInstance)
    {
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
        var taskToSend = _todayTasks[_index];

        XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);
        XmlNodeList textElements = toastXml.GetElementsByTagName("text");
        textElements[0].AppendChild(toastXml.CreateTextNode("Up next"));
        textElements[1].AppendChild(toastXml.CreateTextNode($"{taskToSend.Name}"));
        textElements[2].AppendChild(toastXml.CreateTextNode($"At {taskToSend.Start}"));

        ToastNotification notification = new(toastXml);
        ToastNotificationManager.CreateToastNotifier().Show(notification);
    }

    private bool ShouldSendNotification()
    {
        CallibrateIndex();
        if (_index >= _todayTasks.Count)
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

