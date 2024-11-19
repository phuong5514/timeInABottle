using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace TimeInABottle.BackgroundTasks;

partial class NotificationBackgroundTasks : IBackgroundTask
{
    BackgroundTaskDeferral _deferral; // Note: defined at class scope so that we can mark it complete inside the OnCancel() callback if we choose to support cancellation
    public void Run(IBackgroundTaskInstance taskInstance)
    {
        _deferral = taskInstance.GetDeferral();
        //
        // TODO: Insert code to start one or more asynchronous methods using the
        //       await keyword, for example:
        //
        // await ExampleMethodAsync();
        //

        _deferral.Complete();
    }
}
