//using Microsoft.UI.Dispatching;
//using Microsoft.UI.Xaml;
//using Microsoft.Windows.AppLifecycle;
//using Microsoft.Windows.AppNotifications;

//using TimeInABottle.Contracts.Services;
//using TimeInABottle.ViewModels;

//namespace TimeInABottle.Activation;

//public class AppNotificationActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
//{
//    private readonly INavigationService _navigationService;

//    public AppNotificationActivationHandler(INavigationService navigationService)
//    {
//        _navigationService = navigationService;
//    }

//    protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
//    {
//        return AppInstance.GetCurrent().GetActivatedEventArgs()?.Kind == ExtendedActivationKind.AppNotification;
//    }

//    protected async override Task HandleInternalAsync(LaunchActivatedEventArgs args)
//    {
//        // TODO: Handle notification activations.

//        //// // Access the AppNotificationActivatedEventArgs.
//        //// var activatedEventArgs = (AppNotificationActivatedEventArgs)AppInstance.GetCurrent().GetActivatedEventArgs().Data;

//        //// // Navigate to a specific page based on the notification arguments.
//        //// if (_notificationService.ParseArguments(activatedEventArgs.Argument)["action"] == "Settings")
//        //// {
//        ////     // Queue navigation with low priority to allow the UI to initialize.
//        ////     App.MainWindow.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Low, () =>
//        ////     {
//        ////         _navigationService.NavigateTo(typeof(SettingsViewModel).FullName!);
//        ////     });
//        //// }

//        App.MainWindow.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Low, () =>
//        {
//            App.MainWindow.ShowMessageDialogAsync("TODO: Handle notification activations.", "Notification Activation");
//        });

//        await Task.CompletedTask;
//    }
//}
