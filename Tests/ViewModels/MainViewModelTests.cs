using FluentAssertions;
using MahApps.Metro.Controls.Dialogs;
using Moq;
using System.Reflection;
using System.Windows;
using TrackMyIP.Models;
using TrackMyIP.Services.Interfaces;
using TrackMyIP.ViewModels;

namespace Tests.ViewModels
{
    public class MainViewModelTests
    {
        private readonly Mock<IGeolocationService> _geolocationServiceMock;
        private readonly Mock<IDialogCoordinator> _dialogCoordinatorMock;
        private readonly Mock<IMessageDialogService> _dialogServiceMock;
        private readonly MainViewModel _mainViewModel;

        public MainViewModelTests()
        {
            _geolocationServiceMock = new Mock<IGeolocationService>();
            _dialogCoordinatorMock = new Mock<IDialogCoordinator>();
            _dialogServiceMock = new Mock<IMessageDialogService>();
            _mainViewModel = new MainViewModel(_geolocationServiceMock.Object,
                _dialogServiceMock.Object,
                _dialogCoordinatorMock.Object,
                new SettingsViewModel(Mock.Of<IIpStackService>(), _dialogServiceMock.Object, Mock.Of<IUrlNavigatorService>()));
        }

        [Fact]
        public async Task LoadGeolocationsAsync_ShouldPopulateGeolocationsCollection()
        {
            // Arrange
            _geolocationServiceMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(
                [
                    new GeolocationData { IP = "192.168.0.1" },
                    new GeolocationData { IP = "192.168.0.2" }
                ]);

            // Act
            await _mainViewModel.RefreshGeoocationsCommandAsync!.ExecuteAsync(null);

            // Assert
            _mainViewModel.Geolocations.Should().HaveCount(2);
            _mainViewModel.Geolocations.Should().Contain(g => g.IP == "192.168.0.1");
            _mainViewModel.Geolocations.Should().Contain(g => g.IP == "192.168.0.2");
        }

        [Fact]
        public async Task RefreshGeolocationsCommand_ShouldLoadGeolocations_WhenCalled()
        {
            // Arrange
            var geoLocations = new List<GeolocationData>
            {
                new(){ IP = "1.1.1.1", Country = "Polska" },
                new(){ IP = "2.2.2.2", Country = "Japonia" }
            };

            _geolocationServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(geoLocations);

            // Act
            await _mainViewModel.RefreshGeoocationsCommandAsync!.ExecuteAsync(null);

            // Assert
            _mainViewModel.Geolocations.Should().HaveCount(2);
            _mainViewModel.Geolocations.Should().ContainSingle(x => x.IP == "1.1.1.1");
        }

        [Fact]
        public void ShowGeolocationsCommand_ShouldSetVisibility_WhenExecuted()
        {
            // Act
            _mainViewModel.ShowGeolocationsCommand!.Execute(null);

            // Assert
            _mainViewModel.ShowGeolocationsVisibility.Should().Be(Visibility.Visible);
            _mainViewModel.ShowSettingsVisibility.Should().Be(Visibility.Collapsed);
        }

        [Fact]
        public void ShowSettingsCommand_ShouldSetVisibility_WhenExecuted()
        {
            // Act
            _mainViewModel.ShowSettingsCommand!.Execute(null);

            // Assert
            _mainViewModel.ShowSettingsVisibility.Should().Be(Visibility.Visible);
            _mainViewModel.ShowGeolocationsVisibility.Should().Be(Visibility.Collapsed);
        }

        [Fact]
        public async Task ShowSearchingGeolocationCommand_ShouldCallAddLocationAsync_WhenNewGeolocationIsProvided()
        {
            // Arrange
            var geolocation = new GeolocationData()
            {
                IP = "192.168.0.1",
                Country = "Polska",
                Region = "Dolnyśląsk",
                City = "Wrocław",
                Latitude = 52.2297,
                Longitude = 21.0122
            };

            _geolocationServiceMock
                .Setup(x => x.AddAsync(It.IsAny<GeolocationData>()))
                .Returns(Task.CompletedTask);

            // Act
            var addLocationAsyncMethod = typeof(MainViewModel)
                .GetMethod("AddLocationAsync", BindingFlags.NonPublic | BindingFlags.Instance);
            await (Task)addLocationAsyncMethod!.Invoke(_mainViewModel, [geolocation])!;

            // Assert

            _mainViewModel.Geolocations.Should().Contain(geolocation);
            _geolocationServiceMock.Verify(x => x.AddAsync(It.IsAny<GeolocationData>()), Times.Once);
        }

