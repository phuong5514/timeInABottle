using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Models;
public class TaskWrapper(ITask task) : INotifyPropertyChanged
{
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

    //public TimeSpan ExpectedDuration
    //{
    //    set; get;
    //}

    public event PropertyChangedEventHandler? PropertyChanged;

    public static Array ImportanceValues => Enum.GetValues(typeof(Importance));
    public static Array UrgencyValues => Enum.GetValues(typeof(Urgency));
    public static Array DifficultyValues => Enum.GetValues(typeof(Difficulty));
}

