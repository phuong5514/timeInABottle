using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;

using TimeInABottle.Core.Models;

namespace TimeInABottle.Helpers;
public partial class TimeToGreetingConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language) {
        var TimeValue = (Time)value;
        var hours = TimeValue.Hours;
        string? result;
        if (hours < 4)
        {
            result = "Good evenning!";
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
            result = "Good evenning!";
        }

        return result;
    }

    // not meant to be implemented - Phuong
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
