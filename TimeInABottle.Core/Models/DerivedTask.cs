using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models;
public class DerivedTask : ITask
{
    public DerivedTask(string name, string description, Time start, Time end) : base(name, description, start, end)
    {
    }

    public override string ToString() => throw new NotImplementedException();
}
