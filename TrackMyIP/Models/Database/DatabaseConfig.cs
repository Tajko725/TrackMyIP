using System.IO;

namespace TrackMyIP.Models.Database
{
    /// <summary>
    /// Provides configuration and utility methods related to the database.
    /// This class centralizes the management of the database path and connection settings,
    /// ensuring consistency across the application.
    /// </summary>
    public static class DatabaseConfig
    {
        /// <summary>
        /// Gets the path to the SQLite database file used by the application.
        /// The path is located in the application's base directory and named "TrackMyIP.db".
        /// </summary>
        /// <returns>A string representing the full path to the database file.</returns>
        public static string GetDatabasePath()
        {
            return Path.Combine(AppContext.BaseDirectory, "TrackMyIP.db");
        }
    }
}