        [Fact]
        public void UpdateGeolocationCommand_CanExecute_ShouldReturnTrue_WhenSelectedGeolocationIsNotNullAsync()
        {
            // Arrange
            _mainViewModel.SelectedGeolocation = new GeolocationData
            {
                IP = "192.168.1.1"
            };

            // Act
            var canExecute = _mainViewModel.UpdateGeolocationCommandAsync!.CanExecute(null);

            // Assert
            canExecute.Should().BeTrue();
        }

        [Fact]
        public void UpdateGeolocationCommand_CanExecute_ShouldReturnFalse_WhenSelectedGeolocationIsNull()
        {
            // Arrange
            _mainViewModel.SelectedGeolocation = null;

            // Act
            var canExecute = _mainViewModel.UpdateGeolocationCommandAsync!.CanExecute(null);

            // Assert
            canExecute.Should().BeFalse();
        }

        [Fact]
        public async Task EditLocationAsync_ShouldUpdateGeolocation_WhenCalled()
        {
            // Arrange
            var initialGeolocation = new GeolocationData
            {
                IP = "192.168.1.1",
                Id = 1
            };

            var updatedGeolocation = new GeolocationData
            {
                IP = "192.168.1.2",
                Id = 1
            };

            _mainViewModel.Geolocations.Add(initialGeolocation);
            _mainViewModel.SelectedGeolocation = updatedGeolocation;

            _geolocationServiceMock
                .Setup(x => x.UpdateAsync(It.IsAny<GeolocationData>()))
                .Returns(Task.CompletedTask);

            // Act
            await _mainViewModel.UpdateGeolocationCommandAsync!.ExecuteAsync(null);

            // Assert
            var updatedItem = _mainViewModel.Geolocations.FirstOrDefault(x => x.Id == updatedGeolocation.Id);
            updatedItem.Should().NotBeNull();
            updatedItem!.IP.Should().Be("192.168.1.2");
            _geolocationServiceMock.Verify(x => x.UpdateAsync(It.Is<GeolocationData>(g => g.IP == "192.168.1.2")), Times.Once);
        }

        [Fact]
        public void DeleteGeolocationCommand_CanExecute_ShouldReturnTrue_WhenSelectedGeolocationIsNotNull()
        {
            // Arrange
            _mainViewModel.SelectedGeolocation = new GeolocationData
            {
                IP = "192.168.1.1",
                Id = 1
            };

            // Act
            var canExecute = _mainViewModel.DeleteGeolocationCommandAsync!.CanExecute(null);

            // Assert
            canExecute.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteGeolocationCommandAsync_CanExecute_ShouldReturnTrue_WhenSelectedGeolocationIsNotNull()
        {
            // Arrange
            _dialogServiceMock
                .Setup(x => x.ShowMessageAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    MessageDialogStyle.AffirmativeAndNegative))
                .ReturnsAsync(MessageDialogResult.Affirmative);

            _geolocationServiceMock
                .Setup(x => x.DeleteAsync(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            _mainViewModel.SelectedGeolocation = new GeolocationData
            {
                IP = "192.168.1.1",
                Id = 1
            };
            _mainViewModel.Geolocations.Add(_mainViewModel.SelectedGeolocation);

            // Act
            await _mainViewModel.DeleteGeolocationCommandAsync!.ExecuteAsync(null);

            // Assert
            _mainViewModel.Geolocations.Should().HaveCount(0);
            _dialogServiceMock.Verify(x => x.ShowMessageAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                MessageDialogStyle.AffirmativeAndNegative), Times.Once);
        }
    }
}