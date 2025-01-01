using Microsoft.UI.Xaml.Data;
using TimeInABottle.Core.Models.Weather;

namespace TimeInABottle.Helpers;
/// <summary>
/// Converts a <see cref="WeatherInfoWrapper"/> object to its corresponding <see cref="WeatherType"/>.
/// </summary>
public class WeatherInfoWrapperToWeatherTypeConverter : IValueConverter
{
    /// <summary>
    /// Converts a <see cref="WeatherInfoWrapper"/> object to its corresponding <see cref="WeatherType"/>.
    /// </summary>
    /// <param name="value">The <see cref="WeatherInfoWrapper"/> object to convert.</param>
    /// <param name="targetType">The type of the binding target property. This parameter is not used.</param>
    /// <param name="parameter">The converter parameter to use. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>The <see cref="WeatherType"/> of the <see cref="WeatherInfoWrapper"/> object, or null if the input is not a <see cref="WeatherInfoWrapper"/>.</returns>
    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is WeatherInfoWrapper wrapper)
        {
            return wrapper.WeatherType;
        }
        return null;
    }

    /// <summary>
    /// This method is not implemented and will throw a <see cref="NotImplementedException"/> if called.
    /// </summary>
    /// <param name="value">The value that is to be converted back. This parameter is not used.</param>
    /// <param name="targetType">The type to convert to. This parameter is not used.</param>
    /// <param name="parameter">The converter parameter to use. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>None. This method always throws a <see cref="NotImplementedException"/>.</returns>
    /// <exception cref="NotImplementedException">Always thrown since the method is not implemented.</exception>
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
