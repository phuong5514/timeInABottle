using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models;

namespace TimeInABottle.Core.Services;
public class MockDaoService : IDaoService
{
    FullObservableCollection<ITask> IDaoService.GetAllTasks() {
        var list = new List<ITask>
        {
            new DailyTask("Name E", "Desc F", new TimeOnly(10, 30), new TimeOnly(12, 00)),
            new DailyTask("Name G", "Desc H", new TimeOnly(13, 15), new TimeOnly(14, 00)),
            new DailyTask("Name A", "Desc B", new TimeOnly(6, 0), new TimeOnly(7, 30)),
            new DailyTask("Name C", "Desc D", new TimeOnly(8, 0), new TimeOnly(10, 0)),
            new DailyTask("Name L", "Desc M", new TimeOnly(17, 30), new TimeOnly(20, 00)),
            new DailyTask("Name J", "Desc K", new TimeOnly(15, 20), new TimeOnly(16, 30))
        };

        var sorter = new TaskListSorter();
        sorter.SortByTimeAscending(list);
        return new FullObservableCollection<ITask>(list);
    }

    FullObservableCollection<ITask> IDaoService.GetThisMonthTasks() { 
        return new FullObservableCollection<ITask>();

    }

    FullObservableCollection<ITask> IDaoService.GetThisWeekTasks() { 
        return new FullObservableCollection<ITask>();

    }

    FullObservableCollection<ITask> IDaoService.GetTodayTasks() { 
        return new FullObservableCollection<ITask>();

    }
}
