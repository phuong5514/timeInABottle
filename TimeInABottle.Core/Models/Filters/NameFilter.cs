using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Core.Models.Filters;
/// <summary>
/// Represents a filter that matches tasks based on their name.
/// </summary>
public class NameFilter : IValueFilter
{
    /// <summary>
    /// Gets or sets the criteria to match the task's name against.
    /// </summary>
    public string Criteria
    {
        get;
        set;
    }

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Checks if the task's name matches the criteria.
    /// </summary>
    /// <param name="task">The task to check.</param>
    /// <returns>True if the task's name contains the criteria; otherwise, false.</returns>
    public bool MatchesCriteria(ITask task)
    {
        if (task == null || string.IsNullOrWhiteSpace(Criteria))
        {
            return false;
        }

        // Check if Criteria is in the task's name
        return task.Name.Contains(Criteria, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Gets the name of the filter.
    /// </summary>
    /// <returns>The name of the filter.</returns>
    public string Name() => "Name";

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => $"Name only: {Criteria}";
}
