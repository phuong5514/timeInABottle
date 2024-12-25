using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Core.Models.Tasks;
using TimeInABottle.Models;

namespace TimeInABottle.Contracts.Services;
public interface IPlannerService
{
    IEnumerable<DerivedTask> ScheduleThisWeek(IEnumerable<TaskWrapper> tasks);
    IEnumerable<DerivedTask> ScheduleNextWeek(IEnumerable<TaskWrapper> tasks);
    IEnumerable<DerivedTask> ScheduleThisWeekFromNow(IEnumerable<TaskWrapper> tasks);
}


