using TimeInABottle.Core.Helpers;

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
    public DerivedTask(string name, string description, TimeOnly start, TimeOnly end, DateOnly date) : base(name, description, start, end)
    {
        AssignedDate = date;
    }

    public DerivedTask() : base() // For EF Core
    {
    }

    public DateOnly AssignedDate {
        get; set;
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => "DerivedTask";
    public override object Accept(GetTaskSpecialtiesVisitor visitor) {
        return AssignedDate;
        //return null;
    }

    public override IEnumerable<int> GetWeekdaysInt() { 
        int dayOfWeek = (int)AssignedDate.DayOfWeek;
        return new List<int> { dayOfWeek };
    }
}
