using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TrackMyIP.Models;
using TrackMyIP.Services;

namespace Tests.Services
{
    public class GeolocationServiceTests
    {
        private readonly IDbContextFactory<GeolocationDbContext> _dbContextFactory;
        private readonly GeolocationService _geolocationService;

        public GeolocationServiceTests()
        {
            var options = new DbContextOptionsBuilder<GeolocationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique database for every test
                .Options;

            _dbContextFactory = new PooledDbContextFactory<GeolocationDbContext>(options);
            _geolocationService = new GeolocationService(_dbContextFactory);
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
            var datas = await _geolocationService.GetAllAsync();
            datas.Should().ContainSingle();
            datas.First().Should().BeEquivalentTo(geolocationData);
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

            await _geolocationService.AddAsync(geolocationData);
            geolocationData.IP = "1.1.1.2";

            // Act
            await _geolocationService.UpdateAsync(geolocationData);

            // Assert
            var datas = await _geolocationService.GetAllAsync();
            datas.Should().HaveCount(1);
            datas.First().IP.Should().Be("1.1.1.2");
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

            await _geolocationService.AddAsync(geolocationData);

            // Act
            await _geolocationService.DeleteAsync(geolocationData.Id);

            // Assert
            var datas = await _geolocationService.GetAllAsync();
            datas.Should().BeEmpty();
            datas.Should().NotContainEquivalentOf(geolocationData);
        }
    }
}