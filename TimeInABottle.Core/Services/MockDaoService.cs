using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Models;

namespace TimeInABottle.Core.Services;
public class MockDaoService : IDaoService
{
    public List<ITask> GetAllTasks() => throw new NotImplementedException();
    public List<ITask> GetThisMonthTasks() => throw new NotImplementedException();
    public List<ITask> GetThisWeekTasks() => throw new NotImplementedException();
    public List<ITask> GetTodayTasks() => throw new NotImplementedException();
}
