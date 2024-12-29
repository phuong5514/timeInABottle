using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;
using TimeInABottle.Core.Models.Tasks;
using TimeInABottle.Core.Services;
using TimeInABottle.Models;
using TimeInABottle.ViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace TimeInABottle.Views;

/// <summary>
/// Represents the dashboard page of the application.
/// </summary>
public sealed partial class DashboardPage : Page
{
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
        //ViewModel.Innit();
        InitializeComponent();

        SetGrid();
        SetTitles();
        LoadData();

    }

    /// <summary>
    /// Loads the data for the dashboard page.
    /// </summary>
    private void LoadData()
    {
        if (ViewModel == null) return;

        var thisWeekTasks = ViewModel.ThisWeekTasks;
        foreach (var task in thisWeekTasks)
        {
            var weekdays = task.GetWeekdaysInt();
            foreach (var weekday in weekdays)
            {
                var position = weekday;
                if (position <= 0)
                {
                    position += 7;
                }
                var weeklyEvent = CreateTaskGrid(task);
                Grid.SetColumn((FrameworkElement)weeklyEvent, position); // Convert weekday to column index
                CalendarContainer.Children.Add(weeklyEvent);
            }
        }
    }

    private void ClearData()
    {
        var template = (DataTemplate)Resources["CalendarTaskItem"];
        if (template == null)
        {
            throw new InvalidOperationException("DataTemplate 'CalendarTaskItem' not found in resources.");
        }

        var content = template.LoadContent();
        var contentType = content.GetType();

        var childrenToRemove = CalendarContainer.Children
            .OfType<FrameworkElement>()
            .Where(child => child.GetType() == contentType)
            .ToList();

        foreach (var child in childrenToRemove)
        {
            CalendarContainer.Children.Remove(child);
        }
    }

    /// <summary>
    /// Creates a grid for a task.
    /// </summary>
    /// <param name="task">The task to create a grid for.</param>
    /// <returns>A grid representing the task.</returns>
    private UIElement CreateTaskGrid(ITask task)
    {
        // Retrieve the DataTemplate
        var template = (DataTemplate)Resources["CalendarTaskItem"];
        if (template == null)
        {
            throw new InvalidOperationException("DataTemplate 'CalendarTaskItem' not found in resources.");
        }

        // Load the template content
        var content = (FrameworkElement)template.LoadContent();
        
        // Set the data context to bind the task
        content.DataContext = task;

        // Retrieve the appropriate row and row span
        var row = CalculateRow(task.Start);
        var rowSpan = CalculateRowSpan(task.Start, task.End);

        // Apply grid row and row span properties
        Grid.SetRow(content, row);
        Grid.SetRowSpan(content, rowSpan);

        return content;
    }

    /// <summary>
    /// Calculates the row index based on the time.
    /// </summary>
    /// <param name="time">The time to calculate the row for.</param>
    /// <returns>The row index.</returns>
    private int CalculateRow(TimeOnly time) => 1 + (time.Minute / 30) + (time.Hour * 2);

    /// <summary>
    /// Calculates the row span based on the start and end times.
    /// </summary>
    /// <param name="start">The start time.</param>
    /// <param name="end">The end time.</param>
    /// <returns>The row span.</returns>
    private int CalculateRowSpan(TimeOnly start, TimeOnly end)
    {
        int startRow = CalculateRow(start);
        int endRow = CalculateRow(end);
        return endRow - startRow;
    }

    /// <summary>
    /// Sets the titles for the calendar grid.
    /// </summary>
    private void SetTitles()
    {
        // Add columns (1 for time labels, the rest for days)
        CalendarContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) }); // Time Column
        for (var i = 0; i < 7; i++) // For Monday to Sunday
        {
            CalendarContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });
        }

        // Add rows (48 rows for 30-minute intervals over 24 hours)
        for (var i = 0; i <= 48; i++) // 30-minute intervals
        {
            CalendarContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
        }

        var titles = new[] { "Time", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        var today = DateTime.Now;
        var dayIterator = today.AddDays(-(int)today.DayOfWeek + 1);
        if (today.DayOfWeek == DayOfWeek.Sunday)
        {
            dayIterator = dayIterator.AddDays(-7);
        }

        for (var i = 0; i < titles.Length; i++)
        {
            string? title;
            if (i != 0)
            {
                var formatedDayString = dayIterator.ToString("M");
                title = $"{titles[i]}\n{formatedDayString}";
                dayIterator = dayIterator.AddDays(1);
            }
            else {
                title = titles[i];
            }

            var columnTitle = new TextBlock
            {
                Text = title,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };

            Grid.SetRow(columnTitle, 0);
            Grid.SetColumn(columnTitle, i);


            CalendarContainer.Children.Add(columnTitle);

        }

        // Add time labels in the first column (0)
        for (var i = 0; i < 24; i++) // 24 hours (two blocks per hour)
        {
            TextBlock timeLabel = new TextBlock
            {
                Text = $"{(i % 24)}:00",
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            var rowIndex = (i * 2) + 1;
            Grid.SetRow(timeLabel, rowIndex); // First half of the hour
            Grid.SetColumn(timeLabel, 0);
            CalendarContainer.Children.Add(timeLabel);
        }
    }

    /// <summary>
    /// Sets the grid for the calendar.
    /// </summary>
    private void SetGrid()
    {
        var columns = 8;
        var rows = 49;
        for (var i = 0; i < columns; i++)
        {

            for (var j = 0; j < rows; j++)
            {
                Border emptyCell = new Border
                {
                    Style = GetStyle("Cell")
                };
                Grid.SetColumn(emptyCell, i);
                Grid.SetRow(emptyCell, j);
                CalendarContainer.Children.Add(emptyCell);
            }

        }
    }

    /// <summary>
    /// Gets the style from the application resources.
    /// </summary>
    /// <param name="key">The key of the style.</param>
    /// <returns>The style.</returns>
    private static Style GetStyle(string key)
    {
        return (Style)Microsoft.UI.Xaml.Application.Current.Resources[key];
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
            //Canvas.SetZIndex(SideBar, 10);
            //set ZIndex so that the bar is over other component
            ColumnDefinitionSideBar.Width = new GridLength(3, GridUnitType.Star);
        }
        else
        {
            // Hide the sidebar
            SideBar.Visibility = Visibility.Collapsed;
            //Canvas.SetZIndex(SideBar, 0);
            ColumnDefinitionSideBar.Width = new GridLength(0);
        }
    }

    private void CalendarItemEdit_Click(object sender, RoutedEventArgs e)
    {
        var task = (ITask)((FrameworkElement)sender).DataContext;
        _ = CreateEditDialog(task);
    }

    private void CalendarItemDelete_Click(object sender, RoutedEventArgs e)
    {
        var task = (ITask)((FrameworkElement)sender).DataContext;
        _ = CreateDeleteConfirmationDialog(task);
    }

    private async Task CreateEditDialog(ITask selectedTask)
    {
        if (selectedTask == null)
        {
            return;
        }

        var dialogViewModel = App.GetService<CUDDialogViewModel>();
        dialogViewModel.EditMode(selectedTask);

        var dialogContent = new TaskEditorDialogControl();
        dialogContent.ViewModel = dialogViewModel;

        var dialog = new ContentDialog
        {
            Title = "Edit Task",
            Content = dialogContent,
            PrimaryButtonText = "Save changes",
            CloseButtonText = "Cancel",
            XamlRoot = Content.XamlRoot, // Ensure the dialog is shown in the correct XAML root
            DataContext = dialogViewModel
        };
        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            var code = dialogViewModel.SaveChanges();
            if (code == FunctionResultCode.SUCCESS)
            {
                ViewModel.LoadData();
                ClearData();
                LoadData();

            }
            else
            {
                _ = CreateFailureDialog(code);
            }
        }
        else
        {
            // left blank
        }
    }


    private async Task CreateDeleteConfirmationDialog(ITask selectedTask)
    {
        var dialogViewModel = App.GetService<CUDDialogViewModel>();
        dialogViewModel.EditMode(selectedTask);

        var dialog = new ContentDialog
        {
            Title = "Delete Task",
            Content = new UserControl()
            {
                Content = new TextBlock() { Text = $"Are you sure you want to delete {selectedTask.Name}?" }
            },
            PrimaryButtonText = "Delete",
            CloseButtonText = "Cancel",
            XamlRoot = Content.XamlRoot, // Ensure the dialog is shown in the correct XAML root
            DataContext = dialogViewModel
        };
        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            if (dialogViewModel.DeleteTask())
            {
                ViewModel.LoadData();
                ClearData();
                LoadData();
            }
        }
        else
        {
            // left blank
        }
    }

    private async Task CreateFailureDialog(FunctionResultCode code)
    {
        var message = "";
        switch (code)
        {
            case FunctionResultCode.ERROR:
                message = "An error occurred";
                break;

            case FunctionResultCode.ERROR_INVALID_INPUT:
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("Invalid input, make sure to check your input!");
                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("Commonly occured scenarios:");
                stringBuilder.AppendLine("1. Start time is after or is the same as end time");
                stringBuilder.AppendLine("2. there is already a task occupied that time");
                message = stringBuilder.ToString();
                break;

            case FunctionResultCode.ERROR_MISSING_INPUT:
                message = "Missing input(s), make sure to fill all the required fields!";
                break;

            case FunctionResultCode.ERROR_UNKNOWN:
                message = "An unknown error occurred";
                break;
        }

        var dialog = new ContentDialog
        {
            Title = "Error",
            Content = new UserControl() { Content = new TextBlock() { Text = message } },
            CloseButtonText = "Ok",
            XamlRoot = this.Content.XamlRoot, // Ensure the dialog is shown in the correct XAML root
        };
        _ = await dialog.ShowAsync();
        //if (result == ContentDialogResult.Primary)
        //{
        //    // left blank
        //}
        //else
        //{
        //    // left blank
        //}
    }

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
            XamlRoot = Content.XamlRoot, // Ensure the dialog is shown in the correct XAML root
            DataContext = dialogViewModel
        };

        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            var code = dialogViewModel.SaveChanges();
            if (code == Models.FunctionResultCode.SUCCESS)
            {
                // tell the view model that data is changed
                ViewModel.LoadData();
                ClearData();
                LoadData();
            }
            else
            {
                _ = CreateFailureDialog(code);
            };

        }
        else
        {
            // left blank
        }
    }

    private void CalendarContainer_RightTapped(object sender, Microsoft.UI.Xaml.Input.RightTappedRoutedEventArgs e)
    {
        var flyout = CalendarContainer.Resources["AddTaskFlyout"] as MenuFlyout;

        // Show the flyout at the pointer location
        flyout?.ShowAt(sender as UIElement, e.GetPosition(sender as UIElement));
    }

    private void AddTaskFlyoutItem_Click(object sender, RoutedEventArgs e)
    {
        _ = CreateAddDialog();
    }
}
