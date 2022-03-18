using System.Text.Json.Serialization;

namespace Audacia.Mandrill.Models.WebhookJsonDeserialisation
{
    /// <summary>An item holding details for the webhook being opened.</summary>
    public class WebhookOpenDetailsItem
    {
        /// <summary>The UNIX timestamp of the event.</summary>
        public long Ts { get; set; }

        /// <summary>The IP address where the open occurred.</summary>
        public string Ip { get; set; }

        /// <summary>The approximated geolocation of the IP where the open occurred.</summary>
        public string Location { get; set; }

        /// <summary>The operating system and browser for the open.</summary>
        public string Ua { get; set; }

        /// <summary>The type of SMTP event</summary>
        public string Type { get; set; }

        /// <summary>The SMTP diagnostic or response event.</summary>
        public string Diag { get; set; }

        /// <summary>The remote IP address of the server Mandrill was connected to.</summary>
        [JsonPropertyName("Destination_ip")]
        public string DestinationIp { get; set; }

        /// <summary>The Mandrill IP address that was attempting to send the message.</summary>
        [JsonPropertyName("Source_ip")]
        public string SourceIp { get; set; }
    }
}
