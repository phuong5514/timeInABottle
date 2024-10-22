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
        //int minuteValue = minute % 60;
        //int extraHour = minute / 60;
        //int hourValue = (hour + extraHour) % 24;

        //Hours = hourValue;
        //Minutes = minuteValue;
        Hours = hours;
        Minutes = minutes;    
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