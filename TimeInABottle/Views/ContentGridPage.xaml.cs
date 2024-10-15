using Microsoft.UI.Xaml.Controls;

using TimeInABottle.ViewModels;

namespace TimeInABottle.Views;

public sealed partial class ContentGridPage : Page
{
    public ContentGridViewModel ViewModel
    {
        get;
    }

    public ContentGridPage()
    {
        ViewModel = App.GetService<ContentGridViewModel>();
        InitializeComponent();
    }
}
