using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models;

public interface ITask : INotifyPropertyChanged
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Time Start { get; set; }
    public Time End { get; set; }
}
