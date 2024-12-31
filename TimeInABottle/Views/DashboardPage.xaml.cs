using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Text;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;
using TimeInABottle.Core.Helpers;
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

    private int _increment;
    private int _frequency;
    private int _rowCount;

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
        //ViewModel.Innit();
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
                //ClearData();
                //LoadData();

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
                //ClearData();
                //LoadData();
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
                //ClearData();
                //LoadData();
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
