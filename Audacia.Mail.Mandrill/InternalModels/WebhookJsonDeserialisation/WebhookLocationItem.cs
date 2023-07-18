using System.Text.Json.Serialization;

namespace Audacia.Mail.Mandrill.InternalModels.WebhookJsonDeserialisation
{
    /// <summary>Location details for the webhook trigger.</summary>
    public class WebhookLocationItem
    {
        /// <summary>Gets or sets the abbreviated name for the country.</summary>
        [JsonPropertyName("Country_short")]
        public string CountryShort { get; set; } = default!;

        /// <summary>Gets or sets the full name for the country.</summary>
        [JsonPropertyName("Country_long")]
        public string CountryLong { get; set; } = default!;

        /// <summary>Gets or sets the name for the region.</summary>
        public string Region { get; set; } = default!;

        /// <summary>Gets or sets the name for the city.</summary>
        public string City { get; set; } = default!;

        /// <summary>Gets or sets the postal code for the event.</summary>
        [JsonPropertyName("Postal_Code")]
        public string PostalCode { get; set; } = default!;

        /// <summary>Gets or sets the timezone for the event.</summary>
        public string Timezone { get; set; } = default!;

        /// <summary>Gets or sets the latitude for the event.</summary>
        public float Latitude { get; set; } = default!;

        /// <summary>Gets or sets the longitude for the event.</summary>
        public float Longitude { get; set; }
    }
}
