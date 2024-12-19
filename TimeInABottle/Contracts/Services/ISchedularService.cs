using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Contracts.Services;
public interface ISchedularService
{
    IEnumerable<DerivedTask> ScheduleThisWeek(IEnumerable<TaskWrapper> tasks);
    IEnumerable<DerivedTask> ScheduleNextWeek(IEnumerable<TaskWrapper> tasks);
    IEnumerable<DerivedTask> ScheduleThisWeekFromNow(IEnumerable<TaskWrapper> tasks);
}


public class TaskWrapper
{
    public enum Importance
    {
        Low,
        Medium,
        High
    }
    public enum Urgency
    {
        Low,
        Medium,
        High
    }
    public enum Difficulty
    {
        Low,
        Medium,
        High
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
}
