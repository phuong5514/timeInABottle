using System.ComponentModel;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Core.Models.Filters;

/// <summary>
/// Represents a filter that matches tasks based on their description.
/// </summary>
public class DescriptionFilter : IValueFilter
{
    /// <summary>
    /// Gets or sets the criteria for the filter.
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
    /// Checks if the task matches the filter criteria.
    /// </summary>
    /// <param name="task">The task to check.</param>
    /// <returns>True if the task's description contains the criteria; otherwise, false.</returns>
    public bool MatchesCriteria(ITask task)
    {
        if (task == null || string.IsNullOrWhiteSpace(Criteria))
        {
            return false;
        }

        // Check if Criteria is in the task's description
        return task.Description.Contains(Criteria, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Gets the name of the filter.
    /// </summary>
    /// <returns>The name of the filter.</returns>
    public string Name() => "Description";

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => $"Description: {Criteria}";
}

