using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Contracts.Services;
using Windows.ApplicationModel.Background;

namespace TimeInABottle.BackgroundTasks;

partial class NotificationBackgroundTasks : IBackgroundTask
{
    BackgroundTaskDeferral _deferral; // Note: defined at class scope so that we can mark it complete inside the OnCancel() callback if we choose to support cancellation
    public void Run(IBackgroundTaskInstance? taskInstance)
    {
        if (taskInstance != null)
        {
            _deferral = taskInstance.GetDeferral();
        }
       
       
        var service = App.GetService<INotificationService>();
        service.SendNotification();
       

        if (_deferral != null)
        {
            _deferral.Complete();
        }
    }
}
