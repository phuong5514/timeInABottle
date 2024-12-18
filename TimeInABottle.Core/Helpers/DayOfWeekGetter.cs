using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Helpers;
internal class DayOfWeekGetter
{
    public static DayOfWeek GetDayOfWeekThisMonth(int day)
    {
        DateTime dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day);
        return dateTime.DayOfWeek;
    }
}
