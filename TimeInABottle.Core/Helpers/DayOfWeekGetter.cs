
namespace TimeInABottle.Core.Helpers;
internal class DayOfWeekGetter
{
    /// <summary>
    /// Gets the day of the week for a specified day in the current month.
    /// </summary>
    /// <param name="day">The day of the month.</param>
    /// <returns>The <see cref="DayOfWeek"/> for the specified day in the current month.</returns>
    public static DayOfWeek GetDayOfWeekThisMonth(int day)
    {
        DateTime dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day);
        return dateTime.DayOfWeek;
    }
}
