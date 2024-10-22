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
            new DailyTask("Name A", "Desc B", new Time(6, 0), new Time(7, 30)),
            new DailyTask("Name C", "Desc D", new Time(8, 0), new Time(10, 0)),
            new DailyTask("Name E", "Desc F", new Time(10, 30), new Time(12, 00)),
            new DailyTask("Name G", "Desc H", new Time(13, 15), new Time(14, 00)),
            new DailyTask("Name J", "Desc K", new Time(15, 20), new Time(16, 30)),
            new DailyTask("Name L", "Desc M", new Time(17, 30), new Time(20, 00))
        };
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
