using Microsoft.Extensions.DependencyInjection;

namespace TrackMyIP.Services
{
    /// <summary>
    /// Provides a global service locator for accessing the application services.
    /// It initializes and disposes the <see cref="Services"/> used throughout the application.
    /// </summary>
    public static class ServiceLocator
    {
        /// <summary>
        /// The global <see cref="Services"/> instance for the application.
        /// </summary>
        private readonly static ServiceProvider _services;

        /// <summary>
        /// Exposes the global <see cref="IServiceProvider"/> instance.
        /// </summary>
        public static IServiceProvider Services => _services;

        /// <summary>
        /// Static constructor for initializing the <see cref="Services"/>.
        /// It also sets the provider in <see cref="CurrentServiceLocator"/>.
        /// </summary>
        static ServiceLocator()
        {
            _services = ServiceLocatorBuilder.Build();
            CurrentServiceLocator.SetProvider(_services);
        }

        /// <summary>
        /// Disposes the global <see cref="Services"/> and releases its resources.
        /// </summary>
        public static void Dispose()
        {
            _services.Dispose();
        }
    }
}
