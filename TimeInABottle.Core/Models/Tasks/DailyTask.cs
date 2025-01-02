using TimeInABottle.Core.Helpers;

namespace TimeInABottle.Core.Models.Tasks;
/// <summary>
/// Represents a daily task that repeats every day.
/// </summary>
public class DailyTask : IRepeatedTask
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DailyTask"/> class.
    /// </summary>
    /// <param name="name">The name of the task.</param>
    /// <param name="description">The description of the task.</param>
    /// <param name="startingTime">The starting time of the task.</param>
    /// <param name="endingTime">The ending time of the task.</param>
    public DailyTask(string name, string description, TimeOnly startingTime, TimeOnly endingTime)
        : base(name, description, startingTime, endingTime)
    {
    }

    public DailyTask() : base() // For EF Core
    {
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => $"DailyTask";
    public override object Accept(GetTaskSpecialtiesVisitor visitor) {
        return null;
    }

    public override IEnumerable<int> GetWeekdaysInt() => new List<int> {0,1,2,3,4,5,6};
}
