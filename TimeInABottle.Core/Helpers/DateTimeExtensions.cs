using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Helpers;
//https://stackoverflow.com/questions/38039/how-can-i-get-the-datetime-for-the-start-of-the-week

/// <summary>
/// Provides extension methods for the DateTime structure.
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// Returns the DateTime representing the start of the week for the given DateTime.
    /// </summary>
    /// <param name="dt">The DateTime to calculate the start of the week for.</param>
    /// <param name="startOfWeek">The day of the week to consider as the start of the week.</param>
    /// <returns>A DateTime representing the start of the week.</returns>
    public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
    {
        var diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
        return dt.AddDays(-1 * diff).Date;
    }
}
