using System;
using System.Text.Json.Serialization;

namespace Audacia.Mail.Mandrill.InternalModels.WebhookJsonDeserialisation
{
    /// <summary>Details of the user agent for the webhook event.</summary>
    public class WebhookParsedItem
    {
        /// <summary>Gets or sets a value indicating whether the user agent is a mobile agent.</summary>
        public bool Mobile { get; set; }

        /// <summary>Gets or sets the operating system company.</summary>
        [JsonPropertyName("Os_company")]
        public string OsCompany { get; set; } = default!;

        /// <summary>Gets or sets the url of the operating system company.</summary>
        [JsonPropertyName("Os_company_url")]
        public Uri OsCompanyUrl { get; set; } = default!;

        /// <summary>Gets or sets the operating system family, eg Windows.</summary>
        [JsonPropertyName("Os_family")]
        public string OsFamily { get; set; } = default!;

        /// <summary>Gets or sets the url for an icon for the operating system.</summary>
        [JsonPropertyName("Os_icon")]
        public string OsIcon { get; set; } = default!;

        /// <summary>Gets or sets the name of the operating system used for the event.</summary>
        [JsonPropertyName("Os_name")]
        public string OsName { get; set; } = default!;

        /// <summary>Gets or sets the url for the operating system.</summary>
        [JsonPropertyName("Os_url")]
        public Uri OsUrl { get; set; } = default!;

        /// <summary>Gets or sets the type of user agent, eg Browser.</summary>
        public string Type { get; set; } = default!;

        /// <summary>Gets or sets the company for the user agent.</summary>
        [JsonPropertyName("Ua_company")]
        public string UaCompany { get; set; } = default!;

        /// <summary>Gets or sets the url for the user agent company.</summary>
        [JsonPropertyName("Ua_company_url")]
        public Uri UaCompanyUrl { get; set; } = default!;

        /// <summary>Gets or sets the family for the user agent.</summary>
        [JsonPropertyName("Ua_family")]
        public string UaFamily { get; set; } = default!;

        /// <summary>Gets or sets the url for an icon for the user agent.</summary>
        [JsonPropertyName("Ua_icon")]
        public string UaIcon { get; set; } = default!;

        /// <summary>Gets or sets the name of the user agent.</summary>
        [JsonPropertyName("Ua_name")]
        public string UaName { get; set; } = default!;

        /// <summary>Gets or sets the url for the user agent.</summary>
        [JsonPropertyName("Ua_url")]
        public Uri UaUrl { get; set; } = default!;

        /// <summary>Gets or sets the version of the user agent.</summary>
        [JsonPropertyName("Os_version")]
        public string OsVersion { get; set; } = default!;
    }
}
