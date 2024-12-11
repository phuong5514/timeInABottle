using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Helpers;
internal class WeeklyTaskToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return value is WeeklyTask ? Visibility.Visible : Visibility.Collapsed;
    }
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
