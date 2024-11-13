using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models.Filters;

/// <summary>
/// Represents a filter for monthly tasks.
/// Implements the <see cref="ITypeFilter"/> interface.
/// </summary>
public class MonthlyTaskFilter : ITypeFilter
{
    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Determines whether the specified task matches the criteria of a monthly task.
    /// </summary>
    /// <param name="task">The task to check.</param>
    /// <returns><c>true</c> if the task is a monthly task; otherwise, <c>false</c>.</returns>
    public bool MatchesCriteria(ITask task) => task is MonthlyTask;

    /// <summary>
    /// Gets the name of the filter.
    /// </summary>
    /// <returns>The name of the filter.</returns>
    public string Name() => "Monthly";

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => $"Type: Monthly";
}

