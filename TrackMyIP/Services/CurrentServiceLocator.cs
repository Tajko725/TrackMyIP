using Microsoft.Extensions.DependencyInjection;

namespace TrackMyIP.Services
{
    /// <summary>
    /// Provides a static service locator for accessing registered services.
    /// Allows setting and retrieving services from a global <see cref="ServiceProvider"/>.
    /// </summary>
    public static class CurrentServiceLocator
    {
        /// <summary>
        /// The service provider used to resolve dependencies.
        /// </summary>
        private static ServiceProvider? Provider { get; set; }

        /// <summary>
        /// Sets the global <see cref="ServiceProvider"/> instance.
        /// </summary>
        /// <param name="provider">The <see cref="ServiceProvider"/> to be set.</param>
        public static void SetProvider(ServiceProvider provider)
            => Provider = provider;

        /// <summary>
        /// Retrieves a service of the specified type from the global <see cref="ServiceProvider"/>.
        /// </summary>
        /// <typeparam name="T">The type of service to retrieve.</typeparam>
        /// <returns>The resolved service of type <typeparamref name="T"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the provider is not set.</exception>
        public static T GetService<T>()
            => Provider != null ? (T)Provider.GetService(typeof(T))! : throw new InvalidOperationException("Services is not set.");
    }

}
