using Microsoft.UI.Xaml.Controls;

using TimeInABottle.ViewModels;

namespace TimeInABottle.Views;

public sealed partial class BlankPage : Page
{
    public BlankViewModel ViewModel
    {
        get;
    }

    public BlankPage()
    {
        ViewModel = App.GetService<BlankViewModel>();
        InitializeComponent();
    }
}
