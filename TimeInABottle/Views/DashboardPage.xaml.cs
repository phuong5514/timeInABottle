using System.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models.Tasks;
using TimeInABottle.Models;
using TimeInABottle.ViewModels;
namespace TimeInABottle.Views;

/// <summary>
/// Represents the dashboard page of the application.
/// </summary>
public sealed partial class DashboardPage : Page
{
    private int _increment;
    private int _frequency;
    private int _rowCount;

    /// <summary>
    /// Reads the configuration values for time slot increment and calculates frequency and row count.
    /// </summary>
    private void ReadConfig()
    {
        var incrementString = ConfigHandler.GetConfigValue("TimeSlotIncrement");
        _increment = int.Parse(incrementString);
        _frequency = 60 / _increment;
        _rowCount = 24 * _frequency;
    }

    /// <summary>
    /// Gets the ViewModel associated with the dashboard page.
    /// </summary>
    public DashboardViewModel ViewModel { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DashboardPage"/> class.
    /// </summary>
    public DashboardPage()
    {
        ViewModel = App.GetService<DashboardViewModel>();
        ReadConfig();
        InitializeComponent();
    }

    /// <summary>
    /// Handles the click event of the toggle button to show or hide the sidebar.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void ToggleButton_Click(object sender, RoutedEventArgs e)
    {
        if (SideBar.Visibility == Visibility.Collapsed)
        {
            // Show the sidebar
            SideBar.Visibility = Visibility.Visible;
            ColumnDefinitionSideBar.Width = new GridLength(3, GridUnitType.Star);
        }
        else
        {
            // Hide the sidebar
            SideBar.Visibility = Visibility.Collapsed;
            ColumnDefinitionSideBar.Width = new GridLength(0);
        }
    }

    /// <summary>
    /// Handles the click event to edit a calendar item.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void CalendarItemEdit_Click(object sender, RoutedEventArgs e)
    {
        var task = (ITask)((FrameworkElement)sender).DataContext;
        _ = CreateEditDialog(task);
    }

    /// <summary>
    /// Handles the click event to delete a calendar item.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void CalendarItemDelete_Click(object sender, RoutedEventArgs e)
    {
        var task = (ITask)((FrameworkElement)sender).DataContext;
        _ = CreateDeleteConfirmationDialog(task);
    }

    /// <summary>
    /// Creates and shows a dialog for editing a task.
    /// </summary>
    /// <param name="selectedTask">The task to be edited.</param>
    private async Task CreateEditDialog(ITask selectedTask)
    {
        if (selectedTask == null)
        {
            return;
        }

        var dialogViewModel = App.GetService<CUDDialogViewModel>();
        dialogViewModel.EditMode(selectedTask);

        var dialogContent = new TaskEditorDialogControl
        {
            ViewModel = dialogViewModel
        };

        var dialog = new ContentDialog
        {
            Title = "Edit Task",
            Content = dialogContent,
            PrimaryButtonText = "Save changes",
            CloseButtonText = "Cancel",
            XamlRoot = Content.XamlRoot,
            DataContext = dialogViewModel
        };

        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            var code = dialogViewModel.SaveChanges();
            if (code == FunctionResultCode.SUCCESS)
            {
                ViewModel.LoadData();
            }
            else
            {
                _ = CreateFailureDialog(code);
            }
        }
    }

    /// <summary>
    /// Creates and shows a confirmation dialog for deleting a task.
    /// </summary>
    /// <param name="selectedTask">The task to be deleted.</param>
    private async Task CreateDeleteConfirmationDialog(ITask selectedTask)
    {
        var dialogViewModel = App.GetService<CUDDialogViewModel>();
        dialogViewModel.EditMode(selectedTask);

        var dialog = new ContentDialog
        {
            Title = "Delete Task",
            Content = new UserControl
            {
                Content = new TextBlock { Text = $"Are you sure you want to delete {selectedTask.Name}?" }
            },
            PrimaryButtonText = "Delete",
            CloseButtonText = "Cancel",
            XamlRoot = Content.XamlRoot,
            DataContext = dialogViewModel
        };

        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            if (dialogViewModel.DeleteTask())
            {
                ViewModel.LoadData();
            }
        }
    }

    /// <summary>
    /// Creates and shows a dialog indicating a failure with a specific error code.
    /// </summary>
    /// <param name="code">The error code indicating the type of failure.</param>
    private async Task CreateFailureDialog(FunctionResultCode code)
    {
        var message = code switch
        {
            FunctionResultCode.ERROR => "An error occurred",
            FunctionResultCode.ERROR_INVALID_INPUT => new StringBuilder()
                .AppendLine("Invalid input, make sure to check your input!")
                .AppendLine("")
                .AppendLine("Commonly occurred scenarios:")
                .AppendLine("1. Start time is after or is the same as end time")
                .AppendLine("2. There is already a task occupying that time")
                .ToString(),
            FunctionResultCode.ERROR_MISSING_INPUT => "Missing input(s), make sure to fill all the required fields!",
            FunctionResultCode.ERROR_UNKNOWN => "An unknown error occurred",
            _ => "An unexpected error occurred"
        };

        var dialog = new ContentDialog
        {
            Title = "Error",
            Content = new UserControl { Content = new TextBlock { Text = message } },
            CloseButtonText = "Ok",
            XamlRoot = Content.XamlRoot
        };

        _ = await dialog.ShowAsync();
    }

    /// <summary>
    /// Creates and shows a dialog for adding a new task.
    /// </summary>
    private async Task CreateAddDialog()
    {
        var dialogViewModel = App.GetService<CUDDialogViewModel>();

        var dialogContent = new TaskEditorDialogControl
        {
            ViewModel = dialogViewModel
        };

        var dialog = new ContentDialog
        {
            Title = "Add Task",
            Content = dialogContent,
            PrimaryButtonText = "Add",
            CloseButtonText = "Cancel",
            XamlRoot = Content.XamlRoot,
            DataContext = dialogViewModel
        };

        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            var code = dialogViewModel.SaveChanges();
            if (code == FunctionResultCode.SUCCESS)
            {
                ViewModel.LoadData();
            }
            else
            {
                _ = CreateFailureDialog(code);
            }
        }
    }

    /// <summary>
    /// Handles the right-tap event on the calendar container to show the add task flyout.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void CalendarContainer_RightTapped(object sender, Microsoft.UI.Xaml.Input.RightTappedRoutedEventArgs e)
    {
        var flyout = CalendarContainer.Resources["AddTaskFlyout"] as MenuFlyout;
        // Show the flyout at the pointer location
        flyout?.ShowAt(sender as UIElement, e.GetPosition(sender as UIElement));
    }

    /// <summary>
    /// Handles the click event of the add task flyout item to create a new task.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void AddTaskFlyoutItem_Click(object sender, RoutedEventArgs e)
    {
        _ = CreateAddDialog();
    }
}
