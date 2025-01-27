using CommunityToolkit.Mvvm.DependencyInjection;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Net.Http;
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

            try
            {
                // Apply database migration
                using var scope = Ioc.Default.GetRequiredService<IServiceProvider>().CreateScope();
                var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<GeolocationDbContext>>();
                using var dbContext = dbContextFactory.CreateDbContext();
                dbContext.Database.Migrate();

                var mainView = Ioc.Default.GetRequiredService<MainView>();
                mainView.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas uruchamiania aplikacji: {ex.Message}", "Start aplikacji", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }

        /// <summary>
        /// Configures the Dependency Injection services for the application.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void ConfigureServices()
        {
            var services = new ServiceCollection();

            // Add HttpClient
            services.AddSingleton(provider =>
            {
                var client = new HttpClient()
                {
                    BaseAddress = new Uri("http://api.ipstack.com/")
                };

                return client;
            });

            // Register services
            services.AddPooledDbContextFactory<GeolocationDbContext>(options =>
            {
                string dbPath = Path.Combine(AppContext.BaseDirectory, "TrackMyIP.db");
                options.UseSqlite($"Data Source={dbPath}");
            });

            services.AddSingleton<IGeolocationService, GeolocationService>();
            services.AddSingleton<IIpStackService, IpStackService>();
            services.AddSingleton<IDialogCoordinator>(DialogCoordinator.Instance);
            services.AddSingleton<IMessageDialogService, MessageDialogService>();
            services.AddSingleton<IUrlNavigatorService, UrlNavigatorService>();

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