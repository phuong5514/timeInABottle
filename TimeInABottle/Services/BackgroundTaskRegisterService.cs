using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using TimeInABottle.Contracts.Services;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using WinUIEx.Messaging;

namespace TimeInABottle.Services;
/// <summary>
/// Service for registering and managing background tasks.
/// </summary>
public class BackgroundTaskRegisterService : IBackgroundTaskRegisterService
{
    /// <summary>
    /// Registers a background task with the specified name, entry point, and trigger.
    /// </summary>
    /// <param name="name">The name of the background task.</param>
    /// <param name="entrypoint">The entry point of the background task.</param>
    /// <param name="trigger">The trigger for the background task.</param>
    /// <exception cref="ArgumentException">Thrown when the task name is null or empty.</exception>
    public async void RegisterBackgroundTask(string name, string entrypoint, IBackgroundTrigger trigger)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Task name cannot be null or empty.", nameof(name));
        }

        if (CheckBackgroundTaskRegistration(name))
        {
            return;
        }

        var permission = await CheckPermissionAsync();
        if (!permission)
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

    /// <summary>
    /// Registers a background task with the specified name, entry point, trigger, and condition.
    /// </summary>
    /// <param name="name">The name of the background task.</param>
    /// <param name="entrypoint">The entry point of the background task.</param>
    /// <param name="trigger">The trigger for the background task.</param>
    /// <param name="condition">The condition for the background task.</param>
    /// <exception cref="ArgumentException">Thrown when the task name is null or empty.</exception>
    public async void RegisterBackgroundTaskAsync(string name, string entrypoint, IBackgroundTrigger trigger, IBackgroundCondition condition)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Task name cannot be null or empty.", nameof(name));
        }

        if (CheckBackgroundTaskRegistration(name))
        {
            return;
        }

        var permission = await CheckPermissionAsync();
        if (!permission)
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

    /// <summary>
    /// Unregisters a background task with the specified name.
    /// </summary>
    /// <param name="name">The name of the background task to unregister.</param>
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

    /// <summary>
    /// Unregisters all background tasks.
    /// </summary>
    public void CleanRegister()
    {
        foreach (var task in BackgroundTaskRegistration.AllTasks)
        {
            task.Value.Unregister(true);
        }
    }

    /// <summary>
    /// Checks if a background task with the specified name is already registered.
    /// </summary>
    /// <param name="name">The name of the background task.</param>
    /// <returns>True if the background task is already registered; otherwise, false.</returns>
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

    /// <summary>
    /// Checks if the application has permission to run background tasks.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the application has permission to run background tasks.</returns>
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
