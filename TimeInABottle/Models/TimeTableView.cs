using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Models;
internal class TimeTableView : Grid, INotifyPropertyChanged
{
    private static readonly int _columnCount = 8;
    private readonly int _taskTimeUnit;
    private readonly int _rowCount;

    public event PropertyChangedEventHandler? PropertyChanged;

    public bool TrackTime {
        set; get;
    }

    //private List<ITask> _values;
    //public List<ITask> Values
    //{
    //    get => _values;
    //    set
    //    {
    //        _values = value;
    //        OnPropertyChanged(nameof(Values));
    //        LoadData(); // Reload the grid whenever Values changes
    //    }
    //}

    //private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


    public static readonly DependencyProperty ValuesProperty =
        DependencyProperty.Register(
            nameof(Values),
            typeof(List<ITask>),
            typeof(TimeTableView),
            new PropertyMetadata(new List<ITask>(), OnValuesChanged));

    public List<ITask> Values
    {
        get => (List<ITask>)GetValue(ValuesProperty);
        set => SetValue(ValuesProperty, value);
    }

    private static void OnValuesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TimeTableView view && e.NewValue is List<ITask>)
        {
            view.LoadData(); // Refresh the grid when Values changes
        }
    }

    public TimeTableView(int taskTimeUnit = 15, bool trackTime = false)
    {
        _taskTimeUnit = taskTimeUnit;
        _rowCount = 24 * 60 / taskTimeUnit;
        TrackTime = trackTime;
        Values = new List<ITask>();
        InitializeGrid();
    }

    private void InitializeGrid() { 
        SetGrid();
        SetTitles();
        LoadData();
        if (TrackTime)
        {
            StartGridUpdateTimer();
        }
    }

    private void StartGridUpdateTimer()
    {
        var timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMinutes(5)
        };

        timer.Tick += (s, e) =>
        {
            foreach (var child in Children)
            {
                if (child is Border cell && cell.Tag is DateTime cellTime)
                {
                    UpdateCellStyle(cell, cellTime);
                }
            }
        };

        timer.Start();
    }

    private void SetGrid()
    {
        DateTime startTime = DateTime.MinValue;
        if (TrackTime)
        {
            startTime = DateTime.Now;
            startTime = startTime.AddHours(-startTime.Hour).AddMinutes(-startTime.Minute).AddSeconds(-startTime.Second);
            if (startTime.DayOfWeek == DayOfWeek.Sunday)
            {
                startTime = startTime.AddDays(-7);
            }
            startTime = startTime.AddDays(-(int)startTime.DayOfWeek);
        }
        else
        {
            // nothing
        }
        
        

        for (var i = 0; i < _columnCount; i++)
        {

            for (var j = 0; j <= _rowCount; j++)
            {

                DateTime? cellTime = null;
                if (TrackTime) {
                    if (i > 0 && j > 0)
                    {
                        cellTime = startTime.AddMinutes(j * _taskTimeUnit).AddDays(i);
                    }
                }

                Border emptyCell = new Border
                {
                    Style = (Style)Microsoft.UI.Xaml.Application.Current.Resources["Cell"],
                    Tag = cellTime
                };

                if (cellTime != null)
                {
                    UpdateCellStyle(emptyCell, cellTime);
                }

                Grid.SetColumn(emptyCell, i);
                Grid.SetRow(emptyCell, j);
                Children.Add(emptyCell);
            }

        }
    }

    private void UpdateCellStyle(Border cell, DateTime? cellTime)
    {
        var now = DateTime.Now;
        if (cellTime < now)
        {
            // Time has passed
            cell.Background = (Brush)Microsoft.UI.Xaml.Application.Current.Resources["SurfaceStrokeColorDefaultBrush"];
        }
        else
        {
            // Future or current time
            cell.Background = new SolidColorBrush(Colors.Transparent); // Default color
        }
    }

    private void LoadData()
    {
        foreach (var task in Values)
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
                Children.Add(weeklyEvent);
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

        var childrenToRemove = Children
            .OfType<FrameworkElement>()
            .Where(child => child.GetType() == contentType)
            .ToList();

        foreach (var child in childrenToRemove)
        {
            Children.Remove(child);
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
    private int CalculateRow(TimeOnly time)
    {
        var hourIndex = (time.Hour * 60 / _taskTimeUnit);
        var minuteIndex = time.Minute / _taskTimeUnit;
        return 1 + minuteIndex + hourIndex;
    }

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

    /// <summary>
    /// Sets the titles for the calendar grid.
    /// </summary>
    private void SetTitles()
    {
        // Add columns (1 for time labels, the rest for days)
        ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) }); // Time Column
        for (var i = 0; i < 7; i++) // For Monday to Sunday
        {
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });
        }

        // Add rows
        for (var i = 0; i <= _rowCount; i++) // 15 - 30 - 45 - 60minute intervals
        {
            RowDefinitions.Add(new RowDefinition { Height = new GridLength(60) });
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
            else
            {
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


            Children.Add(columnTitle);

        }

        var timeSpan = new TimeSpan();
        var sqrtFrequency = (int)Math.Floor(Math.Sqrt(_taskTimeUnit));
        for (var i = 0; i < _rowCount / sqrtFrequency; i++)
        {

            TextBlock timeLabel = new TextBlock
            {
                Text = timeSpan.ToString(@"hh\:mm"),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };

            var rowIndex = (i * sqrtFrequency) + 1;
            Grid.SetRow(timeLabel, rowIndex); // First half of the hour
            Grid.SetColumn(timeLabel, 0);
            Children.Add(timeLabel);

            timeSpan = timeSpan.Add(TimeSpan.FromMinutes(_taskTimeUnit * sqrtFrequency));
        }

    }
}
