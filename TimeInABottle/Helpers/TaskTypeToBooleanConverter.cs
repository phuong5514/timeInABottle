using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Helpers;
public class TaskTypeToBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is null) return false;
        if (value is DerivedTask)
        {
            return false;
        }

        if (value is ITask) { 
            return true;
        }

        return false;
    }
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}

