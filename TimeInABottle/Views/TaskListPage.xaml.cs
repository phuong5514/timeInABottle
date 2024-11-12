using CommunityToolkit.WinUI.UI.Controls;

using Microsoft.UI.Xaml.Controls;
using TimeInABottle.Core.Models.Filters;
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
        // set data context for Filter to show
        //DataContext = ViewModel;
        InitializeComponent();
    }

    private void OnViewStateChanged(object sender, ListDetailsViewState e)
    {
        if (e == ListDetailsViewState.Both)
        {
            ViewModel.EnsureItemSelected();
        }
    }

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


    private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        // Set sender.Text. You can use args.SelectedItem to build your text string.
    }


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

    private void OnAddButtonClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {

    }

    private void OnDeleteButtonClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {

    }

    private void OnChangeButtonClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {

    }

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
    }

    private void FilterGrid_ItemClick(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is IFilter selectedFilter)
        {
            ViewModel.RemoveFilterCommand.Execute(selectedFilter);
        }
    }

    private void OnFilterButtonClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
        _ = CreateFilterDialog();
    }



    //private void OnFilterRemoveClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    //{

    //}
}

