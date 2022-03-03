using Newtonsoft.Json;

namespace Audacia.Mandrill.Models.WebhookJsonDeserialisation
{
    /// <summary>Location details for the webhook trigger.</summary>
    public class WebhookLocationItem
    {
        /// <summary>Abbreviated name for the country.</summary>
        [JsonProperty("Country_short")]
        public string CountryShort { get; set; }

        /// <summary>Full name for the country.</summary>
        [JsonProperty("Country_long")]
        public string CountryLong { get; set; }

        /// <summary>Name for the region.</summary>
        public string Region { get; set; }

        /// <summary>Name for the city.</summary>
        public string City { get; set; }

        /// <summary>Postal code for the event.</summary>
        [JsonProperty("Postal_Code")]
        public string PostalCode { get; set; }

        /// <summary>Timezone for the event.</summary>
        public string Timezone { get; set; }

        /// <summary>Latitude for the event.</summary>
        public float Latitude { get; set; }

        /// <summary>Longitude for the event.</summary>
        public float Longitude { get; set; }
    }
}
