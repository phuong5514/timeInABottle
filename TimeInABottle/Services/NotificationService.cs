using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Contracts.Services;
using TimeInABottle.Core.Models;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace TimeInABottle.Services;
/// <summary>
/// Service for handling notifications.
/// </summary>
public partial class NotificationService : INotificationService
{
    /// <summary>
    /// Displays a notification for the next task.
    /// </summary>
    /// <param name="task">The task to be displayed in the notification.</param>
    public void CreateNotification(string v, object deadline) => throw new NotImplementedException();
    public object GetNotifications() => throw new NotImplementedException();

    public void ShowNextTask(ITask task)
    {
        var title = "No more tasks left!";
        var message = "Enjoy your freetime!";

        if (task != null)
        {
            title = $"Up next at {task.Start}:";
            message = $"{task.Name}\n {task.Description}";
        }
        ShowToast(title, message);
    }

    /// <summary>
    /// Displays a toast notification with the specified title and message.
    /// </summary>
    /// <param name="title">The title of the toast notification.</param>
    /// <param name="message">The message of the toast notification.</param>
    private void ShowToast(string title, string message)
    {
        var toastXmlString = $@"
        <toast>
            <visual>
                <binding template='ToastGeneric'>
                    <text>{title}</text>
                    <text>{message}</text>
                </binding>
            </visual>
        </toast>";

        var toastXml = new XmlDocument();
        toastXml.LoadXml(toastXmlString);
        var toast = new ToastNotification(toastXml);
        ToastNotificationManager.CreateToastNotifier().Show(toast);
    }
}

