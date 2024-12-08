using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace TimeInABottle.Helpers;

/// <summary>
/// Converts an enum value to a boolean and vice versa.
/// </summary>
public class EnumToBooleanConverter : IValueConverter
{
    /// <summary>
    /// Converts an enum value to a boolean.
    /// </summary>
    /// <param name="value">The enum value to convert.</param>
    /// <param name="targetType">The target type (not used).</param>
    /// <param name="parameter">The enum name as a string.</param>
    /// <param name="language">The language (not used).</param>
    /// <returns>True if the enum value matches the parameter; otherwise, false.</returns>
    /// <exception cref="ArgumentException">Thrown when the value is not a valid enum or the parameter is not a string.</exception>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (parameter is string enumString)
        {
            if (!Enum.IsDefined(typeof(ElementTheme), value))
            {
                throw new ArgumentException("ExceptionEnumToBooleanConverterValueMustBeAnEnum");
            }

            var enumValue = Enum.Parse(typeof(ElementTheme), enumString);

            return enumValue.Equals(value);
        }

        throw new ArgumentException("ExceptionEnumToBooleanConverterParameterMustBeAnEnumName");
    }

    /// <summary>
    /// Converts a boolean back to an enum value.
    /// </summary>
    /// <param name="value">The boolean value to convert (not used).</param>
    /// <param name="targetType">The target type (not used).</param>
    /// <param name="parameter">The enum name as a string.</param>
    /// <param name="language">The language (not used).</param>
    /// <returns>The enum value corresponding to the parameter.</returns>
    /// <exception cref="ArgumentException">Thrown when the parameter is not a string.</exception>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (parameter is string enumString)
        {
            return Enum.Parse(typeof(ElementTheme), enumString);
        }

        throw new ArgumentException("ExceptionEnumToBooleanConverterParameterMustBeAnEnumName");
    }
}
