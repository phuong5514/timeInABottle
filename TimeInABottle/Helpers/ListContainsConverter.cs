using System.Collections;
using Microsoft.UI.Xaml.Data;

namespace TimeInABottle.Helpers;
public partial class ListContainsConverter : IValueConverter
{
    /// <summary>
    /// Checks if the list contains the specified content.
    /// </summary>
    /// <param name="value">The list to check.</param>
    /// <param name="targetType">The type of the target property.</param>
    /// <param name="parameter">The content to check for in the list.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>True if the list contains the content, otherwise false.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
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

    /// <summary>
    /// Updates the list based on the boolean value.
    /// </summary>
    /// <param name="value">The state of the CheckBox (true or false).</param>
    /// <param name="targetType">The type of the target property.</param>
    /// <param name="parameter">The CheckBox content.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>The updated list.</returns>
    public object? ConvertBack(object value, Type targetType, object parameter, string language)
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


    