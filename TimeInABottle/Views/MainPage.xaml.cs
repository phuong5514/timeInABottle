using Microsoft.UI.Xaml.Controls;

using TimeInABottle.ViewModels;

namespace TimeInABottle.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }
}
