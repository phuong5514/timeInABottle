using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models;
/// <summary>
/// Represents a non-repeated task with a specific date.
/// Inherits from the abstract ITask class.
/// </summary>
public class NonRepeatedTask : ITask
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NonRepeatedTask"/> class.
    /// </summary>
    /// <param name="name">The name of the task.</param>
    /// <param name="description">The description of the task.</param>
    /// <param name="start">The start time of the task.</param>
    /// <param name="end">The end time of the task.</param>
    /// <param name="date">The date of the task.</param>
    public NonRepeatedTask(string name, string description, TimeOnly start, TimeOnly end, DateOnly date) : base(name, description, start, end)
    {
        Date = date;
    }

    /// <summary>
    /// Gets the date of the task.
    /// The date cannot be set in the past and must be within 3 months from the current date.
    /// </summary>
    public DateOnly Date
    {
        // TODO: hạn chế chỉ set ngày trong 1 giới hạn: ko thể set trong quá khứ,
        // và khoảng cách từ ngày cài đặt đến ngày thực hiện không quá 3 tháng (tùy chỉnh)
        get;
        private set;
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => "NonRepeatedTask";
}
