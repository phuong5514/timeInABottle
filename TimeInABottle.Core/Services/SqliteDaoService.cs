using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models.Filters;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Core.Services;
public class SqliteDaoService : IDaoService
{
    private readonly TaskContext _db;
    private string _exportPath;

    public SqliteDaoService()
    {
        var configPath = Path.Combine(AppContext.BaseDirectory, "secret.config");
        if (File.Exists(configPath))
        {
            var config = XElement.Load(configPath);
            var filename = config.Element("Dao")?.Element("bgComFile")?.Value;
            if (filename != null)
            {
                _exportPath = $"{filename}.json";
            }
            else
            {
            }
        }

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
                (task is NonRepeatedTask nrt && nrt.Date.Month == currentMonth)
            )
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
                (task is NonRepeatedTask nrt && nrt.Date >= startOfWeek && nrt.Date <= endOfWeek) ||
                task is DailyTask ||
                (task is MonthlyTask mt && mt.Date >= startOfWeek.Day && (mt.Date <= endOfWeek.Day || endOfWeek.Day < startOfWeek.Day))
            )
            .ToList();
        return new FullObservableCollection<ITask>(thisWeekTasks);
    }

    public FullObservableCollection<ITask> GetTodayTasks() {
        var tasks = _db.Tasks.ToList();
        var today = DateOnly.FromDateTime(DateTime.Now);
        var todayTasks = tasks
            .Where(task =>
                task is DailyTask ||
                (task is NonRepeatedTask nrt && nrt.Date == today) ||
                task is MonthlyTask mt && mt.Date == today.Day ||
                (task is WeeklyTask wt && wt.WeekDays.Contains(today.DayOfWeek))
            )
            .ToList();

        return new FullObservableCollection<ITask>(todayTasks);
    }


    public void AddTask(ITask task) {
        _db.Tasks.Add(task);
        saveChanges();
    }

    // potentially deprecated and unused
    public void UpdateTask(ITask task) {
        var existingTask = _db.Tasks.Find(task.Id);
        if (existingTask != null)
        {
            _db.Entry(existingTask).CurrentValues.SetValues(task);
            saveChanges();
        }
        else
        {
            // Handle the case where the task does not exist in the database
            throw new InvalidOperationException("Task not found.");
        }

        //_db.Tasks.Update(task);
        //saveChanges();
    }

    public void DeleteTask(ITask task)
    {
        _db.Tasks.Remove(task);
        saveChanges();
    }

    private void saveChanges()
    {
        _db.SaveChanges();
        Export();
    }

    public void Export() {
        var tasks = _db.Tasks.ToList();
        var today = DateOnly.FromDateTime(DateTime.Now);
        var optimisedTasks = tasks
            .Where(task =>
                task is DailyTask ||
                (task is NonRepeatedTask nrt && nrt.Date >= today) ||
                task is MonthlyTask ||
                task is WeeklyTask)
            .ToList();


        var jsonTasks = TaskToJsonTaskConverter.ConvertList(optimisedTasks);
        LocalStorageWriter.Write(_exportPath, jsonTasks);
    }

    public FullObservableCollection<ITask> FindTaskFromDate(DateOnly date) {
        var tasks = _db.Tasks.ToList();
        var filteredTasks = tasks
            .Where(task =>
                task is DailyTask ||
                (task is NonRepeatedTask nrt && nrt.Date == date) ||
                (task is MonthlyTask mt && mt.Date == date.Day) ||
                (task is WeeklyTask wt && wt.WeekDays.Contains(date.DayOfWeek))
            ).ToList();

        return new FullObservableCollection<ITask>(filteredTasks);
    }

    public FullObservableCollection<ITask> GetThisWeekTasks(IEnumerable<DayOfWeek> weekdays) {
        var tasks = _db.Tasks.ToList();
        var startOfWeek = DateOnly.FromDateTime(DateTime.Now.StartOfWeek(DayOfWeek.Monday));
        var endOfWeek = startOfWeek.AddDays(6);
        var filteredTasks = tasks
            .Where(task =>
                (task is WeeklyTask wt && wt.WeekDays.Intersect(weekdays).Any()) ||
                (task is NonRepeatedTask nrt && weekdays.Contains(nrt.Date.DayOfWeek) && nrt.Date <= endOfWeek) ||
                task is DailyTask
            ).ToList();

        var monthlyTasks = tasks
            .Where(task =>
                task is MonthlyTask mt && weekdays.Contains(DayOfWeekGetter.GetDayOfWeekThisMonth(mt.Date)) && (mt.Date <= endOfWeek.Day || endOfWeek.Day <= startOfWeek.Day) && mt.Date >= startOfWeek.Day
            ).ToList();

        filteredTasks.AddRange(monthlyTasks);
        return new FullObservableCollection<ITask>(filteredTasks);
    }
}   
