using Newtonsoft.Json;

namespace Audacia.Mandrill.Models.WebhookJsonDeserialisation
{
    /// <summary>Details of the user agent for the webhook event.</summary>
    public class WebhookParsedItem
    {
        /// <summary>Whether the user agent is a mobile agent.</summary>
        public bool Mobile { get; set; }

        /// <summary>The operating system company.</summary>
        [JsonProperty("Os_company")]
        public string OsCompany { get; set; }

        /// <summary>Url of the operating system company.</summary>
        [JsonProperty("Os_company_url")]
        public string OsCompanyUrl { get; set; }

        /// <summary>The operating system family, eg Windows.</summary>
        [JsonProperty("Os_family")]
        public string OsFamily { get; set; }

        /// <summary>Url for an icon for the operating system.</summary>
        [JsonProperty("Os_icon")]
        public string OsIcon { get; set; }

        /// <summary>The name of the operating system used for the event.</summary>
        [JsonProperty("Os_name")]
        public string OsName { get; set; }

        /// <summary>Url for the operating system.</summary>
        [JsonProperty("Os_url")]
        public string OsUrl { get; set; }

        /// <summary>The type of user agent, eg Browser.</summary>
        public string Type { get; set; }

        /// <summary>Company for the user agent.</summary>
        [JsonProperty("Ua_company")]
        public string UaCompany { get; set; }

        /// <summary>Url for the user agent company.</summary>
        [JsonProperty("Ua_company_url")]
        public string UaCompanyUrl { get; set; }

        /// <summary>Family for the user agent.</summary>
        [JsonProperty("Ua_family")]
        public string UaFamily { get; set; }

        /// <summary>Url for an icon for the user agent.</summary>
        [JsonProperty("Ua_icon")]
        public string UaIcon { get; set; }

        /// <summary>Name of the user agent.</summary>
        [JsonProperty("Ua_name")]
        public string UaName { get; set; }

        /// <summary>Url for the user agent.</summary>
        [JsonProperty("Ua_url")]
        public string UaUrl { get; set; }

        /// <summary>Version of the user agent.</summary>
        [JsonProperty("Os_version")]
        public string OsVersion { get; set; }
    }
}
