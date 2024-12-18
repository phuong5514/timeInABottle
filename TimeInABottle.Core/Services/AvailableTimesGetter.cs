using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Core.Services;
public class AvailableTimesGetter : IAvailableTimesGetter
{
    private readonly int _timeIncrement;

    private readonly IDaoService _daoQueryService;
    public AvailableTimesGetter(IDaoService daoQueryService, int timeIncrement)
    {
        _daoQueryService = daoQueryService;
        _timeIncrement = timeIncrement;
    }

    private IEnumerable<TimeSpan> GetAvailableTimes(IEnumerable<ITask> taskList) 
    {
        var startTime = TimeSpan.FromHours(0); // 12:00 AM
        var endTime = TimeSpan.FromHours(24); // 11:59 PM
        var increment = TimeSpan.FromMinutes(_timeIncrement); // 30-minute intervals

        var availableTimes = new List<TimeSpan>();

        for (var time = startTime; time < endTime; time += increment)
        {
            foreach (var task in taskList)
            {
                var blockedStartTime = task.Start.ToTimeSpan();

                var blockedEndTime = task.End.ToTimeSpan();

                if (time < blockedStartTime || time >= blockedEndTime)
                {
                    availableTimes.Add(time);
                }
            }
        }

        return availableTimes;
    }

    public IEnumerable<TimeSpan> GetAvailableTimesForDate(DateOnly date) { 
        var tasks = _daoQueryService.FindTaskFromDate(date);
        return GetAvailableTimes(tasks);
    }

    public IEnumerable<TimeSpan> GetAvailableTimesForToday() { 
        var tasks = _daoQueryService.GetTodayTasks();

        return GetAvailableTimes(tasks);
    }

    public IEnumerable<TimeSpan> GetAvailableTimesForWeek() { 
        var tasks = _daoQueryService.GetThisWeekTasks();
        return GetAvailableTimes(tasks);
    }

    public IEnumerable<TimeSpan> GetAvailableTimesForWeek(IEnumerable<DayOfWeek> weekdays)
    {
        var tasks = _daoQueryService.GetThisWeekTasks(weekdays);
        return GetAvailableTimes(tasks);
    }

    public IEnumerable<TimeSpan> GetAvailableTimesForDate(int date) {
        var dateOnly = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, date);
        return GetAvailableTimesForDate(dateOnly);
    }
}
