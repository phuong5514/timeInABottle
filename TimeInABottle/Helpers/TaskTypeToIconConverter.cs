using Microsoft.UI.Xaml.Data;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Helpers;

/// <summary>
/// Converts task types to their corresponding icon representations.
/// </summary>
class TaskTypeToIconConverter : IValueConverter
{
    /// <summary>
    /// Converts a task type to its corresponding icon.
    /// </summary>
    /// <param name="value">The task type to convert.</param>
    /// <param name="targetType">The target type (not used).</param>
    /// <param name="parameter">Additional parameter (not used).</param>
    /// <param name="language">The language (not used).</param>
    /// <returns>A string representing the icon for the task type.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return value switch
        {
            DailyTask => "\uE8BF",  // Calendar Day
            WeeklyTask => "\uE8C0", // Calendar Week
            MonthlyTask => "\uE787", // Calendar Month
            NonRepeatedTask => "\uE9D5", // Check list
            DerivedTask => "\uE71B", // Link
            _ => "\uE9CE", // Default Task Icon
        };
    }

    /// <summary>
    /// Converts back the icon to its corresponding task type.
    /// This method is not implemented.
    /// </summary>
    /// <param name="value">The value to convert back.</param>
    /// <param name="targetType">The target type.</param>
    /// <param name="parameter">Additional parameter.</param>
    /// <param name="language">The language.</param>
    /// <returns>Throws a <see cref="NotImplementedException"/>.</returns>
    /// <exception cref="NotImplementedException">Always thrown as this method is not implemented.</exception>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
        => throw new NotImplementedException();
}
