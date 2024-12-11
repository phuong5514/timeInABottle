using CommunityToolkit.WinUI.UI.Controls;

using Microsoft.UI.Xaml.Controls;
using TimeInABottle.Core.Models.Filters;
using TimeInABottle.ViewModels;

namespace TimeInABottle.Views;

/// <summary>
/// Represents the TaskListPage which displays a list of tasks and allows filtering and other operations.
/// </summary>
public sealed partial class TaskListPage : Page
{
    /// <summary>
    /// Gets the ViewModel for the TaskListPage.
    /// </summary>
    public TaskListViewModel ViewModel { get; }

    /// <summary>
    /// Initializes a new instance of the TaskListPage class.
    /// </summary>
    public TaskListPage()
    {
        ViewModel = App.GetService<TaskListViewModel>();
        // set data context for Filter to show
        //DataContext = ViewModel;
        InitializeComponent();
    }

    /// <summary>
    /// Handles the view state change event.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void OnViewStateChanged(object sender, ListDetailsViewState e)
    {
        if (e == ListDetailsViewState.Both)
        {
            ViewModel.EnsureItemSelected();
        }
    }

    /// <summary>
    /// Handles the text changed event of the AutoSuggestBox.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="args">The event arguments.</param>
    private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        // Only get results when it was a user typing, 
        // otherwise assume the value got filled in by TextMemberPath 
        // or the handler for SuggestionChosen.
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            //Set the ItemsSource to be your filtered dataset
            //sender.ItemsSource = dataset;
        }
    }

    /// <summary>
    /// Handles the suggestion chosen event of the AutoSuggestBox.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="args">The event arguments.</param>
    private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        // Set sender.Text. You can use args.SelectedItem to build your text string.
    }

    /// <summary>
    /// Handles the query submitted event of the AutoSuggestBox.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="args">The event arguments.</param>
    private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        if (args.ChosenSuggestion != null)
        {
            // User selected an item from the suggestion list, take an action on it here.
        }
        else
        {
            if (args.QueryText != string.Empty)
            {
                KeywordFilter filter = new KeywordFilter { Criteria = args.QueryText };
                ViewModel.AddFilterCommand.Execute(filter);
                //sender.Text = string.Empty;
            }
        }
    }

    /// <summary>
    /// Handles the click event of the Add button.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void OnAddButtonClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        _ = CreateAddDialog();
    }

    /// <summary>
    /// Handles the click event of the Delete button.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void OnDeleteButtonClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        _ = CreateDeleteConfirmationDialog();
    }

    /// <summary>
    /// Handles the click event of the Change button.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void OnChangeButtonClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        _ = CreateEditDialog();
    }

    /// <summary>
    /// Creates and shows a filter dialog.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task CreateFilterDialog()
    {
        var dialog = new ContentDialog
        {
            Title = "Filters",
            Content = new FilterDialogContent(),
            PrimaryButtonText = "Add",
            CloseButtonText = "Cancel",
            XamlRoot = this.Content.XamlRoot, // Ensure the dialog is shown in the correct XAML root
            DataContext = (TaskListViewModel)ViewModel
        };

        var result = await dialog.ShowAsync(); // went to this yet doesnt show dialog
        if (result == ContentDialogResult.Primary)
        {
            ViewModel.AddFilterCommand.Execute(null);
        }
        else
        {
            // left blank
        }

        ViewModel.resetFilterChoice();
    }

    private async Task CreateAddDialog()
    {

        var dialogViewModel = new CUDDialogViewModel();
        var dialog = new ContentDialog
        {
            Title = "Add Task",
            Content = new AddTaskDialogControl(),
            PrimaryButtonText = "Add",
            CloseButtonText = "Cancel",
            XamlRoot = this.Content.XamlRoot, // Ensure the dialog is shown in the correct XAML root
            DataContext = dialogViewModel
        };

        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            dialogViewModel.SaveChanges();
        }
        else
        {
            // left blank
        }
    }

    private async Task CreateEditDialog()
    {
        if (ViewModel.Selected == null)
        {
            return;
        }
        var dialogViewModel = new CUDDialogViewModel(ViewModel.Selected);
        var dialog = new ContentDialog
        {
            Title = "Edit Task",
            Content = new EditTaskDialogControl(),
            PrimaryButtonText = "Edit",
            CloseButtonText = "Cancel",
            XamlRoot = this.Content.XamlRoot, // Ensure the dialog is shown in the correct XAML root
            DataContext = dialogViewModel
        };
        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            dialogViewModel.SaveChanges();
        }
        else
        {
            // left blank
        }
    }

    private async Task CreateDeleteConfirmationDialog()
    {
        //var dialog = new ContentDialog
        //{
        //    Title = "Delete Task",
        //    Content = new DeleteConfirmationDialogControl(),
        //    PrimaryButtonText = "Delete",
        //    CloseButtonText = "Cancel",
        //    XamlRoot = this.Content.XamlRoot, // Ensure the dialog is shown in the correct XAML root
        //    //DataContext = ViewModel
        //};
        //var result = await dialog.ShowAsync();
        //if (result == ContentDialogResult.Primary)
        //{
        //    //ViewModel.DeleteTaskCommand.Execute(null);
        //}
        //else
        //{
        //    // left blank
        //}
    }

    /// <summary>
    /// Handles the item click event of the FilterGrid.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void FilterGrid_ItemClick(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is IFilter selectedFilter)
        {
            ViewModel.RemoveFilterCommand.Execute(selectedFilter);
        }
    }

    /// <summary>
    /// Handles the click event of the Filter button.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void OnFilterButtonClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        _ = CreateFilterDialog();
    }
}

