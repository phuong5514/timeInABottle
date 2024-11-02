using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models;
public class DailyTask : IRepeatedTask
{
    public DailyTask(string name, string description, TimeOnly startingTime, TimeOnly endingTime) : base(name, description, startingTime, endingTime)
    {
    }

    public override string ToString() => $"DailyTask";
}
