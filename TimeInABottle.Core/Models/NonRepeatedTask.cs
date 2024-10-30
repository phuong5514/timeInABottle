using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models;
public class NonRepeatedTask : ITask
{
    public NonRepeatedTask(string name, string description, TimeOnly start, TimeOnly end, DateOnly date) : base(name, description, start, end)
    {
        Date = date;
    }


    // TODO: hạn chế chỉ set ngày trong 1 giới hạn: ko thể set trong quá khứ,
    // và khoảng cách từ ngày cài đặt đến ngày thực hiện không quá 3 tháng (tùy chỉnh)
    public DateOnly Date
    {
        get;
        private set;
    }

    public override string ToString() => "NonRepeatedTask";
}
