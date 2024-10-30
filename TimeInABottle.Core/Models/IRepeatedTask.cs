using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models;
public abstract class IRepeatedTask : ITask
{
    protected IRepeatedTask(string name, string description, TimeOnly startingTime, TimeOnly endingTime) : base(name, description, startingTime, endingTime)    
    {
    }

    public override string ToString() => "RepeatedTask";
}
