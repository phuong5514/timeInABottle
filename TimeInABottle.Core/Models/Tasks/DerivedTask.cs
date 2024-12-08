using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models.Tasks;
/// <summary>
/// Represents a derived task with specific properties and methods.
/// </summary>
public class DerivedTask : ITask
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DerivedTask"/> class.
    /// </summary>
    /// <param name="name">The name of the task.</param>
    /// <param name="description">The description of the task.</param>
    /// <param name="start">The start time of the task.</param>
    /// <param name="end">The end time of the task.</param>
    public DerivedTask(string name, string description, TimeOnly start, TimeOnly end) : base(name, description, start, end)
    {
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => throw new NotImplementedException();
}
