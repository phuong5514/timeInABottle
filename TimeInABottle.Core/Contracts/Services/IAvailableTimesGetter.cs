using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Core.Contracts.Services;
public interface IAvailableTimesGetter
{

    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimes(IEnumerable<ITask> taskList);
    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForToday();
    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForDate(DateOnly date);
    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForDate(int date);
    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForWeek(IEnumerable<DayOfWeek> inputWeekDays);
    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForWeek();
    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForNextWeek();
    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForWeekFromNow();
    public Tuple<IEnumerable<TimeSpan>, IEnumerable<TimeSpan>> GetAvailableTimesForTodayFromNow();
}
