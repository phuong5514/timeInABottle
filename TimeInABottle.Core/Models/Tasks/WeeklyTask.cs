﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    /// <summary>
    /// Initializes a new instance of the <see cref="WeeklyTask"/> class.
    /// </summary>
    /// <param name="name">The name of the weekly task.</param>
    /// <param name="description">The description of the weekly task.</param>
    /// <param name="startingTime">The starting time of the weekly task.</param>
    /// <param name="endingTime">The ending time of the weekly task.</param>
    /// <param name="weekdays">The list of weekdays on which the task occurs.</param>
    public WeeklyTask(string name, string description, TimeOnly startingTime, TimeOnly endingTime, List<Values.Weekdays> weekdays)
        : base(name, description, startingTime, endingTime)
    {
        WeekDays = weekdays;
    }

    /// <summary>
    /// Gets or sets the list of weekdays on which the task occurs.
    /// </summary>
    public List<Values.Weekdays> WeekDays
    {
        get; set;
    }

    /// <summary>
    /// Returns a string that represents the current weekly task.
    /// </summary>
    /// <returns>A string that represents the current weekly task.</returns>
    public override string ToString() => "WeeklyTask";
}
