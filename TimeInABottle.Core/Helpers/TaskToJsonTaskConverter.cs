using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Core.Helpers;
internal class TaskToJsonTaskConverter
{
    /// <summary>
    /// Converts an <see cref="ITask"/> object to a <see cref="JsonTask"/> object.
    /// </summary>
    /// <param name="task">The task to convert.</param>
    /// <returns>A <see cref="JsonTask"/> object representing the converted task.</returns>
    public static JsonTask Convert(ITask task)
    {
        return new JsonTask
        {
            Id = task.Id,
            Name = task.Name,
            Description = task.Description,
            Start = task.Start,
            End = task.End,
            Date = (task is NonRepeatedTask nonRepeatedTask) ? nonRepeatedTask.Date : default,
            DayOfMonth = (task is MonthlyTask monthlyTask) ? monthlyTask.Date : default,
            DaysOfWeek = (task is WeeklyTask weeklyTask) ? weeklyTask.WeekDays : default
        };
    }

    /// <summary>
    /// Converts a list of <see cref="ITask"/> objects to a list of <see cref="JsonTask"/> objects.
    /// </summary>
    /// <param name="tasks">The tasks to convert.</param>
    /// <returns>A list of <see cref="JsonTask"/> objects representing the converted tasks.</returns>
    public static List<JsonTask> ConvertList(IEnumerable<ITask> tasks)
    {
        List<JsonTask> jsonTasks = new();
        foreach (var task in tasks)
        {
            jsonTasks.Add(Convert(task));
        }
        return jsonTasks;
    }

    /// <summary>
    /// Converts a <see cref="JsonTask"/> object back to an <see cref="ITask"/> object.
    /// </summary>
    /// <param name="jsonTask">The JSON task to convert.</param>
    /// <returns>An <see cref="ITask"/> object representing the converted JSON task.</returns>
    public static ITask ConvertBack(JsonTask jsonTask)
    {
        if (jsonTask.DaysOfWeek != null)
        {
            return new WeeklyTask(jsonTask.Name, jsonTask.Description, jsonTask.Start, jsonTask.End, jsonTask.DaysOfWeek);
        }
        else if (jsonTask.DayOfMonth != default)
        {
            return new MonthlyTask(jsonTask.Name, jsonTask.Description, jsonTask.Start, jsonTask.End, jsonTask.DayOfMonth);
        }
        else if (jsonTask.Date != default)
        {
            return new NonRepeatedTask(jsonTask.Name, jsonTask.Description, jsonTask.Start, jsonTask.End, jsonTask.Date);
        }
        else
        {
            return new DailyTask(jsonTask.Name, jsonTask.Description, jsonTask.Start, jsonTask.End);
        }
    }

    /// <summary>
    /// Converts a list of <see cref="JsonTask"/> objects back to a list of <see cref="ITask"/> objects.
    /// </summary>
    /// <param name="jsonTasks">The JSON tasks to convert.</param>
    /// <returns>A list of <see cref="ITask"/> objects representing the converted JSON tasks.</returns>
    public static IEnumerable<ITask> ConvertListBack(IEnumerable<JsonTask> jsonTasks)
    {
        List<ITask> tasks = new();
        foreach (var jsonTask in jsonTasks)
        {
            tasks.Add(ConvertBack(jsonTask));
        }
        return tasks;
    }
}
