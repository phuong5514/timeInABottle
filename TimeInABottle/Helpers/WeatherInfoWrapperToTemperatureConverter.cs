using Microsoft.UI.Xaml.Data;
using TimeInABottle.Core.Models.Weather;

namespace TimeInABottle.Helpers;
/// <summary>
/// Converts a <see cref="WeatherInfoWrapper"/> object to a temperature string representation.
/// </summary>
public class WeatherInfoWrapperToTemperatureConverter : IValueConverter
{
    /// <summary>
    /// Converts a <see cref="WeatherInfoWrapper"/> object to a temperature string representation.
    /// </summary>
    /// <param name="value">The value produced by the binding source.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>A string representation of the temperature in Celsius, or an empty string if the value is not a <see cref="WeatherInfoWrapper"/>.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is WeatherInfoWrapper wrapper)
        {
            return $"{wrapper.Temperature}°C";
        }
        return string.Empty;
    }

    /// <summary>
    /// Converts a value back. This method is not implemented and will throw a <see cref="NotImplementedException"/>.
    /// </summary>
    /// <param name="value">The value that is produced by the binding target.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>Throws a <see cref="NotImplementedException"/>.</returns>
    /// <exception cref="NotImplementedException">Always thrown as this method is not implemented.</exception>
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
