using Microsoft.UI.Xaml.Controls;

using TimeInABottle.ViewModels;

namespace TimeInABottle.Views;

public sealed partial class Blank2Page : Page
{
    public Blank2ViewModel ViewModel
    {
        get;
    }

    public Blank2Page()
    {
        ViewModel = App.GetService<Blank2ViewModel>();
        InitializeComponent();
    }
}
