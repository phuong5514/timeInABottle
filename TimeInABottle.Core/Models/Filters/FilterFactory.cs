using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models.Filters;
/// <summary>
/// Factory class for creating and registering filters.
/// </summary>
public class FilterFactory
{
    private static readonly Dictionary<string, Type> _filters = new();

    /// <summary>
    /// Registers a filter with the specified name and type.
    /// </summary>
    /// <param name="filterName">The name of the filter.</param>
    /// <param name="filterType">The type of the filter.</param>
    public static void RegisterFilter(string filterName, Type filterType)
    {
        if (_filters.ContainsKey(filterName) == false) { 
            _filters.Add(filterName, filterType);
        }
    }

    /// <summary>
    /// Creates an instance of the filter with the specified name.
    /// </summary>
    /// <param name="filterName">The name of the filter.</param>
    /// <returns>An instance of the filter.</returns>
    public static IFilter CreateFilter(string filterName)
    {
        return (IFilter)Activator.CreateInstance(_filters[filterName]);
    }
}
