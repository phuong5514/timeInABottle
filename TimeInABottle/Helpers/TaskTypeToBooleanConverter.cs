using Microsoft.UI.Xaml.Data;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Helpers;
/// <summary>
/// Converts a task type to a boolean value.
/// </summary>
/// <remarks>
/// This converter returns false for null values and instances of DerivedTask.
/// It returns true for instances of ITask.
/// </remarks>
public class TaskTypeToBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is null) return false;
        if (value is DerivedTask)
        {
            return false;
        }

        if (value is ITask)
        {
            return true;
        }

        return false;
    }
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}

