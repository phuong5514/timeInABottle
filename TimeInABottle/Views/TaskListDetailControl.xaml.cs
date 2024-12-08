using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Views;

public sealed partial class TaskListDetailControl : UserControl
{
    public ITask? ListDetailsMenuItem
    {
        get => GetValue(ListDetailsMenuItemProperty) as ITask;
        set => SetValue(ListDetailsMenuItemProperty, value);
    }

    public static readonly DependencyProperty ListDetailsMenuItemProperty = DependencyProperty.Register("ListDetailsMenuItem", typeof(ITask), typeof(TaskListDetailControl), new PropertyMetadata(null, OnListDetailsMenuItemPropertyChanged));

    public TaskListDetailControl()
    {
        InitializeComponent();
    }

    private static void OnListDetailsMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TaskListDetailControl control)
        {
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
