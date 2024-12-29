using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace TimeInABottle.Helpers;


public partial class ListToAvailabilityColorConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is IEnumerable list)
        {
            // Use LINQ's Count() method for non-generic IEnumerable
            var count = list.Cast<object>().Count();

            if (count != 0)
            {
                return Application.Current.Resources["SystemFillColorSuccessBackgroundBrush"] as SolidColorBrush;
            }
            else
            {
                return Application.Current.Resources["SystemFillColorCriticalBackgroundBrush"] as SolidColorBrush;
            }
        }

        // Default value in case the input is not a list
        return Application.Current.Resources["SystemFillColorBaseBrush"] as SolidColorBrush;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
        => throw new NotImplementedException();
}