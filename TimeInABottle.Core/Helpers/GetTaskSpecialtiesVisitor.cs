using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Core.Helpers;
public class GetTaskSpecialtiesVisitor
{
    public object visitTask(ITask task)
    {
        return task.Accept(this);
    }

    public List<Values.Weekdays> VisitWeeklyTask(WeeklyTask task)
    {
        return task.WeekDays;
    }

    public int VisitMonthlyTask(MonthlyTask task)
    {
        return task.Date;
    }

    public DateOnly VisitNonRepeatedTask(NonRepeatedTask task)
    {
        return task.Date;
    }
}
