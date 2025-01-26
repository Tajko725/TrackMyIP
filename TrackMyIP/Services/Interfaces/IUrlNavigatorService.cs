namespace TrackMyIP.Services.Interfaces
{
    /// <summary>
    /// Provides an abstraction for navigating to URLs in the default browser.
    /// </summary>
    public interface IUrlNavigatorService
    {
        /// <summary>
        /// Opens the specified URL in the default web browser.
        /// </summary>
        /// <param name="url">The URL to open.</param>
        void OpenUrl(string url);
    }
}
