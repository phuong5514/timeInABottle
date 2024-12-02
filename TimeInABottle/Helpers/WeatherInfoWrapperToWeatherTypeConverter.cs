using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;
using TimeInABottle.Core.Models.Weather;

namespace TimeInABottle.Helpers;
public class WeatherInfoWrapperToWeatherTypeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is WeatherInfoWrapper wrapper)
        {
            return wrapper.WeatherType;
        }
        return null;
    }
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
