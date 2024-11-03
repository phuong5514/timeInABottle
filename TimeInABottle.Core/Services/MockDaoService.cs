using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TimeInABottle.Core.Services;
public class MockDaoService : IDaoService, IDaoQueryService
{
    private readonly List<ITask> _taskList = new()
    {
        // daily
        new DailyTask("Morning routines", "Brush teeth, wash face, boil water, make breakfast", new TimeOnly(5, 30), new TimeOnly(6, 0)),
        new DailyTask("Lunch", "Eat at: abc place with friends", new TimeOnly(11, 0), new TimeOnly(12, 0)),
        new DailyTask("Dinner and freetime", "none", new TimeOnly(17, 0), new TimeOnly(18, 30)),
        new DailyTask("Pre bedtime", "Brush teeth", new TimeOnly(21, 0), new TimeOnly(21, 30)),

        // weekly
        //public WeeklyTask(string name, string description, TimeOnly startingTime, TimeOnly endingTime)
        new WeeklyTask("Weekend study", "current topics: xyz", new TimeOnly(19,0), new TimeOnly(20, 30)
            , new List<Values.Weekdays> { Values.Weekdays.Saturday, Values.Weekdays.Sunday }),
        new WeeklyTask("Hobbies", "", new TimeOnly(15,30), new TimeOnly(17, 0)
            , new List<Values.Weekdays> { Values.Weekdays.Tuesday, Values.Weekdays.Thursday, Values.Weekdays.Friday }),

        // monthly
        //MonthlyTask(string name, string description, TimeOnly startingTime, TimeOnly endingTime, int date)
        new MonthlyTask("family gathering", "", new TimeOnly(18, 0), new TimeOnly (19, 0), 6),

        // non repeat
        //public NonRepeatedTask(string name, string description, TimeOnly start, TimeOnly end, DateOnly date)
        new NonRepeatedTask("deadline: x", "turn in y", new TimeOnly(22, 0), new TimeOnly(23, 0), new DateOnly(2024, 11, 10)),
        new NonRepeatedTask("doctor's appointment", "annual checkup", new TimeOnly(9, 0), new TimeOnly(10, 0), new DateOnly(2024, 11, 15)),
        new NonRepeatedTask("concert", "live performance at central park", new TimeOnly(19, 0), new TimeOnly(22, 0), new DateOnly(2024, 11, 20)),
        new NonRepeatedTask("friend's wedding", "ceremony and reception", new TimeOnly(14, 0), new TimeOnly(20, 0), new DateOnly(2024, 12, 5)),
        new NonRepeatedTask("project milestone", "submit initial draft", new TimeOnly(16, 0), new TimeOnly(17, 0), new DateOnly(2024, 12, 12)),
        new NonRepeatedTask("job interview", "Interview with Company ABC", new TimeOnly(10, 0), new TimeOnly(11, 30), new DateOnly(2024, 12, 18))

    };

    public List<ITask> TaskList { get; private set; }


    // trong tuong lai, ham nay chi co the dc goi boi class, ko nen dc goi tu ben ngoai ma thay vao do
    // la cac ham dac hieu
    public FullObservableCollection<ITask> CustomQuery(Func<ITask, bool> filter, bool isSortAscending) { 
        var result = TaskList.Where(filter).ToList();

        var sorter = new TaskListSorter();
        if (isSortAscending)
        {
            sorter.SortByTimeAscending(result);
        }
        else {
            sorter.SortByTimeDescending(result);
        }

        return new FullObservableCollection<ITask>(result);
    }

    FullObservableCollection<ITask> IDaoService.GetAllTasks()
    {
        var list = TaskList;

        var sorter = new TaskListSorter();
        sorter.SortByTimeAscending(list);
        return new FullObservableCollection<ITask>(list);
    }

    FullObservableCollection<ITask> IDaoService.GetThisMonthTasks()
    {
        var currentMonth = DateOnly.FromDateTime(DateTime.Now).Month;
        var tasksThisMonth = TaskList
            .Where(task =>
                task is MonthlyTask ||
                task is NonRepeatedTask nrt && nrt.Date.Month == currentMonth)
            .ToList();

        var sorter = new TaskListSorter();

        //sorter.SortByDateAndTimeAscending(tasksThisMonth);
        sorter.SortByTimeAscending(tasksThisMonth);

        return new FullObservableCollection<ITask>(tasksThisMonth);
    }

    FullObservableCollection<ITask> IDaoService.GetThisWeekTasks()
    {
        //https://stackoverflow.com/questions/38039/how-can-i-get-the-datetime-for-the-start-of-the-week
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
