using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Helpers;
/// <summary>
/// Converts a <see cref="MonthlyTask"/> object to a <see cref="Visibility"/> value.
/// *Temporary solution
/// should be deprecated and replaced with a more general converter if possible.
/// </summary>
internal class MonthlyTaskToVisibilityConverter : IValueConverter
{
    /// <summary>
    /// Converts a <see cref="MonthlyTask"/> object to a <see cref="Visibility"/> value.
    /// </summary>
    /// <param name="value">The value produced by the binding source.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>A <see cref="Visibility"/> value based on the input value.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return value is MonthlyTask ? Visibility.Visible : Visibility.Collapsed;
    }

    /// <summary>
    /// Converts a <see cref="Visibility"/> value back to a <see cref="MonthlyTask"/> object.
    /// This method is not implemented and will throw a <see cref="NotImplementedException"/>.
    /// </summary>
    /// <param name="value">The value that is produced by the binding target.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>Throws a <see cref="NotImplementedException"/>.</returns>
    /// <exception cref="NotImplementedException">Always thrown as this method is not implemented.</exception>
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
