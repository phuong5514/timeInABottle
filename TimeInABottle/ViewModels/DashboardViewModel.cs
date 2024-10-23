using CommunityToolkit.Mvvm.ComponentModel;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models;
using TimeInABottle.Core.Services;
namespace TimeInABottle.ViewModels;

public partial class DashboardViewModel : ObservableRecipient
{
    private readonly IDaoService? _dao = null;

    public Time time;

    private FullObservableCollection<ITask> Tasks
    {
        set; get;
    }

    public void Innit(IDaoService _dao)
    {
        _dao = new MockDaoService();
        UpdateTime();
        getAllTasks();
    }

    private void getAllTasks() {
        if (_dao == null) { 
            return;
        }
        var tasks = _dao.GetAllTasks();
        Tasks = tasks;
    }

    private void UpdateTime() => time = new Time();


    public DashboardViewModel()
    {
        UpdateTime();
    }
}
