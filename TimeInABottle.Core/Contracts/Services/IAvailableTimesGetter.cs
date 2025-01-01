using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Core.Contracts.Services;
public interface IAvailableTimesGetter
{
    /// <summary>
    /// Gets the available times based on the provided task list.
    /// </summary>
    /// <param name="taskList">The list of tasks to consider.</param>
    /// <returns>A tuple containing two enumerables of available time spans.</returns>
    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimes(IEnumerable<ITask> taskList);

    /// <summary>
    /// Gets the available times for today.
    /// </summary>
    /// <returns>A tuple containing two enumerables of available time spans for today.</returns>
    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForToday();

    /// <summary>
    /// Gets the available times for a specific date.
    /// </summary>
    /// <param name="date">The date to consider.</param>
    /// <returns>A tuple containing two enumerables of available time spans for the specified date.</returns>
    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForDate(DateOnly date);

    /// <summary>
    /// Gets the available times for a specific date represented as an integer.
    /// </summary>
    /// <param name="date">The date to consider as an integer.</param>
    /// <returns>A tuple containing two enumerables of available time spans for the specified date.</returns>
    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForDate(int date);

    /// <summary>
    /// Gets the available times for a specific week based on the provided weekdays.
    /// </summary>
    /// <param name="inputWeekDays">The weekdays to consider.</param>
    /// <returns>A tuple containing two enumerables of available time spans for the specified week.</returns>
    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForWeek(IEnumerable<DayOfWeek> inputWeekDays);

    /// <summary>
    /// Gets the available times for the current week.
    /// </summary>
    /// <returns>A tuple containing two enumerables of available time spans for the current week.</returns>
    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForWeek();

    /// <summary>
    /// Gets the available times for the next week.
    /// </summary>
    /// <returns>A tuple containing two enumerables of available time spans for the next week.</returns>
    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForNextWeek();

    /// <summary>
    /// Gets the available times for a week starting from now.
    /// </summary>
    /// <returns>A tuple containing two enumerables of available time spans for a week from now.</returns>
    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForWeekFromNow();

    /// <summary>
    /// Gets the available times for today starting from now.
    /// </summary>
    /// <returns>A tuple containing two enumerables of available time spans for today from now.</returns>
    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForTodayFromNow();
}
