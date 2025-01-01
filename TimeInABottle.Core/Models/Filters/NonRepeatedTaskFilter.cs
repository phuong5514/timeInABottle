using System.ComponentModel;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Core.Models.Filters;
/// <summary>
/// Represents a filter that matches tasks of type <see cref="NonRepeatedTask"/>.
/// Implements the <see cref="ITypeFilter"/> interface.
/// </summary>
public class NonRepeatedTaskFilter : ITypeFilter
{
    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Determines whether the specified task matches the criteria of being a <see cref="NonRepeatedTask"/>.
    /// </summary>
    /// <param name="task">The task to check.</param>
    /// <returns><c>true</c> if the task is a <see cref="NonRepeatedTask"/>; otherwise, <c>false</c>.</returns>
    public bool MatchesCriteria(ITask task) => task is NonRepeatedTask;

    /// <summary>
    /// Gets the name of the filter.
    /// </summary>
    /// <returns>A string representing the name of the filter.</returns>
    public string Name() => "Non-Repeated";

    /// <summary>
    /// Returns a string that represents the current filter.
    /// </summary>
    /// <returns>A string that represents the current filter.</returns>
    public override string ToString() => $"Type: Non-Repeated";
}
