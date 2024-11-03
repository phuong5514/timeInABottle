using CommunityToolkit.WinUI.UI.Controls;

using Microsoft.UI.Xaml.Controls;

using TimeInABottle.ViewModels;

namespace TimeInABottle.Views;

public sealed partial class TaskListPage : Page
{
    public TaskListViewModel ViewModel
    {
        get;
    }

    public TaskListPage()
    {
        ViewModel = App.GetService<TaskListViewModel>();
        InitializeComponent();
    }

    private void OnViewStateChanged(object sender, ListDetailsViewState e)
    {
        if (e == ListDetailsViewState.Both)
        {
            ViewModel.EnsureItemSelected();
        }
    }
}
