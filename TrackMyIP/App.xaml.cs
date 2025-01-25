using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TrackMyIP.Services;
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

            var mainView = ServiceLocator.Services.GetRequiredService<MainView>();
            mainView.Show();
        }

        /// <summary>
        /// Handles application exit logic, including disposing of the service provider.
        /// </summary>
        /// <param name="e">Provides data for the <see cref="ExitEventArgs"/> event.</param>
        protected override void OnExit(ExitEventArgs e)
        {
            ServiceLocator.Dispose();
            base.OnExit(e);
        }
    }
}