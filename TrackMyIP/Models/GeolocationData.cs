namespace TrackMyIP.Models
{
    /// <summary>
    /// Represents geolocation data for a specific IP address.
    /// </summary>
    public class GeolocationData
    {
        /// <summary>
        /// Unique identifier of the record.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The IP address associated with this geolocation.
        /// </summary>
        public string? IP { get; set; }

        /// <summary>
        /// The country where the IP address is located.
        /// </summary>
        public string? Country { get; set; }

        /// <summary>
        /// The region or state where the IP address is located.
        /// </summary>
        public string? Region { get; set; }

        /// <summary>
        /// The city where the IP address is located.
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// The latitude of the geolocation.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// The longitude of the geolocation.
        /// </summary>
        public double Longitude { get; set; }
    }
}
