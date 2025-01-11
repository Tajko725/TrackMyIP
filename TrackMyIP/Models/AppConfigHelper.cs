using System.Configuration;

namespace TrackMyIP.Models
{
    /// <summary>
    /// Helper class for managing application configuration settings stored in app.config.
    /// Provides methods to read, update, or add key-value pairs in the appSettings section.
    /// </summary>
    public static class AppConfigHelper
    {
        /// <summary>
        /// Reads the value of a specified key from the appSettings section of app.config.
        /// </summary>
        /// <param name="key">The name of the key to retrieve.</param>
        /// <returns>The value associated with the specified key, or null if the key does not exist.</returns>
        public static string? GetAppSetting(string key)
             => ConfigurationManager.AppSettings[key];

        /// <summary>
        /// Updates the value of an existing key or adds a new key-value pair to the appSettings section of app.config.
        /// Saves the changes and refreshes the appSettings section to reflect updates.
        /// </summary>
        /// <param name="key">The name of the key to update or add.</param>
        /// <param name="value">The new value to associate with the specified key.</param>
        public static void UpdateAppSetting(string key, string value)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (config.AppSettings.Settings[key] != null)
                config.AppSettings.Settings[key].Value = value;
            else
                config.AppSettings.Settings.Add(key, value);

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}