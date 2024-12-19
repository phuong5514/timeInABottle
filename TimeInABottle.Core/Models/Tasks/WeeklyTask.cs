using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Helpers;


namespace TimeInABottle.Core.Models.Tasks;
/// <summary>
/// Represents a task that occurs on a weekly basis.
/// Inherits from the <see cref="IRepeatedTask"/> class.
/// </summary>
public class WeeklyTask : IRepeatedTask
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WeeklyTask"/> class.
    /// </summary>
    /// <param name="name">The name of the weekly task.</param>
    /// <param name="description">The description of the weekly task.</param>
    /// <param name="startingTime">The starting time of the weekly task.</param>
    /// <param name="endingTime">The ending time of the weekly task.</param>
    public WeeklyTask(string name, string description, TimeOnly startingTime, TimeOnly endingTime)
        : base(name, description, startingTime, endingTime)
    {
    }

    public WeeklyTask() : base() // For EF Core
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WeeklyTask"/> class.
    /// </summary>
    /// <param name="name">The name of the weekly task.</param>
    /// <param name="description">The description of the weekly task.</param>
    /// <param name="startingTime">The starting time of the weekly task.</param>
    /// <param name="endingTime">The ending time of the weekly task.</param>
    /// <param name="weekdays">The list of weekdays on which the task occurs.</param>
    public WeeklyTask(string name, string description, TimeOnly startingTime, TimeOnly endingTime, List<DayOfWeek> weekdays)
        : base(name, description, startingTime, endingTime)
    {
        WeekDays = weekdays;
    }

    /// <summary>
    /// Gets or sets the list of weekdays on which the task occurs.
    /// </summary>
    public List<DayOfWeek> WeekDays
    {
        get; set;
    }

    /// <summary>
    /// Returns a string that represents the current weekly task.
    /// </summary>
    /// <returns>A string that represents the current weekly task.</returns>
    public override string ToString() => "WeeklyTask";
    public override object Accept(GetTaskSpecialtiesVisitor visitor) { 
        return visitor.VisitWeeklyTask(this);
    }
}
