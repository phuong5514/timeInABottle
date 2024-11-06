using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using TimeInABottle.Contracts.ViewModels;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models;

namespace TimeInABottle.ViewModels;

public partial class TaskListViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDaoService _daoService;

    public FullObservableCollection<ITask> Tasks { get; private set; }

    private Dictionary<string, object> _filterTags = new();

    public Dictionary<string, object> FilterTags
    {
        get => _filterTags;
        set
        {
            _filterTags = value;
            OnPropertyChanged(nameof(FilterTags));  // Notify the view when the dictionary changes
        }
    }

    public ICommand AddFilterCommand
    {
        get;
    }
    public ICommand RemoveFilterCommand
    {
        get;
    }

    [ObservableProperty]
    private ITask? selected;

    public TaskListViewModel( IDaoService daoService)
    {
        AddFilterCommand = new RelayCommand<string>(AddFilter);
        RemoveFilterCommand = new RelayCommand<string>(RemoveFilter);
        _daoService = daoService;
    }

    private void AddFilter(string tag)
    {
        if (!_filterTags.ContainsKey(tag))
        {
            _filterTags.Add(tag, new object());
        }
    }

    private void RemoveFilter(string tag)
    {
        if (_filterTags.ContainsKey(tag))
        {
            _filterTags.Remove(tag);
        }
    }

    public async void OnNavigatedTo(object parameter)
    {
        Tasks = _daoService.GetAllTasks();
    }

    public void OnNavigatedFrom()
    {

    }

    public void EnsureItemSelected()
    {
        Selected ??= Tasks.First();
    }
}
