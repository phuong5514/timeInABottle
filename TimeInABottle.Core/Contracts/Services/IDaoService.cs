using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models;

namespace TimeInABottle.Core.Contracts.Services;
public interface IDaoService
{
    FullObservableCollection<ITask> GetAllTasks();

    FullObservableCollection<ITask> GetTodayTasks();

    FullObservableCollection<ITask> GetThisWeekTasks();

    FullObservableCollection<ITask> GetThisMonthTasks();

}
