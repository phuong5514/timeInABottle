using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models.Filters;
/// <summary>
/// Represents a filter for daily tasks.
/// </summary>
public class DailyTaskFilter : ITypeFilter
{
    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Determines whether the specified task matches the criteria of a daily task.
    /// </summary>
    /// <param name="task">The task to check.</param>
    /// <returns>true if the task is a daily task; otherwise, false.</returns>
    public bool MatchesCriteria(ITask task) => task is DailyTask;

    /// <summary>
    /// Gets the name of the filter.
    /// </summary>
    /// <returns>The name of the filter.</returns>
    public string Name() => "Daily";

    /// <summary>
    /// Returns a string that represents the current filter.
    /// </summary>
    /// <returns>A string that represents the current filter.</returns>
    public override string ToString() => $"Type: Daily";
}
