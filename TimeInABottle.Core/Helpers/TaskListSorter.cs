using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Core.Helpers;
/// <summary>
/// Provides methods to sort a list of tasks by their start time.
/// </summary>
internal class TaskListSorter
{
    /// <summary>
    /// Sorts the list of tasks in ascending order by their start time using selection sort.
    /// </summary>
    /// <param name="list">The list of tasks to sort.</param>
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

    /// <summary>
    /// Sorts the list of tasks in descending order by their start time using selection sort.
    /// </summary>
    /// <param name="list">The list of tasks to sort.</param>
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
}
