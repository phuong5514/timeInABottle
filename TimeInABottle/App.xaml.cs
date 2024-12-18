﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

using TimeInABottle.Activation;
using TimeInABottle.Background;
using TimeInABottle.Contracts.Services;
using TimeInABottle.Core.Contracts.Services;
using TimeInABottle.Core.Services;
using TimeInABottle.Models;
using TimeInABottle.Services;
using TimeInABottle.ViewModels;
using TimeInABottle.Views;
using Windows.ApplicationModel.Background;

namespace TimeInABottle;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public static UIElement? AppTitlebar { get; set; }

    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers

            // Services
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddTransient<IWebViewService, WebViewService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IBackgroundTaskRegisterService, BackgroundTaskRegisterService>();
            services.AddSingleton<ILocationService, LocationService>();
            services.AddSingleton<IWeatherService, ApiWeatherService>();
            services.AddSingleton<IBehaviorController, ApiWeatherServiceBehaviorController>();
            services.AddSingleton<IStorageService, LocalStorageService>();


            // Core Services
            services.AddSingleton<IDaoService, MockDaoService>();
            services.AddSingleton<ISampleDataService, SampleDataService>();
            services.AddSingleton<IFileService, FileService>();

            // Views and ViewModels
            services.AddTransient<TaskListViewModel>();
            services.AddTransient<TaskListPage>();
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<DashboardPage>();

            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();

            services.AddTransient<MainViewModel>();
            services.AddTransient<MainPage>();

            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();

            // BackgroundTask


            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
        }).
        Build();

        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        await App.GetService<IActivationService>().ActivateAsync(args);

        RegisterBackgroundTask();

        var behaviorController = App.GetService<IBehaviorController>();
        await behaviorController.RunAsync();
    }

    private void RegisterBackgroundTask()
    {
        var backgroundTaskRegisterService = App.GetService<IBackgroundTaskRegisterService>();

        backgroundTaskRegisterService.CleanRegister();

        backgroundTaskRegisterService.RegisterBackgroundTask("NotificationBackgroundTasks", "TimeInABottle.Background.NotificationBackgroundTasks", new TimeTrigger(15, false));
    }

}
