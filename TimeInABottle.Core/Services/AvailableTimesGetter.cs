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

    private Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimes(IEnumerable<ITask> taskList, TimeSpan startTime, TimeSpan endTime)
    {
        var increment = TimeSpan.FromMinutes(_timeIncrement);

        var availableStartTimes = new List<TimeSpan>();
        var availableEndTimes = new List<TimeSpan>();

        var tasks = taskList.ToList();

        if (tasks.Count == 0)
        {
            for (var time = startTime; time < endTime; time += increment)
            {
                availableStartTimes.Add(time);
                availableEndTimes.Add(time);
            }
        }
        else
        {
            for (var time = startTime; time < endTime; time += increment)
            {
                var isAvailableStart = true;
                var isAvailableEnd = true;

                foreach (var task in tasks)
                {
                    var blockedStartTime = task.Start.ToTimeSpan();
                    var blockedEndTime = task.End.ToTimeSpan();

                    if (time >= blockedStartTime && time < blockedEndTime)
                    {
                        isAvailableStart = false;
                    }

                    if (time > blockedStartTime && time <= blockedEndTime)
                    {
                        isAvailableEnd = false;
                    }

                    if (!isAvailableStart && !isAvailableEnd)
                    {
                        break;
                    }
                }

                if (isAvailableStart)
                {
                    availableStartTimes.Add(time);
                }

                if (isAvailableEnd)
                {
                    availableEndTimes.Add(time);
                }
            }
        }

        return Tuple.Create((IEnumerable<TimeSpan>)availableStartTimes, (IEnumerable<TimeSpan>)availableEndTimes);
    }

    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimes(IEnumerable<ITask> taskList)
    {
        var startTime = TimeSpan.FromHours(0); // 12:00 AM
        var endTime = TimeSpan.FromHours(24); // 11:59 PM

        return GetAvailableTimes(taskList, startTime, endTime);
        
    }

    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForDate(DateOnly date)
    {
        var tasks = _daoQueryService.FindTaskFromDate(date);
        return GetAvailableTimes(tasks);
    }

    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForToday()
    {
        var tasks = _daoQueryService.GetTodayTasks();

        return GetAvailableTimes(tasks);
    }

    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForWeek()
    {
        var tasks = _daoQueryService.GetThisWeekTasks();
        return GetAvailableTimes(tasks);
    }

    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForWeek(IEnumerable<DayOfWeek> weekdays)
    {
        var tasks = _daoQueryService.GetThisWeekTasks(weekdays);
        return GetAvailableTimes(tasks);
    }

    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForDate(int date)
    {
        var dateOnly = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, date);
        return GetAvailableTimesForDate(dateOnly);
    }

    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForNextWeek()
    {
        throw new NotImplementedException();
    }

    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForWeekFromNow()
    {
        var tasks = _daoQueryService.GetThisWeekTasksFromNow();
        return GetAvailableTimes(tasks);
    }

    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForTodayFromNow()
    {
        var tasks = _daoQueryService.GetTodayTasks();
        var endTime = TimeSpan.FromHours(24); // 11:59 PM

        var startTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);

        if (startTime.Minutes < 30)
        {
            startTime = new TimeSpan(startTime.Hours, 30, 0);
        }
        else
        {
            startTime = new TimeSpan(startTime.Hours + 1, 0, 0);
        }

        return GetAvailableTimes(tasks, startTime, endTime);
    }
}
