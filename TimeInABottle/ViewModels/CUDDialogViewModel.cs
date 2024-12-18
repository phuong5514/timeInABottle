using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.ViewModels;
public partial class CUDDialogViewModel : ObservableRecipient
{
    public string InputName { set; get; }
    public string InputDescription { set; get; }
    public TimeSpan InputStart { set; get; }
    public TimeSpan InputEnd { set; get; }
    public int Id
    {
        set; get;
    }
    public bool IsEditMode => Id != 0;

    public List<ITask> TaskOptions { private set; get; }
    public ITask SelectedTaskOption
    {
        set; get;
    }
    public String TypeName
    {
        get
        {
            if (SelectedTaskOption == null)
            {
                return "";
            }
            return SelectedTaskOption.TypeName();
        }

    }


    private ITask _task;

    public static readonly List<string> Weekdays = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"];

    // specialized input
    public List<DayOfWeek> InputWeekDays
    {
        set; get;
    }

    public int InputMonthlyDay
    {
        set; get;
    }

    private static readonly List<int> _daysInMonths = [ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
                                                           13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24,
                                                           25, 26, 27, 28, 29, 30, 31 ];

    public List<int> DaysInMonth => _daysInMonths;

    public DateTime InputSpecificDay;

    private readonly IDaoService _daoService;

    public CUDDialogViewModel()
    {
        _daoService = App.GetService<IDaoService>();
        InputDescription = "";
        InputName = "";
        InputStart = new();
        InputEnd = new();
        InputWeekDays = [];
        InputMonthlyDay = 0;
        InputSpecificDay = DateTime.Now;
        SetTaskOptions();
    }

    public void EditMode(ITask task)
    {
        if (task == null) {
            return;
        }

        _task = task;
        InputName = task.Name;
        InputDescription = task.Description;
        InputStart = task.Start.ToTimeSpan();
        InputEnd = task.End.ToTimeSpan();
        Id = task.Id;


        var taskVisitor = new GetTaskSpecialtiesVisitor();
        var specialisedValue = taskVisitor.visitTask(task);
        if (specialisedValue != null)
        {
            if (specialisedValue is List<DayOfWeek> weekdays)
            {
                InputWeekDays = weekdays;
            }
            else if (specialisedValue is int monthlyDay)
            {
                InputMonthlyDay = monthlyDay;
            }
            else if (specialisedValue is DateOnly specificDay)
            {
                InputSpecificDay = specificDay.ToDateTime(TimeOnly.MinValue);
            }
        }

        foreach (var option in TaskOptions)
        {
            if (option.TypeName() == task.TypeName())
            {
                SelectedTaskOption = option;
                break;
            }
        }
    }

    private void SetTaskOptions()
    {
        TaskOptions = [];
        var taskType = typeof(ITask);
        var assembly = taskType.Assembly;

        var taskTypes = assembly.GetTypes()
                                .Where(t => t.IsSubclassOf(taskType) && !t.IsAbstract)
                                .ToList();

        foreach (var type in taskTypes)
        {
            if (Activator.CreateInstance(type) is ITask taskInstance)
            {
                TaskOptions.Add(taskInstance);
                //Core.Models.Tasks.TaskFactory.RegisterTask(type.Name, type);
            }
        }
    }

    private bool ValidateInput()
    {
        var result = true;
        result &= validateEmptyInput();
        result &= validateTime();
        return result;
    }

    private bool validateEmptyInput()
    {
        var result = true;
        result &= !string.IsNullOrWhiteSpace(InputName);
        result &= InputStart < InputEnd;
        result &= !string.IsNullOrWhiteSpace(TypeName);

        switch(TypeName)
        {
            case "WeeklyTask":
                result &= InputWeekDays.Count > 0;
                break;
            case "MonthlyTask":
                result &= InputMonthlyDay > 0;
                break;
            case "NonRepeatedTask":
                result &= DateOnly.FromDateTime(InputSpecificDay) >= new DateOnly();
                break;
        }
        return result;
    }

    private bool validateTime()
    {
        var allowedTimeSpans = new List<TimeSpan>();
        var timeGetter = App.GetService<IAvailableTimesGetter>();
        switch (TypeName)
        {
            case "WeeklyTask":
                allowedTimeSpans = (List<TimeSpan>) timeGetter.GetAvailableTimesForWeek(InputWeekDays);
                break;
            case "MonthlyTask":
                allowedTimeSpans = (List<TimeSpan>) timeGetter.GetAvailableTimesForDate(InputMonthlyDay);
                break;
            case "NonRepeatedTask":
                allowedTimeSpans = (List<TimeSpan>)timeGetter.GetAvailableTimesForDate(DateOnly.FromDateTime(InputSpecificDay));
                break;
        }

        return allowedTimeSpans.Contains(InputStart) && allowedTimeSpans.Contains(InputEnd);
    }


    public bool SaveChanges()
    {
        if (!ValidateInput())
        {
            return false;
        }

        if (IsEditMode)
        {
            _daoService.DeleteTask(_task);
        }

        if (TypeName == nameof(WeeklyTask) && InputWeekDays.Count == Weekdays.Count)
        {
            _task = Core.Models.Tasks.TaskFactory.CreateTask(nameof(DailyTask));
        }
        else
        {
            _task = Core.Models.Tasks.TaskFactory.CreateTask(TypeName);
        }


        _task.Name = InputName;
        _task.Description = InputDescription;
        _task.Start = TimeOnly.FromTimeSpan(InputStart);
        _task.End = TimeOnly.FromTimeSpan(InputEnd);
        if (_task is WeeklyTask weeklyTask)
        {
            weeklyTask.WeekDays = InputWeekDays;
        }
        else if (_task is MonthlyTask monthlyTask)
        {
            monthlyTask.Date = InputMonthlyDay;
        }
        else if (_task is NonRepeatedTask nonRepeatedTask)
        {
            nonRepeatedTask.Date = DateOnly.FromDateTime(InputSpecificDay);
        }

        _daoService.AddTask(_task);
        return true;
    }

    public bool DeleteTask()
    {
        _daoService.DeleteTask(_task);
        return true;
    }
}
