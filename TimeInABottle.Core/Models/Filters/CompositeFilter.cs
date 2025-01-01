using System.ComponentModel;
using TimeInABottle.Core.Models.Tasks;


namespace TimeInABottle.Core.Models.Filters;


/// <summary>
/// CompositeFilter class is used to manage a collection of filters, allowing for complex filtering logic.
/// It supports adding and removing filters, and checking if a task matches the criteria defined by the filters.
/// </summary>
public class CompositeFilter : IFilter
{
    private readonly Dictionary<Type, List<IFilter>> _filtersByType = [];

    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Adds a filter to the collection. If the filter is already present, it will not be added again.
    /// </summary>
    /// <param name="filter">The filter to add.</param>
    /// <returns>True if the filter was added, false if it was already present.</returns>
    public bool AddFilter(IFilter filter)
    {
        var filterType = filter.GetType();
        if (filter is IValueFilter value)
        {
            if (!_filtersByType.ContainsKey(filterType))
            {
                _filtersByType[filter.GetType()] = [];
            }
            var list = _filtersByType[filter.GetType()];
            foreach (var item in list.Cast<IValueFilter>())
            {
                if (item.Criteria == value.Criteria)
                {
                    return false;
                }
            }

            list.Add(filter);

            return true;

        }
        else if (filter is ITypeFilter)
        {
            if (!_filtersByType.ContainsKey(typeof(ITypeFilter)))
            {
                _filtersByType[typeof(ITypeFilter)] = [];
            }
            var list = _filtersByType[typeof(ITypeFilter)];

            foreach (var listitem in list) {
                if (listitem.GetType() == filterType) {
                    return false;
                }
            }

            list.Add(filter);
            return true;
        }
        else
        {
            throw new ArgumentException("Unrecognized filter type!");
        }
    }



    /// <summary>
    /// Removes a filter from the collection.
    /// </summary>
    /// <param name="filter">The filter to remove.</param>
    public void RemoveFilter(IFilter filter)
    {
        var filterType = filter.GetType();
        if (filter is ITypeFilter)
        {
            var list = _filtersByType[typeof(ITypeFilter)];
            list.RemoveAll(existingFilter => existingFilter.GetType() == filter.GetType());

            if (list.Count <= 0)
            {
                _filtersByType.Remove(typeof(ITypeFilter));
            }
        }
        else if (filter is IValueFilter valueFilter)
        {
            if (_filtersByType.TryGetValue(filter.GetType(), out var filterList))
            {
                filterList.RemoveAll(existingFilter =>
                    existingFilter is IValueFilter existingValueFilter &&
                    existingValueFilter.Criteria == valueFilter.Criteria);

                // empty filter list can mess with MatchesCriteria's result
                if (filterList.Count <= 0)
                {
                    _filtersByType.Remove(filter.GetType());
                }
            }
        }

    }

    /// <summary>
    /// Checks if a task matches the criteria defined by the filters.
    /// </summary>
    /// <param name="task">The task to check.</param>
    /// <returns>True if the task matches the criteria, false otherwise.</returns>
    public bool MatchesCriteria(ITask task)
    {
        // Apply each filter type's collection as a union (OR), then intersect (AND) across types.
        return _filtersByType.Values.All(filterList =>
            filterList.Any(filter => filter.MatchesCriteria(task)) // this is really cool
        );
    }

    /// <summary>
    /// Gets the name of the filter.
    /// </summary>
    /// <returns>The name of the filter.</returns>
    public string Name() => "Composite Filter";

    /// <summary>
    /// Returns a string representation of the filter.
    /// </summary>
    /// <returns>A string representation of the filter.</returns>
    public override string ToString()
    {
        return string.Join(" AND ", _filtersByType.Select(group =>
            "(" + string.Join(" OR ", group.Value.Select(filter => filter.ToString())) + ")"
        ));
    }
}
