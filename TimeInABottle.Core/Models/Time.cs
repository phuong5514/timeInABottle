using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models;
internal class Time
{
    public int Hours
    {
        get; set;
    }
    public int Minutes
    {
        get; set;
    }

    public override string ToString()
    {
        return string.Format(
            "{0:00}:{1:00}:{2:00}",
            this.Hours, this.Minutes);
    }
}