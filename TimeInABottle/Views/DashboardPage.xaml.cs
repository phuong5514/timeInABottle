using System.Drawing;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using TimeInABottle.Core.Models;
using TimeInABottle.Core.Services;
using TimeInABottle.ViewModels;

namespace TimeInABottle.Views;

public sealed partial class DashboardPage : Page
{

    public DashboardViewModel ViewModel
    {
        get;
    }

    public DashboardPage()
    {
        ViewModel = App.GetService<DashboardViewModel>();
        //ViewModel.Innit();
        InitializeComponent();


        SetGrid();
        SetTitles();
        LoadData();

    }

    private void LoadData()
    {
        if (ViewModel == null) return;

        var thisWeekTasks = ViewModel.ThisWeekTasks;
        foreach (var task in thisWeekTasks)
        {
            if (task is DailyTask)
            {
                // Daily Task: Create a grid cell for each day
                for (var day = 0; day < 7; day++)
                {
                    var dayEvent = CreateTaskGrid(task);
                    Grid.SetColumn(dayEvent, day); // Set column based on the day
                    CalendarContainer.Children.Add(dayEvent);
                }
            }
            else if (task is WeeklyTask weeklyTask)
            {
                // Weekly Task: Create grids only for specified weekdays
                foreach (var day in weeklyTask.WeekDays)
                {
                    var weeklyEvent = CreateTaskGrid(task);
                    Grid.SetColumn(weeklyEvent, (int)day); // Convert weekday to column index
                    CalendarContainer.Children.Add(weeklyEvent);
                }
            }
            else if (task is MonthlyTask monthlyTask)
            {
                var date = monthlyTask.Date;
                var dayOfWeek = (int)new DateTime(DateTime.Now.Year, DateTime.Now.Month, date).DayOfWeek;
                dayOfWeek = dayOfWeek == 0 ? 7 : dayOfWeek;

                var monthlyEvent = CreateTaskGrid(task);

                Grid.SetColumn(monthlyEvent, dayOfWeek);
                CalendarContainer.Children.Add(monthlyEvent);
            }
            else if (task is NonRepeatedTask nonRepeatedTask) {
                DateOnly date = nonRepeatedTask.Date;
                DateTime eventDate = new DateTime(date.Year, date.Month, date.Day);

                var dayOfWeek = (int)eventDate.DayOfWeek;
                dayOfWeek = dayOfWeek == 0 ? 7 : dayOfWeek;

                var nonRepeatedEvent = CreateTaskGrid(task);

                Grid.SetColumn(nonRepeatedEvent, dayOfWeek);
                CalendarContainer.Children.Add(nonRepeatedEvent);
            }
        }
    }

    // should be a service, this is for milestone 1 only
    private Grid CreateTaskGrid(ITask task)
    {
        var eventGrid = new Grid
        {
            Style = (Style)Application.Current.Resources["CellContent"],
            Background = new SolidColorBrush(Colors.Blue), // Customize as needed
        };
        var row = CalculateRow(task.Start);
        var rowSpan = CalculateRowSpan(task.Start, task.End);

        Grid.SetRow(eventGrid, row);
        Grid.SetRowSpan(eventGrid, rowSpan);

        return eventGrid;
    }

    private int CalculateRow(TimeOnly time) => 1 + (time.Minute / 30) + (time.Hour * 2);

    private int CalculateRowSpan(TimeOnly start, TimeOnly end)
    {
        int startRow = CalculateRow(start);
        int endRow = CalculateRow(end);
        return endRow - startRow;
    }

    //

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
            CalendarContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(36) });
        }

        var titles = new[] { "Time", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        for (var i = 0; i < titles.Length; i++)
        {
            var title = titles[i];
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

    private static Style GetStyle(String key)
    {
        return (Style)Application.Current.Resources[key];
    }

    private void DebugButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.ShowNextTaskNotification(); 
    }

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
}
