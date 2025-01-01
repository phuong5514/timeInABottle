using CommunityToolkit.Mvvm.ComponentModel;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models.Tasks;
using TimeInABottle.Models;

namespace TimeInABottle.ViewModels;
/// <summary>
/// ViewModel for Create, Update, Delete (CUD) dialog operations.
/// </summary>
public partial class CUDDialogViewModel : ObservableRecipient
{
    /// <summary>
    /// Gets or sets the input name.
    /// </summary>
    public string InputName { set; get; }

    /// <summary>
    /// Gets or sets the input description.
    /// </summary>
    public string InputDescription { set; get; }

    /// <summary>
    /// Gets or sets the input start time.
    /// </summary>
    public TimeSpan InputStart { set; get; }

    /// <summary>
    /// Gets or sets the input end time.
    /// </summary>
    public TimeSpan InputEnd { set; get; }

    /// <summary>
    /// Gets or sets the task ID.
    /// </summary>
    public int Id
    {
        set; get;
    }

    /// <summary>
    /// Gets a value indicating whether the dialog is in edit mode.
    /// </summary>
    public bool IsEditMode => Id != 0;

    /// <summary>
    /// Gets the list of task options.
    /// </summary>
    public List<ITask> TaskOptions { private set; get; }

    /// <summary>
    /// Gets or sets the selected task option.
    /// </summary>
    public ITask? SelectedTaskOption
    {
        set; get;
    }

    /// <summary>
    /// Gets the type name of the selected task option.
    /// </summary>
    public string TypeName
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

    /// <summary>
    /// Gets a value indicating whether the combobox is enabled.
    /// </summary>
    public bool IsComboboxEnabled => !(IsEditMode && _task is DerivedTask);

    private ITask? _task;

    /// <summary>
    /// List of weekdays.
    /// </summary>
    public static readonly List<string> Weekdays = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"];

    /// <summary>
    /// Gets or sets the input weekdays.
    /// </summary>
    public List<DayOfWeek> InputWeekDays
    {
        set; get;
    }

    /// <summary>
    /// Gets or sets the input monthly day.
    /// </summary>
    public int InputMonthlyDay
    {
        set; get;
    }

    private static readonly List<int> _daysInMonths = [ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
                                                                   13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24,
                                                                   25, 26, 27, 28, 29, 30, 31 ];

    /// <summary>
    /// Gets the list of days in a month.
    /// </summary>
    public List<int> DaysInMonth => _daysInMonths;

    /// <summary>
    /// Gets or sets the input specific day.
    /// </summary>
    public DateTime InputSpecificDay;

    private readonly IDaoService _daoService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CUDDialogViewModel"/> class.
    /// </summary>
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
        TaskOptions = [];
        _task = null;
        SelectedTaskOption = null;
        SetTaskOptions();
    }

    /// <summary>
    /// Sets the ViewModel to edit mode with the specified task.
    /// </summary>
    /// <param name="task">The task to edit.</param>
    public void EditMode(ITask task)
    {
        if (task == null)
        {
            return;
        }

        _task = task;
        InputName = task.Name;
        InputDescription = task.Description;
        InputStart = task.Start.ToTimeSpan();
        InputEnd = task.End.ToTimeSpan();
        Id = task.Id;

        var taskVisitor = new GetTaskSpecialtiesVisitor();
        var specialisedValue = taskVisitor.VisitTask(task);
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

        if (task is DerivedTask)
        {
            SelectedTaskOption = new DerivedTask();
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

    /// <summary>
    /// Sets the task options available for selection.
    /// </summary>
    private void SetTaskOptions()
    {
        var taskType = typeof(ITask);
        var assembly = taskType.Assembly;

        var taskTypes = assembly.GetTypes()
                                .Where(t => t.IsSubclassOf(taskType) && !t.IsAbstract)
                                .ToList();

        foreach (var type in taskTypes)
        {
            if (Activator.CreateInstance(type) is ITask taskInstance)
            {
                if (taskInstance is DerivedTask)
                {
                    continue;
                }
                TaskOptions.Add(taskInstance);
            }
        }
    }

    /// <summary>
    /// Validates the input fields.
    /// </summary>
    /// <returns>The result code of the validation.</returns>
    private FunctionResultCode ValidateInput()
    {
        if (!ValidateEmptyInput())
        {
            return FunctionResultCode.ERROR_MISSING_INPUT;
        }

        return ValidateTime() ? FunctionResultCode.SUCCESS : FunctionResultCode.ERROR_INVALID_INPUT;
    }

    /// <summary>
    /// Validates that the input fields are not empty.
    /// </summary>
    /// <returns>True if the input fields are not empty; otherwise, false.</returns>
    private bool ValidateEmptyInput()
    {
        var result = true;
        result &= !string.IsNullOrWhiteSpace(InputName);
        result &= !string.IsNullOrWhiteSpace(TypeName);

        switch (TypeName)
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

    /// <summary>
    /// Validates the input start and end times.
    /// </summary>
    /// <returns>True if the input times are valid; otherwise, false.</returns>
    private bool ValidateTime()
    {
        if (InputStart >= InputEnd)
        {
            return false;
        }

        var timeGetter = App.GetService<IAvailableTimesGetter>();
        IEnumerable<TimeSpan> allowedStarts = Enumerable.Empty<TimeSpan>();
        IEnumerable<TimeSpan> allowedEnds = Enumerable.Empty<TimeSpan>();

        switch (TypeName)
        {
            case "WeeklyTask":
                (allowedStarts, allowedEnds) = timeGetter.GetAvailableTimesForWeek(InputWeekDays);
                break;
            case "MonthlyTask":
                (allowedStarts, allowedEnds) = timeGetter.GetAvailableTimesForDate(InputMonthlyDay);
                break;
            case "NonRepeatedTask":
                (allowedStarts, allowedEnds) = timeGetter.GetAvailableTimesForDate(DateOnly.FromDateTime(InputSpecificDay));
                break;
            case "DailyTask":
                (allowedStarts, allowedEnds) = timeGetter.GetAvailableTimesForToday();
                break;
        }

        return allowedStarts.Contains(InputStart) && allowedEnds.Contains(InputEnd);
    }

    /// <summary>
    /// Saves the changes made to the task.
    /// </summary>
    /// <returns>The result code of the save operation.</returns>
    public FunctionResultCode SaveChanges()
    {
        if (IsEditMode)
        {
            _daoService.DeleteTask(_task);
        }

        var validationResult = ValidateInput();
        if (validationResult != FunctionResultCode.SUCCESS)
        {
            if (IsEditMode)
            {
                _daoService.AddTask(_task);
            }

            return validationResult;
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
        return FunctionResultCode.SUCCESS;
    }

    /// <summary>
    /// Deletes the current task.
    /// </summary>
    /// <returns>True if the task was successfully deleted; otherwise, false.</returns>
    public bool DeleteTask()
    {
        _daoService.DeleteTask(_task);
        return true;
    }
}
