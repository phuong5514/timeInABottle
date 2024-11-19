using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Composition;
using TimeInABottle.Contracts.Services;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models;

namespace TimeInABottle.Services;

public partial class NotificationService : INotificationService
{
    private IDaoService _dao;
    private FullObservableCollection<ITask> _todayTasks;
    private int _index;

    public NotificationService()
    {
        InnitDao();
        LoadTasksList();
        StartNotificationScheduler();
    }

    private void InnitDao()
    {
        _dao = App.GetService<IDaoService>();
    }

    private void LoadTasksList()
    {
        if (_dao == null)
        {
            InnitDao();
        }
        //_todayTasks = _dao.GetTodayTasks();
        _todayTasks = _dao.GetAllTasks();
        CallibrateIndex();
    }

    private void CallibrateIndex()
    {
        for (_index = 0; _index < _todayTasks.Count; _index++) {
            if (_todayTasks[_index].Start > TimeOnly.FromDateTime(DateTime.Now)) {
                break;
            }
        }
    }

    private async void StartNotificationScheduler()
    {
        await Task.Run(() =>
        {
            while (true) // Simulate a long-running task
            {
                var now = DateTime.Now;
                CallibrateIndex();

                // Check for notifications to push
                if (ShouldSendNotification(now))
                {
                    //SendNotification("It's time for your task!");

                    this.ShowNextTask(_todayTasks[_index]);
                }


                // Sleep for a while to reduce CPU usage
                // also double served as a reminder notification
                Task.Delay(300000).Wait(); // Sleep for 5 minutes (300000 milliseconds) TODO: config
            }
        });
    }

    private bool ShouldSendNotification(DateTime now)
    {
        // Check if the index is out of range
        if (_index >= _todayTasks.Count)
        {
            return false;
        }

        // Check if the task start time is within 60 minutes from now
        var taskStartTime = _todayTasks[_index].Start;
        var currentTime = TimeOnly.FromDateTime(now);
        var notificationTime = currentTime.AddMinutes(60); // TODO: config this minute value

        return taskStartTime <= notificationTime;
    }

}
