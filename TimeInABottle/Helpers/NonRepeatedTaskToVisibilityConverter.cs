using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Helpers;
/// <summary>
/// Converts a NonRepeatedTask object to a Visibility value.
/// *Temporary solution
/// </summary>
internal class NonRepeatedTaskToVisibilityConverter : IValueConverter
{
    /// <summary>
    /// Converts a NonRepeatedTask object to a Visibility value.
    /// </summary>
    /// <param name="value">The value produced by the binding source.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>Visibility.Visible if the value is a NonRepeatedTask; otherwise, Visibility.Collapsed.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return value is NonRepeatedTask ? Visibility.Visible : Visibility.Collapsed;
    }

    /// <summary>
    /// Converts a value back. This method is not implemented and will throw a NotImplementedException.
    /// </summary>
    /// <param name="value">The value that is produced by the binding target.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>Throws a NotImplementedException.</returns>
    /// <exception cref="NotImplementedException">Always thrown.</exception>
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
