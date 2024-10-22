using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models;
public abstract class IRepeatedTask : ITask
{
    protected IRepeatedTask(string name, string description, Time startingTime, Time endingTime) : base(name, description, startingTime, endingTime)    
    {
    }

    public override string ToString() => "RepeatedTask";
}
