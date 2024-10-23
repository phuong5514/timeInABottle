using CommunityToolkit.Mvvm.ComponentModel;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models;
using TimeInABottle.Core.Services;
namespace TimeInABottle.ViewModels;

public partial class DashboardViewModel : ObservableRecipient
{
    private IDaoService? _dao;

    public Time Time
    {
        get; set;
    }

    public ITask NextTask
    {
        get; set;
    } 

    public FullObservableCollection<ITask> Tasks
    {
        set; get;
    }

    public void Innit()
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

    private void UpdateTime() => Time = new Time();

    //private void 

    public DashboardViewModel()
    {
        Innit();
        UpdateTime();
    }
}
