using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models.Filters;
public class FilterFactory
{
    private static readonly Dictionary<string, Type> _filters = new();

    public static void RegisterFilter(string filterName, Type filterType)
    {
        _filters.Add(filterName, filterType);
    }
        
    public static IFilter CreateFilter(string filterName)
    {
        return (IFilter)Activator.CreateInstance(_filters[filterName]);
    }
}
