using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models.Filters;
using TimeInABottle.Core.Models.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TimeInABottle.Core.Services;
/// <summary>
/// MockDaoService provides a mock implementation of IDaoService and IDaoQueryService interfaces.
/// It manages a list of tasks and provides methods to query and sort these tasks.
/// </summary>
public class MockDaoService : IDaoService
{
    // List of tasks managed by the service
    private readonly List<ITask> _taskList = new()
    {
        // daily tasks
        new DailyTask("Morning routines", "Brush teeth, wash face, boil water, make breakfast", new TimeOnly(5, 30), new TimeOnly(6, 0)),
        new DailyTask("Lunch", "Eat at: abc place with friends", new TimeOnly(11, 0), new TimeOnly(12, 0)),
        new DailyTask("Test 12h", "Eat at: abc place with friends", new TimeOnly(12, 0), new TimeOnly(13, 0)),
        new DailyTask("Test 13h", "Eat at: abc place with friends", new TimeOnly(13, 0), new TimeOnly(14, 0)),
        new DailyTask("Test 14h", "Eat at: abc place with friends", new TimeOnly(14, 0), new TimeOnly(15, 0)),
        new DailyTask("Test 15h", "Eat at: abc place with friends", new TimeOnly(15, 0), new TimeOnly(16, 0)),
        new DailyTask("Test 16h", "Eat at: abc place with friends", new TimeOnly(16, 0), new TimeOnly(17, 0)),

        new DailyTask("Dinner and freetime", "none", new TimeOnly(17, 0), new TimeOnly(18, 30)),
        new DailyTask("Pre bedtime", "Brush teeth", new TimeOnly(21, 0), new TimeOnly(21, 30)),

        // weekly tasks
        new WeeklyTask("Weekend study", "current topics: xyz", new TimeOnly(19,0), new TimeOnly(20, 30)
            , new List<Values.Weekdays> { Values.Weekdays.Saturday, Values.Weekdays.Sunday }),
        new WeeklyTask("Hobbies", "", new TimeOnly(15,30), new TimeOnly(17, 0)
            , new List<Values.Weekdays> { Values.Weekdays.Tuesday, Values.Weekdays.Thursday, Values.Weekdays.Friday }),

        // monthly tasks
        new MonthlyTask("family gathering", "", new TimeOnly(18, 0), new TimeOnly (19, 0), 6),

        // non-repeated tasks
        new NonRepeatedTask("deadline: x", "turn in y", new TimeOnly(22, 0), new TimeOnly(23, 0), new DateOnly(2024, 11, 10)),
        new NonRepeatedTask("doctor's appointment", "annual checkup", new TimeOnly(9, 0), new TimeOnly(10, 0), new DateOnly(2024, 11, 15)),
        new NonRepeatedTask("concert", "live performance at central park", new TimeOnly(19, 0), new TimeOnly(22, 0), new DateOnly(2024, 11, 20)),
        new NonRepeatedTask("friend's wedding", "ceremony and reception", new TimeOnly(14, 0), new TimeOnly(20, 0), new DateOnly(2024, 12, 5)),
        new NonRepeatedTask("project milestone", "submit initial draft", new TimeOnly(16, 0), new TimeOnly(17, 0), new DateOnly(2024, 12, 12)),
        new NonRepeatedTask("job interview", "Interview with Company ABC", new TimeOnly(10, 0), new TimeOnly(11, 30), new DateOnly(2024, 12, 18))
    };

    // Property to get the list of tasks
    public List<ITask> TaskList { get => _taskList; private set { } }

    public void AddTask(ITask task) => throw new NotImplementedException();

    /// <summary>
    /// CustomQuery filters and sorts the task list based on the provided filter and sort order.
    /// </summary>
    /// <param name="filter">The filter to apply to the task list.</param>
    /// <param name="isSortAscending">Indicates whether the result should be sorted in ascending order.</param>
    /// <returns>A collection of tasks that match the filter criteria, sorted as specified.</returns>
    public FullObservableCollection<ITask> CustomQuery(IFilter filter, bool isSortAscending)
    {
        var result = TaskList
            .Where(task => filter.MatchesCriteria(task))
            .OrderBy(task => task.Start)
            .ToList();

        if (!isSortAscending)
        {
            result.Reverse();
        }

        return new FullObservableCollection<ITask>(result);
    }

    public void DeleteTask(ITask task) => throw new NotImplementedException();
    public void UpdateTask(ITask task) => throw new NotImplementedException();

    /// <summary>
    /// GetAllTasks retrieves all tasks and sorts them in ascending order.
    /// </summary>
    /// <returns>A collection of all tasks sorted in ascending order.</returns>
    FullObservableCollection<ITask> IDaoService.GetAllTasks()
    {
        var list = TaskList;

        var sorter = new TaskListSorter();
        sorter.SortByTimeAscending(list);
        return new FullObservableCollection<ITask>(list);
    }

    /// <summary>
    /// GetThisMonthTasks retrieves tasks for the current month and sorts them in ascending order.
    /// </summary>
    /// <returns>A collection of tasks for the current month sorted in ascending order.</returns>
    FullObservableCollection<ITask> IDaoService.GetThisMonthTasks()
    {
        var currentMonth = DateOnly.FromDateTime(DateTime.Now).Month;
        var tasksThisMonth = TaskList
            .Where(task =>
                task is MonthlyTask ||
                task is NonRepeatedTask nrt && nrt.Date.Month == currentMonth)
            .ToList();

        var sorter = new TaskListSorter();
        sorter.SortByTimeAscending(tasksThisMonth);

        return new FullObservableCollection<ITask>(tasksThisMonth);
    }

    /// <summary>
    /// GetThisWeekTasks retrieves tasks for the current week and sorts them in ascending order.
    /// </summary>
    /// <returns>A collection of tasks for the current week sorted in ascending order.</returns>
    FullObservableCollection<ITask> IDaoService.GetThisWeekTasks()
    {
        var startOfWeek = DateOnly.FromDateTime(DateTime.Now.StartOfWeek(DayOfWeek.Monday));
        var endOfWeek = startOfWeek.AddDays(6);

        var tasksThisWeek = TaskList
            .Where(task =>
                task is WeeklyTask ||
                task is NonRepeatedTask nrt && nrt.Date >= startOfWeek && nrt.Date <= endOfWeek ||
                task is MonthlyTask mt && mt.Date >= startOfWeek.Day && mt.Date <= endOfWeek.Day)
            .ToList();

        var sorter = new TaskListSorter();
        sorter.SortByTimeAscending(tasksThisWeek);

        return new FullObservableCollection<ITask>(tasksThisWeek);
    }

    /// <summary>
    /// GetTodayTasks retrieves tasks for the current day and sorts them in ascending order.
    /// </summary>
    /// <returns>A collection of tasks for the current day sorted in ascending order.</returns>
    FullObservableCollection<ITask> IDaoService.GetTodayTasks()
    {
        var today = DateOnly.FromDateTime(DateTime.Now);

        var tasksToday = TaskList
            .Where(task =>
                task is DailyTask ||
                (task is WeeklyTask wt && wt.WeekDays.Contains((Values.Weekdays)today.DayOfWeek)) ||
                (task is NonRepeatedTask nrt && nrt.Date == today) ||
                (task is MonthlyTask mt && mt.Date == today.Day))
            .ToList();

        var sorter = new TaskListSorter();
        sorter.SortByTimeAscending(tasksToday);

        return new FullObservableCollection<ITask>(tasksToday);
    }
}
