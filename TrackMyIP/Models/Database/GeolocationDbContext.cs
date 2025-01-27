using Microsoft.EntityFrameworkCore;
using TrackMyIP.Models.Database;

namespace TrackMyIP.Models
{
    /// <summary>
    /// Represents the database context for managing geolocation data.
    /// Inherits from <see cref="DbContext"/> and configures the SQLite database connection and schema for geolocation data.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="GeolocationDbContext"/> class with specified options.
    /// </remarks>
    /// <param name="options">The options to be used by this context.</param>
    public class GeolocationDbContext(DbContextOptions<GeolocationDbContext> options) : DbContext(options)
    {
        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of geolocation data entities.
        /// </summary>
        public DbSet<GeolocationData> Geolocations { get; set; }

        /// <summary>
        /// Configures the database connection settings for the application.
        /// Sets the SQLite database file path to "TrackMyIP.db" in the application's base directory.
        /// </summary>
        /// <param name="optionsBuilder">The options builder used to configure the database context.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string dbPath = DatabaseConfig.GetDatabasePath();
                optionsBuilder.UseSqlite(dbPath);
            }
        }

        /// <summary>
        /// Configures the entity framework model for geolocation data.
        /// Sets the primary key for the <see cref="GeolocationData"/> entity to the "Id" property.
        /// </summary>
        /// <param name="modelBuilder">The model builder used to configure entity models.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GeolocationData>().HasKey(u => u.Id);
        }
    }
}