using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using TimeInABottle.Core.Models.Tasks;
using TimeInABottle.Models;
using TimeInABottle.ViewModels;

namespace TimeInABottle.Views;

public sealed partial class SchedularPage : Page
{

    public SchedularViewModel ViewModel
    {
        get;
    }

    public SchedularPage()
    {
        ViewModel = App.GetService<SchedularViewModel>();
        InitializeComponent();
    }
    public void CalendarItem_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var button = (Button)sender;
            var task = (ITask)button.DataContext;
            ViewModel.AddTaskForSchedulingCommand.Execute(task);
            //ViewModel.AddTaskForScheduling(task);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
        }
    }


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

    public void ScheduleButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            //ViewModel.ScheduleSelectedTaskCommand.Execute();
            ViewModel.ScheduleSelectedTaskExecute();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
        }
    }
}
