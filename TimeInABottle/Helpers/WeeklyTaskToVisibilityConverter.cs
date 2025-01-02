using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Helpers;
/// <summary>
/// Converts a WeeklyTask object to a Visibility value.
/// </summary>
internal class WeeklyTaskToVisibilityConverter : IValueConverter
{
    /// <summary>
    /// Converts a WeeklyTask object to a Visibility value.
    /// </summary>
    /// <param name="value">The value produced by the binding source.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>A Visibility value based on the input value.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return value is WeeklyTask ? Visibility.Visible : Visibility.Collapsed;
    }

    /// <summary>
    /// Converts a value back. This method is not implemented and will throw an exception if called.
    /// </summary>
    /// <param name="value">The value that is produced by the binding target.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>Throws a NotImplementedException.</returns>
    /// <exception cref="NotImplementedException">Always thrown as this method is not implemented.</exception>
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
