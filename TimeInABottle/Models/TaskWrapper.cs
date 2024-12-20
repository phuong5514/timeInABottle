using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Models;
public class TaskWrapper
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

    public required ITask Task
    {
        set; get;
    }

    public TimeSpan EstimatedCompletionTime
    {
        set; get;
    }

    public Importance ImportanceLevel
    {
        set; get;
    }

    public Urgency UrgencyLevel
    {
        set; get;
    }

    public Difficulty DifficultyLevel
    {
        set; get;
    }

    public bool ParentAsDeadline
    {
        set; get;
    }

    public TimeSpan ExpectedDuration
    {
        set; get;
    }
}
