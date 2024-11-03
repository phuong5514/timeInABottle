using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Helpers;
using System;
using System.Globalization;
using Microsoft.UI.Xaml.Data;

public class TimeRangeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is (TimeOnly start, TimeOnly end))
        {
            return $"{start} - {end}";
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
