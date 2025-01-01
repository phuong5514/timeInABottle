using Microsoft.UI.Xaml.Data;

namespace TimeInABottle.Helpers;
/// <summary>
/// Converts a task object to its name representation.
/// </summary>
internal class TaskToNameConverter : IValueConverter
{
    /// <summary>
    /// Converts a task object to its name representation.
    /// </summary>
    /// <param name="value">The task object to convert.</param>
    /// <param name="targetType">The type of the target property.</param>
    /// <param name="parameter">Optional parameter to be used in the converter logic.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>The name of the task or an empty string if the task is null.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var task = (Core.Models.Tasks.ITask)value;
        if (task == null)
        {
            return "";
        }
        return task.TypeName();
    }

    /// <summary>
    /// Converts a value back to a task object. Not implemented.
    /// </summary>
    /// <param name="value">The value to convert back.</param>
    /// <param name="targetType">The type of the target property.</param>
    /// <param name="parameter">Optional parameter to be used in the converter logic.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>Throws a NotImplementedException.</returns>
    /// <exception cref="NotImplementedException">Always thrown as this method is not implemented.</exception>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
