using FluentAssertions;
using Moq;
using Moq.Protected;
using System.Net;
using TrackMyIP.Services;

namespace Tests.Services
{
    public class IpStackServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly IpStackService _ipStackService;

        public IpStackServiceTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("http://api.ipstack.com/")
            };

            _ipStackService = new IpStackService(_httpClient);
        }

        [Fact]
        public async Task CheckApiKeyAsync_ReturnsTrue_WhenResponseIsValid()
        {
            // Arrange
            var responseContent = "{}";
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",    // Name metod for mocking
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseContent)
                });

            // Act
            var result = await _ipStackService.CheckApiKeyAsync();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task CheckApiKeyAsync_ReturnsFalse_WhenApiKeyIsInvalid()
        {
            // Arrange
            var responseContent = "{\"error\":{\"code\":\"101\",\"type\":\"invalid_access_key\"}}";
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseContent)
                });

            // Act
            var result = await _ipStackService.CheckApiKeyAsync();

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task FetchLocationAsync_ReturnsGeolocationData_WhenResponseIsValid()
        {
            // Arrange
            var responseContent = @"{
                ""ip"": ""134.201.250.155"",
                ""country_name"": ""United States"",
                ""region_name"": ""California"",
                ""city"": ""Los Angeles"",
                ""latitude"": 34.04563903808594,
                ""longitude"": -118.24163818359375
            }";

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseContent)
                });

            // Act
            var result = await _ipStackService.FetchLocationAsync("134.201.250.155");

            // Assert
            result.Should().NotBeNull();
            result.IP.Should().Be("134.201.250.155");
            result.Country.Should().Be("United States");
            result.Region.Should().Be("California");
            result.City.Should().Be("Los Angeles");
            result.Latitude.Should().Be(34.04563903808594);
            result.Longitude.Should().Be(-118.24163818359375);
        }
    }
}