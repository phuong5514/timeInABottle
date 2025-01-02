using Microsoft.UI.Xaml.Data;
namespace TimeInABottle.Helpers;

/// <summary>
/// Converts a TimeOnly value to a greeting string based on the time of day.
/// </summary>
public partial class TimeToGreetingConverter : IValueConverter
{
    /// <summary>
    /// Converts a TimeOnly value to a greeting string.
    /// </summary>
    /// <param name="value">The TimeOnly value to convert.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>A greeting string based on the time of day.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var TimeValue = (TimeOnly)value;
        var hours = TimeValue.Hour;
        string? result;
        if (hours < 4)
        {
            result = "Good evening!";
        }
        else if (hours < 12)
        {
            result = "Good morning!";
        }
        else if (hours < 18)
        {
            result = "Good afternoon!";
        }
        else
        {
            result = "Good evening!";
        }

        return result;
    }

    /// <summary>
    /// This method is not implemented and will throw a NotImplementedException if called.
    /// </summary>
    /// <param name="value">The value that is produced by the binding target.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>Throws a NotImplementedException.</returns>
    /// <exception cref="NotImplementedException">Always thrown.</exception>
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
