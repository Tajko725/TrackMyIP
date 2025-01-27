using Microsoft.EntityFrameworkCore;
using TrackMyIP.Models;
using TrackMyIP.Services.Interfaces;

namespace TrackMyIP.Services
{
    /// <summary>
    /// Service for managing geolocation data in the database.
    /// Implements CRUD operations for geolocation entries.
    /// </summary>
    public class GeolocationService : IGeolocationService
    {
        private readonly IDbContextFactory<GeolocationDbContext> _contextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeolocationService"/> class.
        /// Ensures that the database is created if it does not already exist.
        /// </summary>
        /// <param name="context">The database context for geolocation data.</param>
        public GeolocationService(IDbContextFactory<GeolocationDbContext> context)
        {
            _contextFactory = context;
        }

        /// <summary>
        /// Retrieves all geolocation entries from the database.
        /// </summary>
        /// <returns>A list of <see cref="GeolocationData"/> objects.</returns>
        public async Task<List<GeolocationData>> GetAllAsync()
        {
            using var cont = _contextFactory.CreateDbContext();
            return await cont.Geolocations.ToListAsync();
        }

        /// <summary>
        /// Adds a new geolocation entry to the database.
        /// </summary>
        /// <param name="data">The geolocation data to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddAsync(GeolocationData data)
        {
            using var cont = _contextFactory.CreateDbContext();
            await cont.Geolocations.AddAsync(data);
            await cont.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing geolocation entry in the database.
        /// </summary>
        /// <param name="data">The geolocation data with updated information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateAsync(GeolocationData data)
        {
            using var cont = _contextFactory.CreateDbContext();
            var existing = await cont.Geolocations.FindAsync(data.Id);
            if (existing != null)
            {
                existing.IP = data.IP;
                existing.Country = data.Country;
                existing.Region = data.Region;
                existing.City = data.City;
                existing.Latitude = data.Latitude;
                existing.Longitude = data.Longitude;

                await cont.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Deletes a geolocation entry from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the geolocation entry to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteAsync(int id)
        {
            using var cont = _contextFactory.CreateDbContext();
            var existing = await cont.Geolocations.FindAsync(id);

            if (existing != null)
            {
                cont.Geolocations.Remove(existing);
                await cont.SaveChangesAsync();
            }
        }
    }
}