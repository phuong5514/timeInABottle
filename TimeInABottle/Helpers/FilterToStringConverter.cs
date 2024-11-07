using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;
using TimeInABottle.Core.Models.Filters;

namespace TimeInABottle.Helpers;
internal class FilterToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language) {
        if (value is IFilter filter)
        {
            return filter.ToString();
        }
        return string.Empty;
    }
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
