
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Core.Services;
public class AvailableTimesGetter : IAvailableTimesGetter
{
    private readonly int _timeIncrement;

    private readonly IDaoService _daoQueryService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AvailableTimesGetter"/> class.
    /// </summary>
    /// <param name="daoQueryService">The DAO query service.</param>
    public AvailableTimesGetter(IDaoService daoQueryService)
    {
        _daoQueryService = daoQueryService;
        _timeIncrement = int.Parse(ConfigHandler.GetConfigValue("TimeSlotIncrement"));
    }

    /// <summary>
    /// Gets the available times based on the provided task list, start time, and end time.
    /// </summary>
    /// <param name="taskList">The list of tasks to consider.</param>
    /// <param name="startTime">The start time to consider.</param>
    /// <param name="endTime">The end time to consider.</param>
    /// <returns>A tuple containing two enumerables of available time spans.</returns>
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

    /// <summary>
    /// Gets the available times based on the provided task list.
    /// </summary>
    /// <param name="taskList">The list of tasks to consider.</param>
    /// <returns>A tuple containing two enumerables of available time spans.</returns>
    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimes(IEnumerable<ITask> taskList)
    {
        var startTime = TimeSpan.FromHours(0); // 12:00 AM
        var endTime = TimeSpan.FromHours(24); // 11:59 PM

        return GetAvailableTimes(taskList, startTime, endTime);

    }

    /// <summary>
    /// Gets the available times for a specific date.
    /// </summary>
    /// <param name="date">The date to consider.</param>
    /// <returns>A tuple containing two enumerables of available time spans for the specified date.</returns>
    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForDate(DateOnly date)
    {
        var tasks = _daoQueryService.FindTaskFromDate(date);
        return GetAvailableTimes(tasks);
    }

    /// <summary>
    /// Gets the available times for today.
    /// </summary>
    /// <returns>A tuple containing two enumerables of available time spans for today.</returns>
    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForToday()
    {
        var tasks = _daoQueryService.GetTodayTasks();

        return GetAvailableTimes(tasks);
    }

    /// <summary>
    /// Gets the available times for the current week.
    /// </summary>
    /// <returns>A tuple containing two enumerables of available time spans for the current week.</returns>
    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForWeek()
    {
        var tasks = _daoQueryService.GetThisWeekTasks();
        return GetAvailableTimes(tasks);
    }

    /// <summary>
    /// Gets the available times for a specific week based on the provided weekdays.
    /// </summary>
    /// <param name="weekdays">The weekdays to consider.</param>
    /// <returns>A tuple containing two enumerables of available time spans for the specified week.</returns>
    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForWeek(IEnumerable<DayOfWeek> weekdays)
    {
        var tasks = _daoQueryService.GetThisWeekTasks(weekdays);
        return GetAvailableTimes(tasks);
    }

    /// <summary>
    /// Gets the available times for a specific date represented as an integer.
    /// </summary>
    /// <param name="date">The date to consider as an integer.</param>
    /// <returns>A tuple containing two enumerables of available time spans for the specified date.</returns>
    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForDate(int date)
    {
        var dateOnly = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, date);
        return GetAvailableTimesForDate(dateOnly);
    }

    /// <summary>
    /// Gets the available times for the next week.
    /// </summary>
    /// <returns>A tuple containing two enumerables of available time spans for the next week.</returns>
    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForNextWeek()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets the available times for a week starting from now.
    /// </summary>
    /// <returns>A tuple containing two enumerables of available time spans for a week from now.</returns>
    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForWeekFromNow()
    {
        var tasks = _daoQueryService.GetThisWeekTasksFromNow();
        return GetAvailableTimes(tasks);
    }

    /// <summary>
    /// Gets the available times for today starting from now.
    /// </summary>
    /// <returns>A tuple containing two enumerables of available time spans for today from now.</returns>
    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForTodayFromNow()
    {
        var tasks = _daoQueryService.GetTodayTasks();
        var endTime = TimeSpan.FromHours(24); // 11:59 PM

        var startTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);

        // todo: refactor this using _increment 
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
