using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.Internal;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Helpers;

namespace TimeInABottle.Core.Models.Tasks;

/// <summary>
/// Represents an abstract task with a name, description, start time, and end time.
/// Implements the INotifyPropertyChanged interface to support property change notifications.
/// </summary>
public abstract class ITask : INotifyPropertyChanged
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ITask"/> class.
    /// </summary>
    /// <param name="name">The name of the task.</param>
    /// <param name="description">The description of the task.</param>
    /// <param name="start">The start time of the task.</param>
    /// <param name="end">The end time of the task.</param>
    protected ITask(string name, string description, TimeOnly start, TimeOnly end)
    {
        Name = name;
        Description = description;
        Start = start;
        End = end;
    }

    protected ITask() // For EF Core
    {
    }

    [Key]
    public int Id
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the name of the task.
    /// </summary>
    public string Name
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the description of the task.
    /// </summary>
    public string Description
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the start time of the task.
    /// </summary>
    public TimeOnly Start
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the end time of the task.
    /// </summary>
    public TimeOnly End
    {
        get; set;
    }

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public abstract override string ToString();

    /// <summary>
    /// Gets the formatted time range of the task.
    /// </summary>
    public string FormattedTime => $"{Start.ToString()} - {End.ToString()}";
    
    public string TypeName() => GetType().Name;

    public abstract object Accept(GetTaskSpecialtiesVisitor visitor);
}
