using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using TimeInABottle.Core.Helpers;
using TimeInABottle.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TimeInABottle.Views;
public sealed partial class TaskEditorDialogControl : UserControl
{
    public CUDDialogViewModel ViewModel {
        get;
    }

    public TaskEditorDialogControl()
    {
        ViewModel = new CUDDialogViewModel();
        this.InitializeComponent();
    }

    private void Option_Checked(object sender, RoutedEventArgs e)
    {
        // get sender name
        var option = (CheckBox)sender;
        var day = ExtractCheckboxOptionValue(option);
        ViewModel.InputWeekDays.Add(day);
    }

    private void Option_Unchecked(object sender, RoutedEventArgs e)
    {
        var option = (CheckBox)sender;
        var day = ExtractCheckboxOptionValue(option);
        ViewModel.InputWeekDays.Remove(day);
    }

    private Values.Weekdays ExtractCheckboxOptionValue(CheckBox sender)
    {
        var optionContentString = sender.Content.ToString();
        if (optionContentString == null) {
            throw new ArgumentNullException(nameof(optionContentString));
        }
        return (Values.Weekdays)Enum.Parse(typeof(Values.Weekdays), optionContentString);
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

    private void SelectAll_Indeterminate(object sender, RoutedEventArgs e)
    {

    }
}
