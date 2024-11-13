using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models;
/// <summary>
/// Represents an abstract repeated task that inherits from the ITask class.
/// </summary>
public abstract class IRepeatedTask : ITask
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IRepeatedTask"/> class.
    /// </summary>
    /// <param name="name">The name of the repeated task.</param>
    /// <param name="description">The description of the repeated task.</param>
    /// <param name="startingTime">The starting time of the repeated task.</param>
    /// <param name="endingTime">The ending time of the repeated task.</param>
    protected IRepeatedTask(string name, string description, TimeOnly startingTime, TimeOnly endingTime)
        : base(name, description, startingTime, endingTime)
    {
    }

    /// <summary>
    /// Returns a string that represents the current repeated task.
    /// </summary>
    /// <returns>A string that represents the current repeated task.</returns>
    public override string ToString() => "RepeatedTask";
}
