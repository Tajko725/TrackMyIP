using MahApps.Metro.Controls.Dialogs;
using Microsoft.Extensions.DependencyInjection;
using TrackMyIP.Models;
using TrackMyIP.Services.Interfaces;
using TrackMyIP.ViewModels;
using TrackMyIP.Views;

namespace TrackMyIP.Services
{
    /// <summary>
    /// Provides a builder class for constructing the <see cref="ServiceProvider"/>.
    /// It defines all the required services, ViewModels, and views for the application.
    /// </summary>
    public static class ServiceLocatorBuilder
    {
        /// <summary>
        /// Builds and returns the <see cref="ServiceProvider"/> with all required services.
        /// </summary>
        /// <returns>A fully constructed <see cref="ServiceProvider"/> instance.</returns>
        public static ServiceProvider Build()
        {
            var services = new ServiceCollection();
            services.AddServices();

            var provider = services.BuildServiceProvider();
            return provider;
        }

        /// <summary>
        /// Registers all required services, ViewModels, and views in the provided <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to register services with.</param>
        private static void AddServices(this IServiceCollection services)
        {
            // Register database context
            services.AddSingleton<GeolocationDbContext>();

            // Register services
            services.AddSingleton<IGeolocationService, GeolocationService>();
            services.AddSingleton<IIpStackService, IpStackService>();
            services.AddSingleton<IDialogCoordinator>(DialogCoordinator.Instance);

            // Register ViewModels
            services.AddSingleton<MainViewModel>();
            services.AddTransient<SearchGeolocationViewModel>();
            services.AddTransient<SettingsViewModel>();

            // Register views
            services.AddSingleton<MainView>();
            services.AddTransient<SearchGeolocationView>();
        }
    }
}
