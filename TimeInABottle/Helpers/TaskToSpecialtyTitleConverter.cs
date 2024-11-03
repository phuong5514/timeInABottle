using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;
using TimeInABottle.Core.Models;

namespace TimeInABottle.Helpers;
public class TaskToSpecialtyTitleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language) {
        if (value is ITask task)
        {
            // Determine task type and format title accordingly.
            switch (task)
            {
                case DailyTask:
                    return ""; // Daily tasks might not need a specific specialty title.
                case WeeklyTask weeklyTask:
                    // Assuming WeeklyTask has a property `DaysOfWeek` (e.g., a list of days or flags for each day).
                    //return FormatDaysOfWeek(weeklyTask.WeekDays);
                    return "";
                case MonthlyTask monthlyTask:
                    // Assuming MonthlyTask has a property `DayInMonth` for the day of the month.
                    return $"Day {monthlyTask.Date}";
                case NonRepeatedTask nonRepeatingTask:
                    // Assuming NonRepeatingTask has a specific date property.
                    return nonRepeatingTask.Date.ToString("MMMM dd, yyyy");
                default:
                    return "Unknown Task Type";
            }
        }
        return "";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();

    private string FormatDaysOfWeek(IEnumerable<DayOfWeek> daysOfWeek)
    {
        // Convert each day of the week to a string, then join them.
        return string.Join(", ", daysOfWeek.Select(day => day.ToString()));
    }
}

