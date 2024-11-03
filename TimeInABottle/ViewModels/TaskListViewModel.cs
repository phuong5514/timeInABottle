using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using TimeInABottle.Contracts.ViewModels;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models;

namespace TimeInABottle.ViewModels;

public partial class TaskListViewModel : ObservableRecipient, INavigationAware
{
    //private readonly ISampleDataService _sampleDataService;
    private IDaoService _daoService;
    public FullObservableCollection<ITask> Tasks { get; private set; }

    [ObservableProperty]
    private ITask? selected;

    //public ObservableCollection<SampleOrder> SampleItems { get; private set; } = new ObservableCollection<SampleOrder>();

    //public TaskListViewModel(ISampleDataService sampleDataService)
    //{
    //    _sampleDataService = sampleDataService;
    //}

    public TaskListViewModel(IDaoService daoService)
    {
        _daoService = daoService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        //SampleItems.Clear();
        Tasks = _daoService.GetAllTasks();

        //foreach (var item in data)
        //{
        //    SampleItems.Add(item);
        //}
    }

    public void OnNavigatedFrom()
    {
    }

    public void EnsureItemSelected()
    {
        Selected ??= Tasks.First();
    }
}
