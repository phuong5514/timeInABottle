using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace TimeInABottle.Helpers;
public class DateTimeToDateTimeOffsetConverter : IValueConverter
{
    /// <summary>
    /// Converts a DateTime object to a DateTimeOffset object.
    /// </summary>
    /// <param name="value">The DateTime object to convert.</param>
    /// <param name="targetType">The type of the target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>A DateTimeOffset object if the input value is a DateTime; otherwise, DependencyProperty.UnsetValue.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is DateTime dateTime)
        {
            return new DateTimeOffset(dateTime);
        }
        return DependencyProperty.UnsetValue;
    }

    /// <summary>
    /// Converts a DateTimeOffset object back to a DateTime object.
    /// </summary>
    /// <param name="value">The DateTimeOffset object to convert back.</param>
    /// <param name="targetType">The type of the target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>A DateTime object if the input value is a DateTimeOffset; otherwise, the current date and time.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.DateTime;
        }
        return DateTime.Now;
    }
}
