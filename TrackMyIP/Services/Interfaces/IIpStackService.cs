using TrackMyIP.Models;

namespace TrackMyIP.Services.Interfaces
{
    /// <summary>
    /// Defines the interface for interacting with the ipstack API.
    /// </summary>
    public interface IIpStackService
    {
        /// <summary>
        /// Checks if the provided API key is valid.
        /// </summary>
        /// <returns>A boolean indicating whether the API key is valid.</returns>
        Task<bool> CheckApiKeyAsync();

        /// <summary>
        /// Fetches geolocation data for a given IP address or URL.
        /// </summary>
        /// <param name="query">The IP address or URL to fetch geolocation data for.</param>
        /// <returns>A <see cref="GeolocationData"/> object containing the location details, or null if an error occurs.</returns>
        Task<GeolocationData?> FetchLocationAsync(string query);

        /// <summary>
        /// Validates the API key by making a test request to the ipstack service.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation, containing a string message indicating whether the API key is valid.
        /// Returns "Podano prawidłowy klucz." if the API key is valid, otherwise returns "Nieprawidłowy klucz API.".
        /// </returns>
        Task<string> ValidateApiKeyAsync();
    }
}