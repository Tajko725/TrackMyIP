using System.Diagnostics;
using TrackMyIP.Services.Interfaces;

namespace TrackMyIP.Services
{
    /// <summary>
    /// Provides functionality for navigating to a specified URL in the default web browser.
    /// Implements the <see cref="IUrlNavigatorService"/> interface.
    /// </summary>
    public class UrlNavigatorService : IUrlNavigatorService
    {
        /// <summary>
        /// Opens the specified URL in the default web browser.
        /// </summary>
        /// <param name="url">The URL to open. Must be a valid, non-empty string starting with "http://" or "https://".</param>
        /// <exception cref="ArgumentException">Thrown when the provided URL is null, empty, or invalid.</exception>
        /// <exception cref="Exception">Thrown when an error occurs while attempting to open the URL.</exception>
        public void OpenUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("URL nie może być nullem lub pusty.", nameof(url));

            if (!IsValidUrl(url))
                throw new ArgumentException("Niepoprawny URL.", nameof(url));

            try
            {
                // Use Process.Start to open the URL
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true // Ensures the URL is opened in the default browser
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Niepowodzenie otwarcia URL: {url}\nBłąd: {ex.Message}");
            }
        }

        /// <summary>
        /// Validates whether the provided string is a valid URL.
        /// </summary>
        /// <param name="url">The URL string to validate.</param>
        /// <returns>Returns <c>true</c> if the URL is valid, absolute, and uses the "http" or "https" scheme;
        /// otherwise, <c>false</c>.</returns>
        private bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
