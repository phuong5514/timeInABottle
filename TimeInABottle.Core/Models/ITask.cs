using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models;

public abstract class ITask : INotifyPropertyChanged
{
    protected ITask(string name, string description, Time start, Time end)
    {
        Name = name;
        Description = description;
        Start = start;
        End = end;
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public Time Start { get; set; }
    public Time End { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    public abstract string ToString();
}
