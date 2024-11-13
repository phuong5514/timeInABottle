using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;
using TimeInABottle.Core.Models;

namespace TimeInABottle.Helpers;
/// <summary>
/// Converts a TimeOnly object to a string representation and vice versa.
/// </summary>
public class TimeToStringConverter : IValueConverter
{
    /// <summary>
    /// Converts a TimeOnly object to a string representation in the format "HH:mm".
    /// </summary>
    /// <param name="value">The TimeOnly object to convert.</param>
    /// <param name="targetType">The type of the target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>A string representation of the TimeOnly object in the format "HH:mm".</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var timeValue = (TimeOnly)value;
        if (timeValue == null)
        {
            return new ArgumentNullException();
        }

        var hours = timeValue.Hour;
        var minutes = timeValue.Minute;
        var result = string.Format("{0:00}:{1:00}", hours, minutes);
        return result;
    }

    /// <summary>
    /// Converts a string representation of time in the format "HH:mm" back to a TimeOnly object.
    /// </summary>
    /// <param name="value">The string representation of time to convert.</param>
    /// <param name="targetType">The type of the target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>A TimeOnly object representing the time.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        var stringValue = (string)value;
        if (string.IsNullOrEmpty(stringValue))
        {
            return new ArgumentException("value cannot be converted back to Time");
        }

        var separator = ':';
        var list = stringValue.Split(separator);
        if (list.Length == 2)
        {
            var hours = Int32.Parse(list[0]);
            var minutes = Int32.Parse(list[1]);
            return new TimeOnly(hours, minutes);
        }

        return new Exception();
    }
}
