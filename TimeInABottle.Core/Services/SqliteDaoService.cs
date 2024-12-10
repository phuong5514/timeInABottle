using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models.Filters;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Core.Services;
public class SqliteDaoService : IDaoService
{
    private readonly TaskContext _db;

    public SqliteDaoService()
    {
        _db = new();
    }

    public FullObservableCollection<ITask> CustomQuery(IFilter filter, bool isSortAscending = true)
    {
        var tasks = _db.Tasks.ToList();
        var filteredTasks = tasks.Where(task => filter.MatchesCriteria(task)).ToList();
        return new FullObservableCollection<ITask>(filteredTasks);
    }


    public FullObservableCollection<ITask> GetAllTasks() { 
        var tasks = _db.Tasks.ToList();
        return new FullObservableCollection<ITask>(tasks);
    }

    public FullObservableCollection<ITask> GetThisMonthTasks()
    {
        var tasks = _db.Tasks.ToList();
        var currentMonth = DateOnly.FromDateTime(DateTime.Now).Month;
        var thisMonthTasks = tasks
            .Where(task =>
                task is MonthlyTask ||
                task is NonRepeatedTask nrt && nrt.Date.Month == currentMonth)
            .ToList();

        return new FullObservableCollection<ITask>(thisMonthTasks);
    }

    public FullObservableCollection<ITask> GetThisWeekTasks() {
        var tasks = _db.Tasks.ToList();
        var startOfWeek = DateOnly.FromDateTime(DateTime.Now.StartOfWeek(DayOfWeek.Monday));
        var endOfWeek = startOfWeek.AddDays(6);
        var thisWeekTasks = tasks
            .Where(task =>
                task is WeeklyTask ||
                task is NonRepeatedTask nrt && nrt.Date >= startOfWeek && nrt.Date <= endOfWeek ||
                task is DailyTask)
            .ToList();
        return new FullObservableCollection<ITask>(thisWeekTasks);
    }

    public FullObservableCollection<ITask> GetTodayTasks() {
        var tasks = _db.Tasks.ToList();
        var today = DateOnly.FromDateTime(DateTime.Now);
        var todayTasks = tasks
            .Where(task =>
                task is DailyTask ||
                task is NonRepeatedTask nrt && nrt.Date == today ||
                task is MonthlyTask mt && mt.Date == today.Day ||
                task is WeeklyTask wt && wt.WeekDays.Contains((Values.Weekdays)today.DayOfWeek))
            .ToList();

        return new FullObservableCollection<ITask>(todayTasks);
    }


    public void AddTask(ITask task) { 
        _db.Tasks.Add(task);
        _db.SaveChanges();
    }

    // potentially deprecated and unused
    public void UpdateTask(ITask task) { 
        _db.Tasks.Update(task);
        _db.SaveChanges();
    }

    public void DeleteTask(ITask task)
    {
        _db.Tasks.Remove(task);
        _db.SaveChanges();
    }
}
