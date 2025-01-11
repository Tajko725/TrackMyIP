using TrackMyIP.Models;

namespace TrackMyIP.Services
{
    /// <summary>
    /// Defines methods for managing geolocation data.
    /// Provides an abstraction for CRUD operations on geolocation data entities.
    /// </summary>
    public interface IGeolocationService
    {
        /// <summary>
        /// Retrieves all geolocation data entries.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation, containing a list of <see cref="GeolocationData"/>.
        /// </returns>
        Task<List<GeolocationData>> GetAllAsync();

        /// <summary>
        /// Adds a new geolocation data entry.
        /// </summary>
        /// <param name="data">The geolocation data to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddAsync(GeolocationData data);

        /// <summary>
        /// Updates an existing geolocation data entry.
        /// </summary>
        /// <param name="data">The geolocation data with updated values.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAsync(GeolocationData data);

        /// <summary>
        /// Deletes a geolocation data entry by its unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the geolocation data to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAsync(int id);
    }
}
