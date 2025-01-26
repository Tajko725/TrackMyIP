using FluentAssertions;
using Moq;
using System.Windows.Input;
using TrackMyIP.Models;
using TrackMyIP.Services.Interfaces;
using TrackMyIP.ViewModels;

namespace Tests.ViewModels
{
    public class SearchGeolocationViewModelTests
    {
        private readonly Mock<IIpStackService> _ipStackServiceMock;
        private readonly Mock<IMessageDialogService> _dialogServiceMock;
        private readonly SearchGeolocationViewModel _searchGeolocationViewModel;

        public SearchGeolocationViewModelTests()
        {
            _ipStackServiceMock = new Mock<IIpStackService>();
            _dialogServiceMock = new Mock<IMessageDialogService>();
            _searchGeolocationViewModel = new SearchGeolocationViewModel(_ipStackServiceMock.Object, _dialogServiceMock.Object);
        }

        [Fact]
        public async Task SearchCommandAsync_ValidAddress_ShouldFetchGeolocation()
        {
            // Arrange
            var mockGeolocationData = new GeolocationData
            {
                IP = "192.168.0.1",
                City = "Testowe miasto",
                Country = "Testowe Państwo"
            };

            _ipStackServiceMock
                .Setup(x => x.FetchLocationAsync(It.IsAny<string>()))
                .ReturnsAsync(mockGeolocationData);

            _searchGeolocationViewModel.AddressSearched = "192.168.0.1";

            // Act
            await _searchGeolocationViewModel.SearchCommandAsync!.ExecuteAsync(null);

            // Assert
            _searchGeolocationViewModel.GeolocationData.Should().BeEquivalentTo(mockGeolocationData);
            _searchGeolocationViewModel.GeolocationData!.City.Should().Be("Testowe miasto");
            _searchGeolocationViewModel.GeolocationData.Country.Should().Be("Testowe Państwo");
        }

        [Fact]
        public async Task SearchCommandAsync_InvalidAddress_ShouldNotSetGeolocationData()
        {
            // Arrange
            _ipStackServiceMock
                .Setup(x => x.FetchLocationAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Invalid Address"));

            _searchGeolocationViewModel.AddressSearched = "Invalid Address";

            // Act
            await _searchGeolocationViewModel.SearchCommandAsync!.ExecuteAsync(null);

            // Assert
            _searchGeolocationViewModel.GeolocationData.Should().BeNull();
        }

        [Fact]
        public void AddressSearched_SetValue_ShouldTriggerPropertyChanged()
        {
            // Arrange
            var wasPropertyChangedCalled = false;
            _searchGeolocationViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(SearchGeolocationViewModel.AddressSearched))
                {
                    wasPropertyChangedCalled = true;
                }
            };

            // Act
            _searchGeolocationViewModel.AddressSearched = "Testowy adres";

            // Assert
            wasPropertyChangedCalled.Should().BeTrue();
            _searchGeolocationViewModel.AddressSearched.Should().Be("Testowy adres");
        }

        [Fact]
        public void AddCommand_ShouldBeEnabled_WhenGeolocationDataIsSet()
        {
            // Arrange
            var mockGeolocationData = new GeolocationData { IP = "192.168.0.1" };
            _searchGeolocationViewModel.GeolocationData = mockGeolocationData;

            // Act
            var canExecute = _searchGeolocationViewModel.AddCommand!.CanExecute(null);

            // Assert
            canExecute.Should().BeTrue();
        }

        [Fact]
        public void AddCommand_ShouldBeDisabled_WhenGeolocationDataIsNull()
        {
            // Arrange
            _searchGeolocationViewModel.GeolocationData = null;

            // Act
            var canExecute = _searchGeolocationViewModel.AddCommand!.CanExecute(null);

            // Assert
            canExecute.Should().BeFalse();
        }

        [Fact]
        public async Task OnKeyPressedAsync_ShouldTriggerSearchAsync_WhenEnterKeyIsPressed()
        {
            // Arrange
            _ipStackServiceMock
                .Setup(x => x.FetchLocationAsync(It.IsAny<string>()))
                .ReturnsAsync(new GeolocationData
                {
                    IP = "192.168.0.1",
                    Country = "TestCountry",
                    Region = "TestRegion",
                    City = "TestCity",
                    Latitude = 0,
                    Longitude = 0
                });

            _searchGeolocationViewModel.AddressSearched = "192.168.0.1";

            // Act
            await _searchGeolocationViewModel.OnKeyPressedAsync(Key.Enter);

            // Assert
            _searchGeolocationViewModel.GeolocationData.Should().NotBeNull();
            _searchGeolocationViewModel.GeolocationData!.IP.Should().Be("192.168.0.1");
            _searchGeolocationViewModel.GeolocationData.Country.Should().Be("TestCountry");
        }
    }
}