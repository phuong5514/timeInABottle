using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Core.Models;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace TimeInABottle.Services;
public class NotificationService
{
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

