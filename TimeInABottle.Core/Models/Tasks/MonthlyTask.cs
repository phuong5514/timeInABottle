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
/// Represents a task that occurs monthly on a specific date.
/// </summary>
public class MonthlyTask : IRepeatedTask
{
    // TODO: sẽ xử lý trường hợp ngày trong tháng này không tồn tại trong tháng sau
    // vd: tháng 1 ngày 30 -> tháng 2 ngày 28/29

    /// <summary>
    /// Gets or sets the date of the month on which the task occurs.
    /// </summary>
    public int Date
    {
        get; set;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MonthlyTask"/> class.
    /// </summary>
    /// <param name="name">The name of the task.</param>
    /// <param name="description">The description of the task.</param>
    /// <param name="startingTime">The starting time of the task.</param>
    /// <param name="endingTime">The ending time of the task.</param>
    /// <param name="date">The date of the month on which the task occurs.</param>
    public MonthlyTask(string name, string description, TimeOnly startingTime, TimeOnly endingTime, int date)
        : base(name, description, startingTime, endingTime)
    {
        Date = date;
    }

    public MonthlyTask() : base() // For EF Core
    {
    }

    /// <summary>
    /// Returns a string that represents the current <see cref="MonthlyTask"/>.
    /// </summary>
    /// <returns>A string that represents the current <see cref="MonthlyTask"/>.</returns>
    public override string ToString() => "MonthlyTask";
    public override object Accept(GetTaskSpecialtiesVisitor visitor) {
        return visitor.VisitMonthlyTask(this);   
    }
}
