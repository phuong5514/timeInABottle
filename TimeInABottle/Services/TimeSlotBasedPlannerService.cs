using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Contracts.Services;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models.Tasks;
using TimeInABottle.Core.Services;
using TimeInABottle.Models;

namespace TimeInABottle.Services;
public class TimeSlotBasedPlannerService : IPlannerService
{
    private TimeSpan _minimumStart;
    private TimeSpan _maximumEnd;
    private int _increment;


    private readonly IAvailableTimesGetter _availableTimesGetter;

    // Dictionary to store the time slots for each day of the week
    // think of it like pagetable in OS
    private Dictionary<DayOfWeek, List<TimeSlot>> TimeSlots;


    private class TimeSlot
    {
        public TimeSpan StartTime
        {
            get; set;
        }
        public TimeSpan EndTime
        {
            get; set;
        }
    }

    public TimeSlotBasedPlannerService()
    {
        _availableTimesGetter = App.GetService<IAvailableTimesGetter>();
        ReadConfig();
        InitializeTimeSlots();
    }

    private void ReadConfig()
    {
        var configs = ConfigHandler.GetConfigValues(new[] { "SchedulingStartTime", "SchedulingEndTime", "TimeSlotIncrement" }).ToList();
        _minimumStart = TimeSpan.Parse(configs[0]);
        _maximumEnd = TimeSpan.Parse(configs[1]);
        _increment = int.Parse(configs[2]);
    }

    private FunctionResultCode LoadDayOfWeekTimeSlots(DayOfWeek dayOfWeek, IEnumerable<TimeSpan> availableTimes)
    {

        if (!TimeSlots.ContainsKey(dayOfWeek)) {
            return FunctionResultCode.ERROR_INVALID_INPUT;
        }

        var list = new List<TimeSlot>();
        var start = _minimumStart;
        var end = start;

        while (end < _maximumEnd)
        {
            // If the end time is available, increment it by _increment minutes: goal is to make the largest time slot possible
            if (availableTimes.Contains(end))
            {
                end = end.Add(TimeSpan.FromMinutes(_increment));
            }
            else
            {
                if (start != end)
                {
                    list.Add(new TimeSlot { StartTime = start, EndTime = end });
                }

                // skip the unavailable time slots
                while (!availableTimes.Contains(end))
                {
                    end = end.Add(TimeSpan.FromMinutes(_increment));
                }
                start = end;
            }
        }

        if (start != end) {
            list.Add(new TimeSlot { StartTime = start, EndTime = end });
        }


        TimeSlots[dayOfWeek] = list;
        return FunctionResultCode.SUCCESS;
    }


    private void InitializeTimeSlots()
    {
        TimeSlots = [];
        foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
        {
            TimeSlots.Add(day, new List<TimeSlot>());
        }
    }

    private double CalculatePriority(TaskWrapper taskWrapper)
    {
        const double importanceWeight = 0.4;
        const double urgencyWeight = 0.4;
        const double difficultyWeight = 0.2;

        var importanceScore = (int)taskWrapper.ImportanceLevel; // Low = 1, Medium = 2, High = 3
        var urgencyScore = (int)taskWrapper.UrgencyLevel;
        var difficultyScore = (int)taskWrapper.DifficultyLevel;

        var priority = (importanceScore * importanceWeight) +
                          (urgencyScore * urgencyWeight) +
                          (difficultyScore * difficultyWeight);

        if (taskWrapper.EstimatedCompletionTime == TimeSpan.Zero)
        {
            taskWrapper.EstimatedCompletionTime = EstimateDefaultTime(taskWrapper);
        }

        priority /= (taskWrapper.EstimatedCompletionTime.TotalHours + 1);

        if (taskWrapper.ParentAsDeadline)
        {
            var parentDeadline = DetermineParentDeadline(taskWrapper.Task);
            var timeLeft = parentDeadline - DateTime.Now;

            if (timeLeft.TotalDays > 0)
            {
                var parentWeight = 1.0 / (timeLeft.TotalDays + 1); // Dynamic weight
                priority += parentWeight;
            }
            else
            {
                priority += 2.0; // Fixed boost for overdue or imminent deadlines
            }
        }

        return priority;
    }

    private static List<DayOfWeek> DAY_OF_WEEKS =
        [
        DayOfWeek.Monday,
        DayOfWeek.Tuesday,
        DayOfWeek.Wednesday,
        DayOfWeek.Thursday,
        DayOfWeek.Friday,
        DayOfWeek.Saturday,
        DayOfWeek.Sunday
        ];

    public IEnumerable<DerivedTask> ScheduleThisWeek(IEnumerable<TaskWrapper> tasks)
    {
        var result = new List<DerivedTask>();
        var sortedTasks = tasks.OrderByDescending(task => CalculatePriority(task)).ToList();

        foreach (var day in DAY_OF_WEEKS)
        {
            var date = DateOnly.FromDateTime(DateTime.Now);
            date = date.AddDays((int)day - (int)date.DayOfWeek);
            if (day is DayOfWeek.Sunday)
            {
                date = date.AddDays(-7);
            }

            var availableTimes = _availableTimesGetter.GetAvailableTimesForWeek([day]);

            LoadDayOfWeekTimeSlots(day, availableTimes.Item1);

            var dayTimeSlots = TimeSlots[day];
            while (dayTimeSlots.Count > 0 && sortedTasks.Count > 0)
            {
                ProcessTaskInTimeSlot(dayTimeSlots, sortedTasks, result, date);
            }
        }

        return result;
    }

