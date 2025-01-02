using System.Xml.Linq;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models.Filters;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Core.Services;
/// <summary>
/// Service for interacting with SQLite database for task management.
/// </summary>
public class SqliteDaoService : IDaoService
{
    private readonly TaskContext _db;
    private readonly string _exportPath;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqliteDaoService"/> class.
    /// Loads configuration and initializes the database context.
    /// </summary>
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
                // Handle the case where the filename is not found in the configuration
            }
        }

        _db = new();
    }

    /// <summary>
    /// Executes a custom query based on the provided filter and sorting order.
    /// </summary>
    /// <param name="filter">The filter criteria to apply to the query.</param>
    /// <param name="isSortAscending">Indicates whether the results should be sorted in ascending order.</param>
    /// <returns>A collection of tasks that match the filter criteria.</returns>
    public FullObservableCollection<ITask> CustomQuery(IFilter filter, bool isSortAscending = true)
    {
        var tasks = _db.Tasks.ToList();
        var filteredTasks = tasks.Where(task => filter.MatchesCriteria(task)).ToList();
        return new FullObservableCollection<ITask>(filteredTasks);
    }

    /// <summary>
    /// Retrieves all tasks.
    /// </summary>
    /// <returns>A collection of all tasks.</returns>
    public FullObservableCollection<ITask> GetAllTasks()
    {
        var tasks = _db.Tasks.ToList();
        return new FullObservableCollection<ITask>(tasks);
    }

    /// <summary>
    /// Retrieves tasks scheduled for the current month.
    /// </summary>
    /// <returns>A collection of this month's tasks.</returns>
    public FullObservableCollection<ITask> GetThisMonthTasks()
    {
        var tasks = _db.Tasks.ToList();
        var currentMonth = DateOnly.FromDateTime(DateTime.Now).Month;
        var thisMonthTasks = tasks
            .Where(task =>
                task is MonthlyTask ||
                (task is NonRepeatedTask nrt && nrt.Date.Month == currentMonth) ||
                (task is DerivedTask dt && dt.AssignedDate.Month == currentMonth)
            )
            .ToList();

        return new FullObservableCollection<ITask>(thisMonthTasks);
    }

    /// <summary>
    /// Retrieves tasks scheduled for the current week.
    /// </summary>
    /// <returns>A collection of this week's tasks.</returns>
    public FullObservableCollection<ITask> GetThisWeekTasks()
    {
        var tasks = _db.Tasks.ToList();
        var startOfWeek = DateOnly.FromDateTime(DateTime.Now.StartOfWeek(DayOfWeek.Monday));
        var endOfWeek = startOfWeek.AddDays(6);
        var thisWeekTasks = tasks
            .Where(task =>
                task is WeeklyTask ||
                (task is NonRepeatedTask nrt && nrt.Date >= startOfWeek && nrt.Date <= endOfWeek) ||
                (task is DerivedTask dt && dt.AssignedDate >= startOfWeek && dt.AssignedDate <= endOfWeek) ||
                task is DailyTask ||
                (task is MonthlyTask mt && mt.Date >= startOfWeek.Day && (mt.Date <= endOfWeek.Day || endOfWeek.Day < startOfWeek.Day))
            )
            .ToList();
        return new FullObservableCollection<ITask>(thisWeekTasks);
    }

    /// <summary>
    /// Retrieves tasks scheduled for today.
    /// </summary>
    /// <returns>A collection of today's tasks.</returns>
    public FullObservableCollection<ITask> GetTodayTasks()
    {
        var tasks = _db.Tasks.ToList();
        var today = DateOnly.FromDateTime(DateTime.Now);
        var todayTasks = tasks
            .Where(task =>
                task is DailyTask ||
                (task is NonRepeatedTask nrt && nrt.Date == today) ||
                (task is DerivedTask dt && dt.AssignedDate == today) ||
                task is MonthlyTask mt && mt.Date == today.Day ||
                (task is WeeklyTask wt && wt.WeekDays.Contains(today.DayOfWeek))
            )
            .ToList();

        return new FullObservableCollection<ITask>(todayTasks);
    }

    /// <summary>
    /// Adds a new task to the database.
    /// </summary>
    /// <param name="task">The task to add.</param>
    public void AddTask(ITask task)
    {
        _db.Tasks.Add(task);
        saveChanges();
    }

    /// <summary>
    /// Updates an existing task in the database.
    /// </summary>
    /// <param name="task">The task to update.</param>
    /// <exception cref="InvalidOperationException">Thrown when the task is not found in the database.</exception>
    public void UpdateTask(ITask task)
    {
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
    }

    /// <summary>
    /// Deletes a task from the database.
    /// </summary>
    /// <param name="task">The task to delete.</param>
    public void DeleteTask(ITask task)
    {
        _db.Tasks.Remove(task);
        saveChanges();
    }

    /// <summary>
    /// Saves changes to the database and exports the tasks to a JSON file.
    /// </summary>
    private void saveChanges()
    {
        _db.SaveChanges();
        Export();
    }

    /// <summary>
    /// Exports the tasks to a JSON file.
    /// </summary>
    public void Export()
    {
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

    /// <summary>
    /// Finds tasks scheduled for a specific date.
    /// </summary>
    /// <param name="date">The date to search for tasks.</param>
    /// <returns>A collection of tasks scheduled for the specified date.</returns>
    public FullObservableCollection<ITask> FindTaskFromDate(DateOnly date)
    {
        var tasks = _db.Tasks.ToList();
        var filteredTasks = tasks
            .Where(task =>
                task is DailyTask ||
                (task is NonRepeatedTask nrt && nrt.Date == date) ||
                (task is DerivedTask dt && dt.AssignedDate == date) ||
                (task is MonthlyTask mt && mt.Date == date.Day) ||
                (task is WeeklyTask wt && wt.WeekDays.Contains(date.DayOfWeek))
            ).ToList();

        return new FullObservableCollection<ITask>(filteredTasks);
    }

    /// <summary>
    /// Retrieves tasks scheduled for the current week based on specified weekdays.
    /// </summary>
    /// <param name="weekdays">The weekdays to filter tasks.</param>
    /// <returns>A collection of tasks scheduled for the specified weekdays.</returns>
    public FullObservableCollection<ITask> GetThisWeekTasks(IEnumerable<DayOfWeek> weekdays)
    {
        var tasks = _db.Tasks.ToList();
        var startOfWeek = DateOnly.FromDateTime(DateTime.Now.StartOfWeek(DayOfWeek.Monday));
        var endOfWeek = startOfWeek.AddDays(6);
        var filteredTasks = tasks
            .Where(task =>
                (task is WeeklyTask wt && wt.WeekDays.Intersect(weekdays).Any()) ||
                (task is NonRepeatedTask nrt && weekdays.Contains(nrt.Date.DayOfWeek) && nrt.Date <= endOfWeek) ||
                (task is DerivedTask dt && weekdays.Contains(dt.AssignedDate.DayOfWeek) && dt.AssignedDate <= endOfWeek) ||
                task is DailyTask
            ).ToList();

        var monthlyTasks = tasks
            .Where(task =>
                task is MonthlyTask mt && weekdays.Contains(DayOfWeekGetter.GetDayOfWeekThisMonth(mt.Date)) && (mt.Date <= endOfWeek.Day || endOfWeek.Day <= startOfWeek.Day) && mt.Date >= startOfWeek.Day
            ).ToList();

        filteredTasks.AddRange(monthlyTasks);
        return new FullObservableCollection<ITask>(filteredTasks);
    }

    /// <summary>
    /// Retrieves tasks scheduled for the next week.
    /// </summary>
    /// <returns>A collection of next week's tasks.</returns>
    public FullObservableCollection<ITask> GetNextWeekTasks()
    {
        var tasks = _db.Tasks.ToList();
        var startOfWeek = DateOnly.FromDateTime(DateTime.Now.StartOfWeek(DayOfWeek.Monday)).AddDays(7);
        var endOfWeek = startOfWeek.AddDays(6);

        var thisWeekTasks = tasks
            .Where(task =>
                task is WeeklyTask ||
                (task is NonRepeatedTask nrt && nrt.Date >= startOfWeek && nrt.Date <= endOfWeek) ||
                (task is DerivedTask dt && dt.AssignedDate >= startOfWeek && dt.AssignedDate <= endOfWeek) ||
                task is DailyTask ||
                (task is MonthlyTask mt && mt.Date >= startOfWeek.Day && (mt.Date <= endOfWeek.Day || endOfWeek.Day < startOfWeek.Day))
            )
            .ToList();
        return new FullObservableCollection<ITask>(thisWeekTasks);
    }

    /// <summary>
    /// Retrieves tasks scheduled for the current week from now.
    /// </summary>
    /// <returns>A collection of tasks scheduled for the rest of the week from now.</returns>
    public FullObservableCollection<ITask> GetThisWeekTasksFromNow()
    {
        var tasks = _db.Tasks.ToList();
        var endOfWeek = DateOnly.FromDateTime(DateTime.Now.StartOfWeek(DayOfWeek.Monday)).AddDays(6);
        var today = DateOnly.FromDateTime(DateTime.Now);
        var now = TimeOnly.FromDateTime(DateTime.Now);

        var filteredTasks = tasks
            .Where(task =>
                (task is NonRepeatedTask nrt && (nrt.Date > today && nrt.Date <= endOfWeek || (nrt.Date == today && nrt.Start > now))) ||
                (task is DerivedTask dtk && (dtk.AssignedDate > today && dtk.AssignedDate <= endOfWeek || (dtk.AssignedDate == today && dtk.Start > now))) ||
                task is DailyTask dt && dt.Start > now ||
                (task is MonthlyTask mt && ((mt.Date > today.Day && (mt.Date <= endOfWeek.Day || endOfWeek.Day < today.Day)) || mt.Date == today.Day && mt.Start > now)) ||
                (task is WeeklyTask wt)
            ).ToList();
        return new FullObservableCollection<ITask>(filteredTasks);
    }

    /// <summary>
    /// Adds multiple tasks to the database.
    /// </summary>
    /// <param name="tasks">The tasks to add.</param>
    public void AddTasks(IEnumerable<ITask> tasks)
    {
        foreach (var task in tasks)
        {
            _db.Tasks.Add(task);
        }
        saveChanges();
    }

    /// <summary>
    /// Updates multiple tasks in the database.
    /// </summary>
    /// <param name="tasks">The tasks to update.</param>
    public void UpdateTasks(IEnumerable<ITask> tasks)
    {
        foreach (var task in tasks)
        {
            UpdateTask(task);
        }
    }

    /// <summary>
    /// Deletes multiple tasks from the database.
    /// </summary>
    /// <param name="tasks">The tasks to delete.</param>
    public void DeleteTasks(IEnumerable<ITask> tasks)
    {
        foreach (var task in tasks)
        {
            DeleteTask(task);
        }
    }
}
