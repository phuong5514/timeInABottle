using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Helpers;

class TaskTypeToIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return value switch
        {
            DailyTask => "\uE8BF",  // Calendar Day
            WeeklyTask => "\uE8C0", // Calendar Week
            MonthlyTask => "\uE787", // Calendar Month
            NonRepeatedTask => "\uE9D5", // Check list
            DerivedTask => "\uE71B", // Link
            _ => "\uE9CE", // Default Task Icon
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
        => throw new NotImplementedException();
}