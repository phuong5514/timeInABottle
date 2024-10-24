using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Core.Models;

namespace TimeInABottle.Core.Helpers;
internal class TaskListSorter
{
    // selection sort
    public void SortByTimeAscending(List<ITask> list)
    {
        for (var i = 0; i < list.Count; i++)
        {
            for (var j = i + 1; j < list.Count; j++)
            {
                var taskA = list[i];
                var taskATime = taskA.Start;
                var taskB = list[j];
                var taskBTime = taskB.Start;

                if (taskATime > taskBTime)
                {
                    var temp = list[i];
                    list[i] = list[j];
                    list[j] = temp;
                }
            }
        }
    }

    public void SortByTimeDescending(List<ITask> list)
    {
        for (var i = 0; i < list.Count; i++)
        {
            for (var j = i + 1; j < list.Count; j++)
            {
                var taskA = list[i];
                var taskATime = taskA.Start;
                var taskB = list[j];
                var taskBTime = taskB.Start;

                if (taskATime < taskBTime)
                {
                    var temp = list[i];
                    list[i] = list[j];
                    list[j] = temp;
                }
            }
        }
    }

    //public void SortByDay
}
