
namespace TimeInABottle.Core.Models.Tasks;
public class JsonTask
{
    public string Name
    {
        get; set;
    }

    public string Description
    {
        get; set;
    }
    public TimeOnly Start
    {
        get; set;
    }
    public TimeOnly End
    {
        get; set;
    }
    public int Id
    {
        get; set;
    }
    // Non repeated task
    public DateOnly Date
    {
        get; set;
    }
    // Monthly repeated task
    public int DayOfMonth
    {
        get; set;
    }
    // Weekly repeated task
    public List<DayOfWeek> DaysOfWeek
    {
        get; set;
    }

    public bool IsDerived
    {
        get; set;
    }
}
