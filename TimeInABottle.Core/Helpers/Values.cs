using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Helpers;
internal class Values
{
    public enum Weekdays
    {
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday
    }

    public enum Months
    {
        January = 1,  // Start from 1 for better readability (1-based index)
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }

    public static readonly int[] DAYS_COUNT_IN_MONTHS =
    {
        31, // January
        28, // February (29 in leap years)
        31, // March
        30, // April
        31, // May
        30, // June
        31, // July
        31, // August
        30, // September
        31, // October
        30, // November
        31, // December
    };

}
