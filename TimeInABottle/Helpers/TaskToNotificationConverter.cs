using Microsoft.UI.Xaml.Data;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Helpers;
/// <summary>
/// Converts an ITask object to a notification string for display purposes.
/// </summary>
public class TaskToNotificationConverter : IValueConverter
{
    /// <summary>
    /// Converts an ITask object to a notification string.
    /// </summary>
    /// <param name="value">The ITask object to convert.</param>
    /// <param name="targetType">The type of the target property, unused.</param>
    /// <param name="parameter">Optional parameter, unused.</param>
    /// <param name="language">The language of the conversion, unused.</param>
    /// <returns>A string representing the notification message.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value == null)
        {
            return "Thats all for today!";
        }

        var TaskValue = (ITask)value;
        var result = $"Up next: {TaskValue.Name}";
        return result;
    }

    /// <summary>
    /// Converts a value back to an ITask object. Not implemented.
    /// </summary>
    /// <param name="value">The value to convert back.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">Optional parameter.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>Throws a NotImplementedException.</returns>
    /// <exception cref="NotImplementedException">Always thrown as this method is not implemented.</exception>
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}


