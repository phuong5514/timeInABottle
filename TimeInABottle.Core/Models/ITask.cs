using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models;

public abstract class ITask : INotifyPropertyChanged
{
    protected ITask(string name, string description, TimeOnly start, TimeOnly end)
    {
        Name = name;
        Description = description;
        Start = start;
        End = end;
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public TimeOnly Start { get; set; }
    public TimeOnly End { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    public abstract string ToString();
}
