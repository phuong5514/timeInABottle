using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Core.Helpers;
/// <summary>
/// Visitor class to get specific properties from different types of tasks.
/// </summary>
public class GetTaskSpecialtiesVisitor
{
    /// <summary>
    /// Visits a task and returns its specific property.
    /// </summary>
    /// <param name="task">The task to visit.</param>
    /// <returns>The specific property of the task.</returns>
    public object VisitTask(ITask task)
    {
        return task.Accept(this);
    }

    /// <summary>
    /// Visits a weekly task and returns the list of weekdays on which the task occurs.
    /// </summary>
    /// <param name="task">The weekly task to visit.</param>
    /// <returns>The list of weekdays on which the task occurs.</returns>
    public List<DayOfWeek> VisitWeeklyTask(WeeklyTask task) => task.WeekDays;

    /// <summary>
    /// Visits a monthly task and returns the date of the month on which the task occurs.
    /// </summary>
    /// <param name="task">The monthly task to visit.</param>
    /// <returns>The date of the month on which the task occurs.</returns>
    public int VisitMonthlyTask(MonthlyTask task) => task.Date;

    /// <summary>
    /// Visits a non-repeated task and returns the date on which the task occurs.
    /// </summary>
    /// <param name="task">The non-repeated task to visit.</param>
    /// <returns>The date on which the task occurs.</returns>
    public DateOnly VisitNonRepeatedTask(NonRepeatedTask task) => task.Date;
}
