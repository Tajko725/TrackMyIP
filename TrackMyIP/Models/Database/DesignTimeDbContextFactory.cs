using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TrackMyIP.Models.Database
{
    /// <summary>
    /// Factory for creating GeolocationDbContext instances at design time.
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<GeolocationDbContext>
    {
        /// <summary>
        /// Creates a new instance of GeolocationDbContext with the specified options.
        /// </summary>
        /// <param name="args">Arguments passed during the design-time invocation.</param>
        /// <returns>A configured GeolocationDbContext instance.</returns>
        public GeolocationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GeolocationDbContext>();

            // Configure SQLite as the database provider
            string dbPath = DatabaseConfig.GetDatabasePath();
            optionsBuilder.UseSqlite(dbPath);

            return new GeolocationDbContext(optionsBuilder.Options);
        }
    }
}