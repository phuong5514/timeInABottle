using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using TimeInABottle.Contracts.ViewModels;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models;
using TimeInABottle.Core.Models.Filters;

namespace TimeInABottle.ViewModels;

public partial class TaskListViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDaoService _daoService;

    public FullObservableCollection<ITask> Tasks { get; private set; }
    public ObservableCollection<IFilter> DisplayedFilters { get; } = new ObservableCollection<IFilter>();

    private readonly CompositeFilter _filter = new();
    

    private bool _isInvertOrder = false;

    public ICommand AddFilterCommand
    {
        get;
    }
    public ICommand RemoveFilterCommand
    {
        get;
    }

    public ICommand SwitchOrderCommand
    {
        get;
    }

    [ObservableProperty]
    private ITask? selected;

    public TaskListViewModel( IDaoService daoService)
    {
        AddFilterCommand = new RelayCommand<IFilter>(AddFilter);
        RemoveFilterCommand = new RelayCommand<IFilter>(RemoveFilter);
        SwitchOrderCommand = new RelayCommand<bool>(SwitchOrder);
        _daoService = daoService;
        Tasks = new FullObservableCollection<ITask>();
    }

    private void SwitchOrder(bool value)
    {
        if (_isInvertOrder != value)
        {
            _isInvertOrder = value;
            LoadTask();
        }
    }

    private void AddFilter(IFilter filter)
    {
        var success = _filter.AddFilter(filter);
        if (success) { 
            DisplayedFilters.Add(filter);
        }
        LoadTask();
    }

    private void RemoveFilter(IFilter filter)
    {
        _filter.RemoveFilter(filter);
        DisplayedFilters.Remove(filter);
        LoadTask();
    }

    private void AddTasks(IEnumerable<ITask> newTasks)
    {
        Tasks.Clear();
        foreach (var task in newTasks)
        {
            Tasks.Add(task);
        }
    }

    private void LoadTask()
    {
        if (_daoService is IDaoQueryService DaoService)
        {
            var newTasks = DaoService.CustomQuery(_filter, !_isInvertOrder);
            AddTasks(newTasks);
        }
        else
        {
            var allTasks = _daoService.GetAllTasks();
            AddTasks(allTasks);
        }
    }

    public async void OnNavigatedTo(object parameter)
    {
        LoadTask();
    }

    public void OnNavigatedFrom()
    {

    }

    public void EnsureItemSelected()
    {
        Selected ??= Tasks.First();
    }
}
