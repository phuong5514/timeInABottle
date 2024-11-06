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
            // Use args.QueryText to determine what to do.
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

    private void OnFilterButtonClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {

    }

    //private void OnFilterRemoveClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    //{

    //}
}
