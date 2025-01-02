using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using TimeInABottle.Contracts.ViewModels;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models.Filters;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.ViewModels;

/// <summary>
/// ViewModel for managing and displaying a list of tasks with filtering options.
/// </summary>
public partial class TaskListViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDaoService _daoService;

    /// <summary>
    /// List of available filter options.
    /// </summary>
    public List<IFilter> FilterOptions
    {
        get;
        set;
    }

    /// <summary>
    /// Currently selected filter option.
    /// </summary>
    public IFilter SelectedFilterOption
    {
        get;
        set;
    }

    /// <summary>
    /// Indicates whether the filter parameter input should be visible.
    /// </summary>
    public bool IsFilterParameterVisible => SelectedFilterOption is IValueFilter;

    /// <summary>
    /// Parameter for the selected filter.
    /// </summary>
    public string FilterParameter
    {
        get;
        set;
    }

    /// <summary>
    /// Collection of tasks to be displayed.
    /// </summary>
    public FullObservableCollection<ITask> Tasks
    {
        get; private set;
    }

    /// <summary>
    /// Collection of currently applied filters.
    /// </summary>
    public ObservableCollection<IFilter> DisplayedFilters { get; } = new ObservableCollection<IFilter>();

    private readonly CompositeFilter _filter = new();

    private bool _isInvertOrder = false;


    private DispatcherTimer _timer;
    private void StartTimer()
    {
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(30) // TODO: config file / setting page options
        };
        _timer.Tick += reloadTasks;
        _timer.Start();
    }


    private void reloadTasks(object sender, object e)
    {
        LoadTask();
    }



    /// <summary>
    /// Command to add a filter.
    /// </summary>
    public ICommand AddFilterCommand
    {
        get;
    }

    //public ICommand AddSelectedFilterCommand
    //{
    //    get;
    //}

    /// <summary>
    /// Command to remove a filter.
    /// </summary>
    public ICommand RemoveFilterCommand
    {
        get;
    }

    /// <summary>
    /// Command to switch the order of tasks.
    /// </summary>
    public ICommand SwitchOrderCommand
    {
        get;
    }

    [ObservableProperty]
    private ITask? selected;

    /// <summary>
    /// Initializes a new instance of the TaskListViewModel class.
    /// </summary>
    /// <param name="daoService">The data access service.</param>
    public TaskListViewModel(IDaoService daoService)
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

    /// <summary>
    /// Sets the available filter options.
    /// </summary>
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

    /// <summary>
    /// Resets the selected filter option and parameter.
    /// </summary>
    public void resetFilterChoice()
    {
        SelectedFilterOption = FilterOptions.First();
        FilterParameter = "";
    }

    /// <summary>
    /// Switches the order of tasks.
    /// </summary>
    /// <param name="value">True to invert the order, false otherwise.</param>
    private void SwitchOrder(bool value)
    {
        if (_isInvertOrder != value)
        {
            _isInvertOrder = value;
            LoadTask();
        }
    }

    /// <summary>
    /// Adds a filter to the collection of applied filters.
    /// </summary>
    /// <param name="filter">The filter to add.</param>
    private void AddFilter(IFilter filter)
    {
        if (filter == null)
        {
            //filter = SelectedFilterOption;
            filter = FilterFactory.CreateFilter(SelectedFilterOption.GetType().Name);
            if (filter is IValueFilter valueFilter)
            {
                valueFilter.Criteria = FilterParameter;
            }

        }
        var success = _filter.AddFilter(filter);
        if (success)
        {
            DisplayedFilters.Add(filter);
        }

        // TODO: somehow notify from the Filter so that this can get removed
        LoadTask();
    }

    /// <summary>
    /// Removes a filter from the collection of applied filters.
    /// </summary>
    /// <param name="filter">The filter to remove.</param>
    private void RemoveFilter(IFilter filter)
    {
        _filter.RemoveFilter(filter);
        DisplayedFilters.Remove(filter);

        // TODO: somehow notify from the Filter so that this can get removed
        LoadTask();
    }

    /// <summary>
    /// Adds tasks to the collection of displayed tasks.
    /// </summary>
    /// <param name="newTasks">The tasks to add.</param>
    private void AddTasks(IEnumerable<ITask> newTasks)
    {
        Tasks.Clear();
        foreach (var task in newTasks)
        {
            Tasks.Add(task);
        }
    }

    /// <summary>
    /// Loads tasks based on the applied filters and order.
    /// </summary>
    public void LoadTask()
    {
        FullObservableCollection<ITask> newTasks;
        if (_daoService is IDaoService queryDao)
        {
            newTasks = queryDao.CustomQuery(_filter, !_isInvertOrder);
        }
        else {
            newTasks = _daoService.GetAllTasks();
        }
        AddTasks(newTasks);
    }

    /// <summary>
    /// Called when the view is navigated to.
    /// </summary>
    /// <param name="parameter">The navigation parameter.</param>
    public async void OnNavigatedTo(object parameter)
    {
        LoadTask();
        StartTimer();
    }

    /// <summary>
    /// Called when the view is navigated from.
    /// </summary>
    public void OnNavigatedFrom()
    {

    }

    /// <summary>
    /// Ensures that a task is selected.
    /// </summary>
    public void EnsureItemSelected()
    {
        try
        {
            Selected ??= Tasks.First();
        }
        catch (InvalidOperationException)
        {
            Selected = null;
        }
    }
}
