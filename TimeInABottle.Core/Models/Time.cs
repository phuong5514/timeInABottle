using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models;
// not needed

//public class Time
//{
//    public Time(int hours, int minutes)
//    {
//        if (hours < 0 || hours > 23)
//        {
//            throw new ArgumentException("hour must be in range of 0-23");
//        }

//        if (minutes < 0 || minutes > 59)
//        {
//            throw new ArgumentException("minute must be in range of 0-59");
//        }

//        Hours = hours;
//        Minutes = minutes;
//    }

//    public Time()
//    {
//        DateTime localNow = DateTime.Now;
//        Hours = localNow.Hour;
//        Minutes = localNow.Minute;
//    }

//    public int Hours
//    {
//        get; set;
//    }
//    public int Minutes
//    {
//        get; set;
//    }

//    public override string ToString()
//    {
//        return string.Format(
//            "{0:00}:{1:00}",
//            Hours, Minutes);
//    }


//    public static bool operator <(Time left, Time right)
//    {
//        if (left == null || right == null)
//        {
//            throw new ArgumentNullException("Cannot compare null Time objects.");
//        }

//        if (left.Hours == right.Hours)
//        {
//            return left.Minutes < right.Minutes;
//        }
//        return left.Hours < right.Hours;
//    }

//    public static bool operator >(Time left, Time right)
//    {
//        if (left == null || right == null)
//        {
//            throw new ArgumentNullException("Cannot compare null Time objects.");
//        }

//        if (left.Hours == right.Hours)
//        {
//            return left.Minutes > right.Minutes;
//        }
//        return left.Hours > right.Hours;
//    }

//    // Equality operators
//    public static bool operator ==(Time left, Time right)
//    {
//        if (ReferenceEquals(left, right)) return true;
//        if (left is null || right is null) return false;

//        return left.Hours == right.Hours && left.Minutes == right.Minutes;
//    }

//    public static bool operator !=(Time left, Time right)
//    {
//        return !(left == right);
//    }

//    // Override Equals and GetHashCode
//    public override bool Equals(object obj)
//    {
//        if (obj is Time other)
//        {
//            return this == other;
//        }
//        return false;
//    }

//    public override int GetHashCode()
//    {
//        return HashCode.Combine(Hours, Minutes);
//    }
//}