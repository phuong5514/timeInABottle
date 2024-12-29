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

        SetGrid();
        SetTitles();
        LoadData();

        //ViewModel.SelectedTaskChanged += OnSelectedTaskChanged;
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

    private static Style GetStyle(string key)
    {
        return (Style)Microsoft.UI.Xaml.Application.Current.Resources[key];
    }

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
        var templateString = "CalendarTaskItem";
        var template = (DataTemplate)Resources[templateString];

        if (template == null)
        {
            throw new InvalidOperationException($"DataTemplate '{templateString}' not found in resources.");
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
        var startRow = CalculateRow(start);
        var endRow = CalculateRow(end);
        return endRow - startRow;
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
            ClearData();
            LoadData();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
        }
    }
}
