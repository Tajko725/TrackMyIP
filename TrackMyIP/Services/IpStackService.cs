using Newtonsoft.Json.Linq;
using System.Net.Http;
using TrackMyIP.Models;
using TrackMyIP.Services.Interfaces;

namespace TrackMyIP.Services
{
    /// <summary>
    /// Service for interacting with the ipstack API, including checking API keys,
    /// fetching geolocation data, and monitoring API usage.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="IpStackService"/> class.
    /// </remarks>
    /// <param name="httpClient">An instance of <see cref="HttpClient"/> for making HTTP requests.</param>
    public class IpStackService(HttpClient httpClient) : IIpStackService
    {
        private const string _baseUrl = "http://api.ipstack.com/";
        private readonly HttpClient _httpClient = httpClient;

        /// <summary>
        /// Checks if the provided API key is valid.
        /// </summary>
        /// <returns>A boolean indicating whether the API key is valid.</returns>
        public async Task<bool> CheckApiKeyAsync()
        {
            try
            {
                var data = await SendRequestAsync("www.google.pl");
                var error = data["error"];
                return error == null || error["code"]?.ToString() != "101";
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Fetches geolocation data for a given IP address or URL.
        /// </summary>
        /// <param name="query">The IP address or URL to fetch geolocation data for.</param>
        /// <returns>A <see cref="GeolocationData"/> object containing the location details, or null if an error occurs.</returns>
        /// <exception cref="Exception">Throws an exception if the API key is invalid or the query is incorrect.</exception>
        public async Task<GeolocationData?> FetchLocationAsync(string query)
        {
            var data = await SendRequestAsync(query);
            var error = data["error"];
            if (error != null)
            {
                var code = error["code"]?.ToString();
                if (code == "101") throw new Exception("Nieprawidłowy klucz API.");
                if (code == "106") throw new Exception("Błędny adres IP / URL.");
            }

            return new GeolocationData
            {
                IP = data["ip"]?.ToString(),
                Country = data["country_name"]?.ToString(),
                Region = data["region_name"]?.ToString(),
                City = data["city"]?.ToString(),
                Latitude = data["latitude"]?.ToObject<double>() ?? 0,
                Longitude = data["longitude"]?.ToObject<double>() ?? 0
            };
        }

        /// <summary>
        /// Validates the API key by making a test request to the ipstack service.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation, containing a string message indicating whether the API key is valid.
        /// Returns "Podano prawidłowy klucz." if the API key is valid, otherwise returns "Nieprawidłowy klucz API.".
        /// </returns>
        public async Task<string> ValidateApiKeyAsync()
        {
            bool isOk = await CheckApiKeyAsync();
            return isOk ? "Podano prawidłowy klucz." : "Nieprawidłowy klucz API.";
        }

        /// <summary>
        /// Sends a request to the ipstack API and parses the response as a JSON object.
        /// </summary>
        /// <param name="query">The query string, such as an IP address, URL, or "status" for usage information.</param>
        /// <returns>A <see cref="JObject"/> containing the API response.</returns>
        /// <exception cref="Exception">Throws an exception if the HTTP request fails or the response is not successful.</exception>
        private async Task<JObject> SendRequestAsync(string query)
        {
            string url = $"{_baseUrl}{query}?access_key={AppConfigHelper.GetAppSetting("IPStackApiKey")}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"HTTP Error: {response.StatusCode}");

            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JObject.Parse(jsonResponse);
        }
    }
}