using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Core.Helpers;
internal class TaskToJsonTaskConverter
{
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

    public static List<JsonTask> ConvertList(IEnumerable<ITask> tasks)
    {
        List<JsonTask> jsonTasks = new();
        foreach (var task in tasks)
        {
            jsonTasks.Add(Convert(task));
        }
        return jsonTasks;
    }

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
