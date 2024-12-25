using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Helpers;
/// <summary>
/// 
/// Converts task objects to their corresponding specialty titles.
/// </summary>
public class TaskToSpecialtyStringConverter : IValueConverter
{
    /// <summary>
    /// Converts a task object to a specialty title string.
    /// </summary>
    /// <param name="value">The task object to convert.</param>
    /// <param name="targetType">The type of the target property.</param>
    /// <param name="parameter">Optional parameter to use in the converter logic.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>A string representing the specialty title of the task.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is ITask task)
        {
            // Determine task type and format title accordingly.
            switch (task)
            {
                case DailyTask:
                    return "Every day"; // Daily tasks might not need a specific specialty title.
                case WeeklyTask weeklyTask:
                    var stringBuilder = new StringBuilder("Weekly on ");
                    foreach(var day in weeklyTask.WeekDays)
                    {
                        stringBuilder.Append(day.ToString());
                        stringBuilder.Append(", ");
                    }

                    return stringBuilder.ToString().TrimEnd(',', ' ');
                case MonthlyTask monthlyTask:
                    // Assuming MonthlyTask has a property `DayInMonth` for the day of the month.
                    return $"Monthly on day {monthlyTask.Date}";
                case NonRepeatedTask nonRepeatingTask:
                    // Assuming NonRepeatingTask has a specific date property.
                    return nonRepeatingTask.Date.ToString("MMMM dd, yyyy");
                case DerivedTask derivedTask:
                    return derivedTask.AssignedDate.ToString("MMMM dd, yyyy");
                default:
                    return "Unknown Task Type";
            }
        }
        return "";
    }

    /// <summary>
    /// Converts a specialty title string back to a task object.
    /// </summary>
    /// <param name="value">The specialty title string to convert back.</param>
    /// <param name="targetType">The type of the target property.</param>
    /// <param name="parameter">Optional parameter to use in the converter logic.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>The task object corresponding to the specialty title string.</returns>
    /// <exception cref="NotImplementedException">Thrown when the method is not implemented.</exception>
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();

    /// <summary>
    /// Formats a collection of days of the week into a comma-separated string.
    /// </summary>
    /// <param name="daysOfWeek">The collection of days of the week to format.</param>
    /// <returns>A comma-separated string representing the days of the week.</returns>
    private string FormatDaysOfWeek(IEnumerable<DayOfWeek> daysOfWeek)
    {
        // Convert each day of the week to a string, then join them.
        return string.Join(", ", daysOfWeek.Select(day => day.ToString()));
    }
}

