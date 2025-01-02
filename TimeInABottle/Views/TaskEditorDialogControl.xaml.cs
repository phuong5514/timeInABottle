using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using TimeInABottle.ViewModels;

namespace TimeInABottle.Views;
/// <summary>
/// Represents a user control for editing tasks in a dialog.
/// </summary>
public sealed partial class TaskEditorDialogControl : UserControl
{

    /// <summary>
    /// Gets or sets the ViewModel for the dialog.
    /// </summary>
    public CUDDialogViewModel ViewModel
    {
        get => _viewModel;
        set
        {
            _viewModel = value;
            if (_viewModel != null && NonRepeatedTaskDatePicker != null)
            {
                NonRepeatedTaskDatePicker.Date = new DateTimeOffset(_viewModel.InputSpecificDay);
            }
        }
    }
    private CUDDialogViewModel _viewModel = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskEditorDialogControl"/> class.
    /// </summary>
    public TaskEditorDialogControl()
    {
        InitializeComponent();
    }

    private void Option_Checked(object sender, RoutedEventArgs e)
    {
        // get sender name
        var option = (CheckBox)sender;
        var day = ExtractCheckboxOptionValue(option);
        if (ViewModel.InputWeekDays.Contains(day))
        {
            return;
        }
        ViewModel.InputWeekDays.Add(day);
    }

    private void Option_Unchecked(object sender, RoutedEventArgs e)
    {
        var option = (CheckBox)sender;
        var day = ExtractCheckboxOptionValue(option);
        ViewModel.InputWeekDays.Remove(day);
    }

    private DayOfWeek ExtractCheckboxOptionValue(CheckBox sender)
    {
        var optionContentString = sender.Content.ToString();
        if (optionContentString == null)
        {
            throw new ArgumentNullException(nameof(optionContentString));
        }
        return (DayOfWeek)Enum.Parse(typeof(DayOfWeek), optionContentString);
    }

    private void SelectAll_Checked(object sender, RoutedEventArgs e)
    {
        // check all checkboxes
        foreach (var child in Weekdays.Children)
        {
            if (child is CheckBox checkbox)
            {
                checkbox.IsChecked = true;
            }
        }
    }

    private void SelectAll_Unchecked(object sender, RoutedEventArgs e)
    {
        foreach (var child in Weekdays.Children)
        {
            if (child is CheckBox checkbox)
            {
                checkbox.IsChecked = false;
            }
        }
    }

    //private void SelectAll_Indeterminate(object sender, RoutedEventArgs e)
    //{
    //    foreach (var child in Weekdays.Children)
    //    {
    //        if (child is CheckBox checkbox)
    //        {
    //            checkbox.IsChecked = false;
    //        }
    //    }
    //}

    private void NonRepeatedTaskDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
    {
        if (sender.Date.HasValue)
        {
            ViewModel.InputSpecificDay = sender.Date.Value.DateTime;
        }
    }
}
