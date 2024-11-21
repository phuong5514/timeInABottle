using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Contracts.Services;
using Windows.ApplicationModel.Background;

namespace TimeInABottle.Services;
public class BackgroundTaskRegisterService : IBackgroundTaskRegisterService
{
    public void RegisterBackgroundTask(string name, string entrypoint, IBackgroundTrigger trigger)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Task name cannot be null or empty.", nameof(name));
        }

        if (CheckBackgroundTaskRegistration(name))
        {
            return;
        }

        if (!CheckPermissionAsync().Result)
        {
            return;
        }

        var builder = new BackgroundTaskBuilder
        {
            Name = name,
            TaskEntryPoint = entrypoint
        };
        builder.SetTrigger(trigger);
        builder.Register();
    }

    public void RegisterBackgroundTask(string name, string entrypoint, IBackgroundTrigger trigger, IBackgroundCondition condition)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Task name cannot be null or empty.", nameof(name));
        }

        if (CheckBackgroundTaskRegistration(name))
        {
            return;
        }

        if (!CheckPermissionAsync().Result)
        {
            return;
        }

        var builder = new BackgroundTaskBuilder
        {
            Name = name,
            TaskEntryPoint = entrypoint
        };
        builder.SetTrigger(trigger);
        builder.AddCondition(condition);
        builder.Register();
    }

    public void UnregisterBackgroundTask(string name)
    {
        for (var i = 0; i < BackgroundTaskRegistration.AllTasks.Count; i++)
        {
            var task = BackgroundTaskRegistration.AllTasks.ElementAt(i);
            if (task.Value.Name == name)
            {
                task.Value.Unregister(true);
                break;
            }
        }
    }

    protected bool CheckBackgroundTaskRegistration(string name)
    {
        var taskRegistered = false;

        foreach (var task in BackgroundTaskRegistration.AllTasks)
        {
            if (task.Value.Name == name)
            {
                taskRegistered = true;
                break;
            }
        }
        return taskRegistered;
    }

    protected async Task<bool> CheckPermissionAsync()
    {
        var accessStatus = await BackgroundExecutionManager.RequestAccessAsync();
        if (accessStatus != BackgroundAccessStatus.AlwaysAllowed &&
            accessStatus != BackgroundAccessStatus.AllowedSubjectToSystemPolicy)
        {
            return false;
        }
        return true;
    }
}
