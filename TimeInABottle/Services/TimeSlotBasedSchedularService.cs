using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Contracts.Services;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Models.Tasks;
using TimeInABottle.Core.Services;

namespace TimeInABottle.Services;
public class TimeSlotBasedSchedularService : ISchedularService
{
    private readonly IAvailableTimesGetter _availableTimesGetter;

    public TimeSlotBasedSchedularService()
    {
        _availableTimesGetter = App.GetService<IAvailableTimesGetter>();
    }

    public IEnumerable<DerivedTask> ScheduleNextWeek(IEnumerable<TaskWrapper> tasks) {
        var availableTime = _availableTimesGetter.GetAvailableTimesForToday();
        
    }


    public IEnumerable<DerivedTask> ScheduleThisWeek(IEnumerable<TaskWrapper> tasks)
    {

    }

    public IEnumerable<DerivedTask> ScheduleThisWeekFromNow(IEnumerable<TaskWrapper> tasks)
    {


    }
}
