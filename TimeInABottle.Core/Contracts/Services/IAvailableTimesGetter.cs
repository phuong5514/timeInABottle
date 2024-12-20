using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Core.Contracts.Services;
public interface IAvailableTimesGetter
{

    public IEnumerable<TimeSpan> GetAvailableTimes(IEnumerable<ITask> taskList);
    public IEnumerable<TimeSpan> GetAvailableTimesForToday();
    public IEnumerable<TimeSpan> GetAvailableTimesForDate(DateOnly date);
    public IEnumerable<TimeSpan> GetAvailableTimesForDate(int date);
    public IEnumerable<TimeSpan> GetAvailableTimesForWeek(IEnumerable<DayOfWeek> inputWeekDays);
    public IEnumerable<TimeSpan> GetAvailableTimesForWeek();
    public IEnumerable<TimeSpan> GetAvailableTimesForNextWeek();
    public IEnumerable<TimeSpan> GetAvailableTimesForWeekFromNow();
    public IEnumerable<TimeSpan> GetAvailableTimesForTodayFromNow();
}
