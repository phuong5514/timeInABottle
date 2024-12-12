using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;

namespace TimeInABottle.Helpers;
public partial class ListContainsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language) { 
        if (value is IEnumerable list && parameter is string content)
        {
            foreach (var item in list)
            {
                if (item?.ToString() == content)
                {
                    return true;
                }
            }
        }
        return false;

    }
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        // `value` is the state of the CheckBox (true or false)
        // `parameter` is the CheckBox Content
        if (parameter is string content && targetType == typeof(IEnumerable))
        {
            var list = value as IList;

            if (list != null)
            {
                if ((bool)value)
                {
                    if (!list.Contains(content))
                    {
                        list.Add(content);
                    }
                }
                else
                {
                    list.Remove(content);
                }
            }
            return list;
        }
        return null;
    }
}


    