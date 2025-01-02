using System.ComponentModel;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Models;
public class TaskWrapper(ITask task) : INotifyPropertyChanged
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TaskWrapper"/> class by copying values from another instance.
    /// </summary>
    /// <param name="value">The <see cref="TaskWrapper"/> instance to copy values from.</param>
    public TaskWrapper(TaskWrapper value) : this(value.Task)
    {
        EstimatedCompletionTime = value.EstimatedCompletionTime;
        ImportanceLevel = value.ImportanceLevel;
        UrgencyLevel = value.UrgencyLevel;
        DifficultyLevel = value.DifficultyLevel;
        ParentAsDeadline = value.ParentAsDeadline;
    }

    public enum Importance
    {
        Low = 1,
        Medium = 2,
        High = 3
    }
    public enum Urgency
    {
        Low = 1,
        Medium = 2,
        High = 3
    }
    public enum Difficulty
    {
        Low = 1,
        Medium = 2,
        High = 3
    }

    public ITask Task
    {
        set; get;
    } = task;

    public TimeSpan EstimatedCompletionTime
    {
        set; get;
    } = TimeSpan.Zero;

    public Importance ImportanceLevel
    {
        set; get;
    } = Importance.Medium;

    public Urgency UrgencyLevel
    {
        set; get;
    } = Urgency.Medium;

    public Difficulty DifficultyLevel
    {
        set; get;
    } = Difficulty.Medium;

    public bool ParentAsDeadline
    {
        set; get;
    } = false;


    public event PropertyChangedEventHandler? PropertyChanged;

    public static Array ImportanceValues => Enum.GetValues(typeof(Importance));
    public static Array UrgencyValues => Enum.GetValues(typeof(Urgency));
    public static Array DifficultyValues => Enum.GetValues(typeof(Difficulty));

    internal void CopyFrom(TaskWrapper other)
    {
        if (other == null) return;

        DifficultyLevel = other.DifficultyLevel;
        UrgencyLevel = other.UrgencyLevel;
        ImportanceLevel = other.ImportanceLevel;
        EstimatedCompletionTime = other.EstimatedCompletionTime;
        ParentAsDeadline = other.ParentAsDeadline;
    }
}

