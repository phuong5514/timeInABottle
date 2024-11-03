using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TimeInABottle.Core.Helpers;


namespace TimeInABottle.Core.Models;
public class WeeklyTask : IRepeatedTask
{

    public WeeklyTask(string name, string description, TimeOnly startingTime, TimeOnly endingTime)
        : base(name, description, startingTime, endingTime)
    {
    }

    public WeeklyTask(string name, string description, TimeOnly startingTime, TimeOnly endingTime, List<Values.Weekdays> weekdays)
        : base(name, description, startingTime, endingTime)  
    {
        WeekDays = weekdays;
    }


    
    public List<Values.Weekdays> WeekDays

    {
        set; get;
    }

    public override string ToString() => "WeeklyTask";
}
