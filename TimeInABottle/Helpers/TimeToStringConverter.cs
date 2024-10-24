using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;
using TimeInABottle.Core.Models;

namespace TimeInABottle.Helpers;
public class TimeToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language) {
        var timeValue = (TimeOnly)value;
        if (timeValue == null) {
            return new ArgumentNullException();
        }


        var hours = timeValue.Hour;
        var minutes = timeValue.Minute;
        var result = string.Format("{0:00}:{1:00}", hours, minutes);
        return result;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) {
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
