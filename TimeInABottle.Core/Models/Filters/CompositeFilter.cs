using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models.Filters;
public class CompositeFilter : IFilter
{
    private readonly Dictionary<Type, List<IFilter>> _filtersByType = new();

    public event PropertyChangedEventHandler PropertyChanged;

    public bool AddFilter(IFilter filter)
    {
        var filterType = filter.GetType();
        if (filter is IValueFilter value)
        {
            if (!_filtersByType.ContainsKey(filterType))
            {
                _filtersByType[filter.GetType()] = new List<IFilter>();
            }
            var list = _filtersByType[filter.GetType()];
            foreach (var item in list.Cast<IValueFilter>()) {
                if (item.Criteria == value.Criteria) {
                    return false;
                }
            }

            list.Add(filter);
            return true;
        }
        else if (filter is ITypeFilter)
        {
            if (!_filtersByType.ContainsKey(filterType))
            {
                _filtersByType[filter.GetType()] = new List<IFilter> { filter };
                return true;
            }
        }
        else
        {
            throw new ArgumentException("Unrecognized filter type!");
        }

        return false;

    }

    public void RemoveFilter(IFilter filter)
    {
        var filterType = filter.GetType();
        if (filter is ITypeFilter)
        {
            _filtersByType.Remove(filter.GetType());
        }
        else if (filter is IValueFilter valueFilter)
        {
            if (_filtersByType.TryGetValue(filter.GetType(), out var filterList))
            {
                filterList.RemoveAll(existingFilter =>
                    existingFilter is IValueFilter existingValueFilter &&
                    existingValueFilter.Criteria == valueFilter.Criteria);

                // empty filter list can mess with MatchesCriteria's result
                if (filterList.Count <= 0) {
                    _filtersByType.Remove(filter.GetType());
                }
            }
        }

    }

    public bool MatchesCriteria(ITask task)
    {
        // Apply each filter type's collection as a union (OR), then intersect (AND) across types.
        return _filtersByType.Values.All(filterList =>
            filterList.Any(filter => filter.MatchesCriteria(task)) // this is really cool
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