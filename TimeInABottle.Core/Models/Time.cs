using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models;
public class Time
{
    public Time(int hours, int minutes)
    {
        if (hours < 0 || hours > 23) {
            throw new ArgumentException("hour must be in range of 0-23");
        }

        if (minutes < 0 || minutes > 59) { 
            throw new ArgumentException("minute must be in range of 0-59");
        }

        Hours = hours;
        Minutes = minutes;
    }

    public Time() {
        DateTime localNow = DateTime.Now;
        Hours = localNow.Hour;
        Minutes = localNow.Minute;
    }

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
            "{0:00}:{1:00}",
            Hours, Minutes);
    }
}