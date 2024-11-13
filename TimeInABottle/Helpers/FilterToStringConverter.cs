using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;
using TimeInABottle.Core.Models.Filters;

namespace TimeInABottle.Helpers;
/// <summary>
/// Converts an IFilter object to its string representation.
/// </summary>
internal class FilterToStringConverter : IValueConverter
{
    /// <summary>
    /// Converts an IFilter object to a string.
    /// </summary>
    /// <param name="value">The IFilter object to convert.</param>
    /// <param name="targetType">The type of the target property.</param>
    /// <param name="parameter">Optional parameter to be used in the converter logic.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>A string representation of the IFilter object, or an empty string if the value is not an IFilter.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is IFilter filter)
        {
            return filter.ToString();
        }
        return string.Empty;
    }

    /// <summary>
    /// Converts a string back to an IFilter object. Not implemented.
    /// </summary>
    /// <param name="value">The string to convert back.</param>
    /// <param name="targetType">The type of the target property.</param>
    /// <param name="parameter">Optional parameter to be used in the converter logic.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>Throws a NotImplementedException.</returns>
    /// <exception cref="NotImplementedException">Always thrown as this method is not implemented.</exception>
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
