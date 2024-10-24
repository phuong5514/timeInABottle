using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models;
public class MonthlyTask : IRepeatedTask
{
    // sẽ xử lý trường hợp ngày trong tháng này không tồn tại trong tháng sau
    // vd: tháng 1 ngày 30 -> tháng 2 ngày 28/29
    public int Date
    {
        get; set;
    }

    MonthlyTask(string name, string description, TimeOnly startingTime, TimeOnly endingTime, int date) : base(name, description, startingTime, endingTime)
    {
        Date = date;
    }

    public override string ToString() => "MonthlyTask";
}
