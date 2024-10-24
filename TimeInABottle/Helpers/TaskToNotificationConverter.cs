using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;
using TimeInABottle.Core.Models;

namespace TimeInABottle.Helpers;
public class TaskToNotificationConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language) {
        if (value == null) {
            return "Thats all for today!";
        }

        var TaskValue = (ITask)value;
        var result = $"Up next: {TaskValue.Name}";
        return result;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}


