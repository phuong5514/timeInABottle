using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Helpers;
/// <summary>
/// The <c>Values</c> class contains enumerations for weekdays and months,
/// as well as a static array representing the number of days in each month.
/// </summary>
public class Values
{
    /// <summary>
    /// Enumeration of the days of the week, starting with Monday as 1.
    /// </summary>
    //public enum Weekdays
    //{
    //    Monday = 1,
    //    Tuesday,
    //    Wednesday,
    //    Thursday,
    //    Friday,
    //    Saturday,
    //    Sunday
    //}

    /// <summary>
    /// Enumeration of the months of the year, starting with January as 1.
    /// </summary>
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

    /// <summary>
    /// Static array representing the number of days in each month.
    /// Note: February has 28 days, 29 in leap years.
    /// </summary>
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
