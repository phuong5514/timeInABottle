using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;
using TimeInABottle.Core.Models.Filters;

namespace TimeInABottle.Helpers;
/// <summary>
/// Converts an IFilter object to its name representation.
/// </summary>
internal class FilterToNameConverter : IValueConverter
{
    /// <summary>
    /// Converts an IFilter object to its name.
    /// </summary>
    /// <param name="value">The IFilter object to convert.</param>
    /// <param name="targetType">The type of the target property.</param>
    /// <param name="parameter">Optional parameter to be used in the converter logic.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>The name of the IFilter object, or an empty string if the value is not an IFilter.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is IFilter filter)
        {
            return filter.Name();
        }
        return string.Empty;
    }

    /// <summary>
    /// Not implemented. Throws a NotImplementedException if called.
    /// </summary>
    /// <param name="value">The value produced by the binding target.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">Optional parameter to be used in the converter logic.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>Throws a NotImplementedException.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
