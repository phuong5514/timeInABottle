using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models.Filters;
public class CompositeFilter : IFilter
{
    private readonly Dictionary<Type, List<IFilter>> _filtersByType = new();

    public void AddFilter(IFilter filter)
    {
        var filterType = filter.GetType();
        if (!_filtersByType.ContainsKey(filterType))
        {
            _filtersByType[filterType] = new List<IFilter>();
        }
        _filtersByType[filterType].Add(filter);
    }

    public bool MatchesCriteria(ITask task)
    {
        // Apply each filter type's collection as a union (OR), then intersect (AND) across types.
        return _filtersByType.Values.All(filterList =>
            filterList.Any(filter => filter.MatchesCriteria(task)) // this is really cool, I dont even know this exist
        );
    }

    public string Name() => "Composite Filter";

    public override string ToString()
    {
        return string.Join(" AND ", _filtersByType.Select(group =>
            "(" + string.Join(" OR ", group.Value.Select(filter => filter.ToString())) + ")"
        ));
    }
}