    public IEnumerable<DerivedTask> ScheduleThisWeekFromNow(IEnumerable<TaskWrapper> tasks)
    {
        var result = new List<DerivedTask>();
        var sortedTasks = tasks.OrderByDescending(task => CalculatePriority(task)).ToList();

        foreach (var day in DAY_OF_WEEKS)
        {
            var now = DateOnly.FromDateTime(DateTime.Now);

            var date = DateOnly.FromDateTime(DateTime.Now);
            date = date.AddDays((int)day - (int)date.DayOfWeek);
            if (day is DayOfWeek.Sunday) {
                date = date.AddDays(-7);
            }

            if (date < now)
            {
                continue;
            }

            Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> availableTimes;
            if (date == now)
            {
                availableTimes = _availableTimesGetter.GetAvailableTimesForTodayFromNow();
            }
            else
            {
                availableTimes = _availableTimesGetter.GetAvailableTimesForWeek([day]);
            }

            LoadDayOfWeekTimeSlots(day, availableTimes.Item1);

            var dayTimeSlots = TimeSlots[day];
            while (dayTimeSlots.Count > 0 && sortedTasks.Count > 0)
            {
                ProcessTaskInTimeSlot(dayTimeSlots, sortedTasks, result, date);
            }
        }

        return result;
    }


    // Handles processing a single task within a time slot
    private void ProcessTaskInTimeSlot(List<TimeSlot> dayTimeSlots, List<TaskWrapper> sortedTasks, List<DerivedTask> result, DateOnly date)
    {
        var timeSlot = dayTimeSlots[0];
        var task = sortedTasks[0];

        var start = TimeOnly.FromTimeSpan(timeSlot.StartTime);
        var timeSlotDuration = timeSlot.EndTime - timeSlot.StartTime;

        if (timeSlotDuration >= task.EstimatedCompletionTime)
        {
            AddDerivedTask(result, task.Task.Name, start, timeSlot.StartTime.Add(task.EstimatedCompletionTime), date);
            UpdateTimeSlotAfterTask(dayTimeSlots, timeSlot, task.EstimatedCompletionTime);
            sortedTasks.RemoveAt(0);
        }
        else
        {
            AddDerivedTask(result, task.Task.Name, start, timeSlot.EndTime, date);
            task.EstimatedCompletionTime -= timeSlotDuration;
            dayTimeSlots.RemoveAt(0);
        }
    }

    // Adds a DerivedTask to the result list
    private void AddDerivedTask(List<DerivedTask> result, string taskName, TimeOnly start, TimeSpan endTS, DateOnly date)
    {
        var end = TimeOnly.FromTimeSpan(endTS);
        result.Add(new DerivedTask($"Task from {taskName}", "", start, end, date));
    }

    // Updates a time slot after scheduling a task
    private void UpdateTimeSlotAfterTask(List<TimeSlot> dayTimeSlots, TimeSlot timeSlot, TimeSpan duration)
    {
        var endTS = timeSlot.StartTime.Add(duration);
        if (timeSlot.EndTime > endTS)
        {
            timeSlot.StartTime = endTS;
        }
        else
        {
            dayTimeSlots.RemoveAt(0);
        }
    }

    private TimeSpan EstimateDefaultTime(TaskWrapper taskWrapper)
    {
        // Base difficulty time in hours
        var baseDifficultyTime = taskWrapper.DifficultyLevel switch
        {
            TaskWrapper.Difficulty.Low => 1,       // 1 hour
            TaskWrapper.Difficulty.Medium => 3,    // 3 hours
            TaskWrapper.Difficulty.High => 6,      // 6 hours
            _ => 3 // Default to medium if unknown
        };

        // Importance multiplier
        var importanceMultiplier = taskWrapper.ImportanceLevel switch
        {
            TaskWrapper.Importance.Low => 0.8,
            TaskWrapper.Importance.Medium => 1.0,
            TaskWrapper.Importance.High => 1.2,
            _ => 1.0 // Default to medium
        };

        // Urgency multiplier
        var urgencyMultiplier = taskWrapper.UrgencyLevel switch
        {
            TaskWrapper.Urgency.Low => 0.8,
            TaskWrapper.Urgency.Medium => 1.0,
            TaskWrapper.Urgency.High => 1.2,
            _ => 1.0 // Default to medium
        };

        // Calculate default estimated time in minutes
        var totalMinutes = baseDifficultyTime * 60 * importanceMultiplier * urgencyMultiplier;

        // Round up to the nearest time unit
        var roundedMinutes = (int)Math.Ceiling(totalMinutes / _increment) * _increment;

        return TimeSpan.FromMinutes(roundedMinutes);
    }

    private DateTime DetermineParentDeadline(ITask task)
    {
        switch (task.TypeName())
        {
            case "WeeklyTask":
                var weeklyTask = task as WeeklyTask;
                return DateTime.Now.AddDays(weeklyTask.WeekDays.Min() - DateTime.Now.DayOfWeek);
            case "MonthlyTask":
                var monthlyTask = task as MonthlyTask;
                return new DateTime(DateTime.Now.Year, DateTime.Now.Month, monthlyTask.Date);
            case "NonRepeatedTask":
                var nonRepeatedTask = task as NonRepeatedTask;
                return nonRepeatedTask.Date.ToDateTime(nonRepeatedTask.Start);
            default:
                return DateTime.Now;
        }
    }


}
