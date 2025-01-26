using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TrackMyIP.Models;
using TrackMyIP.Services;

namespace Tests.Services
{
    public class GeolocationServiceTests : IDisposable
    {
        private readonly GeolocationDbContext _geolocationDbContext;
        private readonly GeolocationService _geolocationService;

        public GeolocationServiceTests()
        {
            var options = new DbContextOptionsBuilder<GeolocationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique database for every test
                .Options;

            _geolocationDbContext = new GeolocationDbContext(options);
            _geolocationService = new GeolocationService(_geolocationDbContext);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllGeolocationData()
        {
            // Arrange
            var geolocationData = new GeolocationData()
            {
                IP = "1.1.1.1",
                City = "Test City 1",
                Country = "Test Country 1",
                Latitude = 1.1,
                Longitude = 1.1
            };

            // Act
            await _geolocationService.AddAsync(geolocationData);

            // Assert
            var datas = await _geolocationService.GetAllAsync();
            datas.Should().NotBeEmpty();
            datas.Should().ContainSingle();
            datas.Should().ContainEquivalentOf(geolocationData);
        }

        [Fact]
        public async Task AddAsync_ShouldAddNewGeolocation()
        {
            // Arrange
            var geolocationData = new GeolocationData()
            {
                IP = "1.1.1.1",
                City = "Test City 1",
                Country = "Test Country 1",
                Latitude = 1.1,
                Longitude = 1.1
            };

            // Act
            await _geolocationService.AddAsync(geolocationData);

            // Assert
            var firstData = _geolocationDbContext.Geolocations.FirstOrDefault();
            firstData.Should().NotBeNull();
            firstData.Should().BeEquivalentTo(geolocationData);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateExistingGeolocation()
        {
            // Arrange
            var geolocationData = new GeolocationData()
            {
                IP = "1.1.1.1",
                City = "Test City 1",
                Country = "Test Country 1",
                Latitude = 1.1,
                Longitude = 1.1
            };

            await _geolocationDbContext.Geolocations.AddAsync(geolocationData);
            await _geolocationDbContext.SaveChangesAsync();

            geolocationData.IP = "1.1.1.2";

            // Act
            await _geolocationService.UpdateAsync(geolocationData);

            // Assert
            var updatedData = await _geolocationDbContext.Geolocations.FindAsync(geolocationData.Id);
            updatedData.Should().NotBeNull();
            updatedData.IP.Should().Be("1.1.1.2");
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteExistingGeolocation()
        {
            // Arrange
            var geolocationData = new GeolocationData()
            {
                IP = "1.1.1.1",
                City = "Test City 1",
                Country = "Test Country 1",
                Latitude = 1.1,
                Longitude = 1.1
            };

            await _geolocationDbContext.Geolocations.AddAsync(geolocationData);
            await _geolocationDbContext.SaveChangesAsync();

            // Act
            await _geolocationService.DeleteAsync(geolocationData.Id);

            // Assert
            var deletedData = await _geolocationDbContext.Geolocations.FindAsync(geolocationData.Id);
            deletedData.Should().BeNull();
        }

        public void Dispose()
        {
            _geolocationDbContext.Dispose();
        }
    }
}