using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models.Filters;
/// <summary>
/// Represents a filter that matches tasks based on a keyword criteria.
/// </summary>
public class KeywordFilter : IValueFilter
{
    /// <summary>
    /// Gets or sets the keyword criteria for the filter.
    /// </summary>
    public string Criteria { get; set; }

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Checks if the given task matches the keyword criteria.
    /// </summary>
    /// <param name="task">The task to check against the criteria.</param>
    /// <returns>True if the task matches the criteria; otherwise, false.</returns>
    public bool MatchesCriteria(ITask task)
    {
        if (task == null || string.IsNullOrWhiteSpace(Criteria))
        {
            return false;
        }

        // Check if Criteria is in the task's name, description, or formatted time
        return task.Name.Contains(Criteria, StringComparison.OrdinalIgnoreCase) ||
               task.Description.Contains(Criteria, StringComparison.OrdinalIgnoreCase) ||
               task.FormattedTime.Contains(Criteria, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Gets the name of the filter.
    /// </summary>
    /// <returns>The name of the filter.</returns>
    public string Name() => "Keyword";

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => $"Keyword: {Criteria}";
}
