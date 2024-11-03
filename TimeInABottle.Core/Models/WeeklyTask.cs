using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models;
public class WeeklyTask : IRepeatedTask
{
    public WeeklyTask(string name, string description, TimeOnly startingTime, TimeOnly endingTime) : base(name, description, startingTime, endingTime)
    {
    }

    public List<int> WeekDays
    {
        set; get;
    }

    public override string ToString() => "WeeklyTask";
}
