using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using TimeInABottle.Core.Models.Tasks;
using TimeInABottle.ViewModels;

namespace TimeInABottle.Views;

/// <summary>
/// Represents the SchedularPage class which is a sealed partial class inheriting from Page.
/// </summary>
public sealed partial class SchedularPage : Page
{
    /// <summary>
    /// Gets the ViewModel for the SchedularPage.
    /// </summary>
    public SchedularViewModel ViewModel
    {
        get;
    }

    /// <summary>
    /// Initializes a new instance of the SchedularPage class.
    /// </summary>
    public SchedularPage()
    {
        ViewModel = App.GetService<SchedularViewModel>();
        InitializeComponent();
    }

    /// <summary>
    /// Handles the click event for the calendar item.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    public void CalendarItem_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var button = (Button)sender;
            var task = (ITask)button.DataContext;
            ViewModel.AddTaskForSchedulingCommand.Execute(task);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
        }
    }

    /// <summary>
    /// Handles the click event for removing a task.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    public void RemoveTask_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var button = (Button)sender;
            var task = button.DataContext;
            ViewModel.RemoveTaskForSchedulingCommand.Execute(task);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
        }
    }

    /// <summary>
    /// Handles the click event for scheduling a task.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    public void ScheduleButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            ViewModel.ScheduleSelectedTaskExecute();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
        }
    }
}
