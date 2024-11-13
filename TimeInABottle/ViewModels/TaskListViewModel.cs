using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
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

    // list of filter options
    public List<IFilter> FilterOptions
    {
        get;
        set;
    } 


    public IFilter SelectedFilterOption
    {
        get;
        set;
    }

    public bool IsFilterParameterVisible => SelectedFilterOption is IValueFilter;

    public string FilterParameter
    {
        get;
        set;
    }

    public FullObservableCollection<ITask> Tasks { get; private set; }
    public ObservableCollection<IFilter> DisplayedFilters { get; } = new ObservableCollection<IFilter>();

    private readonly CompositeFilter _filter = new();
    

    private bool _isInvertOrder = false;

    public ICommand AddFilterCommand
    {
        get;
    }

    //public ICommand AddSelectedFilterCommand
    //{
    //    get;
    //}

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
        //AddSelectedFilterCommand = new RelayCommand(AddFilter);
        RemoveFilterCommand = new RelayCommand<IFilter>(RemoveFilter);
        SwitchOrderCommand = new RelayCommand<bool>(SwitchOrder);
        _daoService = daoService;
        Tasks = new FullObservableCollection<ITask>();



        SetFilterOptions();

        _filter.PropertyChanged += (sender, args) => LoadTask();
    }

    private void SetFilterOptions()
    {
        FilterOptions = new List<IFilter>();
        // get all the filter types from the assembly
        var filterTypes = Assembly.GetAssembly(typeof(IFilter)).GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.GetInterface(nameof(IFilter)) != null);
        foreach (var filterType in filterTypes)
        {
            // skip the composite filter
            if (filterType == typeof(CompositeFilter) || filterType == typeof(KeywordFilter))
            {
                continue;
            }

            // add the filter to the list of filter options
            FilterOptions.Add((IFilter)Activator.CreateInstance(filterType));
            // register the filter types with the filter factory
            FilterFactory.RegisterFilter(filterType.Name, filterType);
        }
    }

    public void resetFilterChoice()
    {
        SelectedFilterOption = FilterOptions.First();
        FilterParameter = "";
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
        if (filter == null) {
            //filter = SelectedFilterOption;
            filter = FilterFactory.CreateFilter(SelectedFilterOption.GetType().Name);
            if (filter is IValueFilter valueFilter)
            {
                valueFilter.Criteria = FilterParameter;
            }
            
        }
        var success = _filter.AddFilter(filter);
        if (success) { 
            DisplayedFilters.Add(filter);
        }

        // TODO: somehow notify from the Filter so that this can get removed
        LoadTask();
    }

    

    private void RemoveFilter(IFilter filter)
    {
        _filter.RemoveFilter(filter);
        DisplayedFilters.Remove(filter);

        // TODO: somehow notify from the Filter so that this can get removed
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
