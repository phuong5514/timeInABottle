using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace TimeInABottle.Contracts.Services;
public interface IBackgroundTaskRegisterService
{
    public void RegisterBackgroundTask(string name, string entrypoint, IBackgroundTrigger trigger);
    public void UnregisterBackgroundTask(string name);

}
