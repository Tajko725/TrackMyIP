using CommunityToolkit.Mvvm.DependencyInjection;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TrackMyIP.Models;
using TrackMyIP.Services;
using TrackMyIP.Services.Interfaces;
using TrackMyIP.ViewModels;
using TrackMyIP.Views;

namespace TrackMyIP
{
    /// <summary>
    /// Represents the entry point of the WPF application and manages its lifecycle.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Handles application startup logic, including initializing the main view.
        /// </summary>
        /// <param name="e">Provides data for the <see cref="StartupEventArgs"/> event.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Configure Dependency Injection services
            ConfigureServices();

            var mainView = Ioc.Default.GetRequiredService<MainView>();
            mainView.Show();
        }

        /// <summary>
        /// Configures the Dependency Injection services for the application.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void ConfigureServices()
        {
            var services = new ServiceCollection();

            // Register services
            services.AddSingleton<GeolocationDbContext>();
            services.AddSingleton<IGeolocationService, GeolocationService>();
            services.AddSingleton<IIpStackService, IpStackService>();
            services.AddSingleton<IDialogCoordinator>(DialogCoordinator.Instance);

            // Register ViewModels
            services.AddSingleton<MainViewModel>();
            services.AddTransient<SearchGeolocationViewModel>();
            services.AddTransient<SettingsViewModel>();

            // Register Views
            services.AddSingleton<MainView>();
            services.AddTransient<SearchGeolocationView>();

            // Assign the configuration to Ioc.Default
            Ioc.Default.ConfigureServices(services.BuildServiceProvider());
        }

        /// <summary>
        /// Handles application exit logic, including disposing of the service provider.
        /// </summary>
        /// <param name="e">Provides data for the <see cref="ExitEventArgs"/> event.</param>
        protected override void OnExit(ExitEventArgs e)
        {
            (Ioc.Default.GetRequiredService<IServiceProvider>() as IDisposable)?.Dispose();
            base.OnExit(e);
        }
    }
}