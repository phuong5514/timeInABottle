using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models.Filters;
using TimeInABottle.Core.Models.Tasks;
using TimeInABottle.Core.Models.Weather;

namespace TimeInABottle.Core.Services;
public class LocalStorageDaoService : IDaoService
{
    private readonly string _filename = "tasks.json";
    private readonly List<ITask> _taskList;
    public List<ITask> TaskList
    {
        get => _taskList; private set { }
    }

    public LocalStorageDaoService()
    {
        var configPath = Path.Combine(AppContext.BaseDirectory, "secret.config");
        if (File.Exists(configPath))
        {
            var config = XElement.Load(configPath);
            var filename = config.Element("Dao")?.Element("bgComFile")?.Value;
            if (filename != null)
            {
                _filename = $"{filename}.json";
            }
            else
            {
            }
        }

        var path = Path.Combine(AppContext.BaseDirectory, _filename);
        var jsonString = File.ReadAllText(path);
        var jsonTasks = JsonConvert.DeserializeObject<List<JsonTask>>(jsonString);
        _taskList = (List<ITask>)TaskToJsonTaskConverter.ConvertListBack(jsonTasks);
    }

    FullObservableCollection<ITask> IDaoService.GetTodayTasks()
    {
        var today = DateOnly.FromDateTime(DateTime.Now);

        var tasksToday = TaskList
            .Where(task =>
                task is DailyTask ||
                task is NonRepeatedTask nrt && nrt.Date == today ||
                task is MonthlyTask mt && mt.Date == today.Day ||
                task is WeeklyTask wt && wt.WeekDays.Contains(today.DayOfWeek))
            .ToList();

        var sorter = new TaskListSorter();
        sorter.SortByTimeAscending(tasksToday);

        return new FullObservableCollection<ITask>(tasksToday);
    }

    public FullObservableCollection<ITask> GetAllTasks() => throw new NotImplementedException();
    public FullObservableCollection<ITask> CustomQuery(IFilter filter, bool isSortAscending = true) => throw new NotImplementedException();
    public FullObservableCollection<ITask> FindTaskFromDate(DateOnly date) => throw new NotImplementedException();
    public FullObservableCollection<ITask> GetThisWeekTasks() => throw new NotImplementedException();
    public FullObservableCollection<ITask> GetThisMonthTasks() => throw new NotImplementedException();
    public FullObservableCollection<ITask> GetThisWeekTasks(IEnumerable<DayOfWeek> weekdays) => throw new NotImplementedException();
    public void AddTask(ITask task) => throw new NotImplementedException();
    public void UpdateTask(ITask task) => throw new NotImplementedException();
    public void DeleteTask(ITask task) => throw new NotImplementedException();
}
