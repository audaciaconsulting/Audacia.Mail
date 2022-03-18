using System.Text.Json.Serialization;

namespace Audacia.Mandrill.Models.WebhookJsonDeserialisation
{
    public class WebhookSmtpDetailsItem
    {
        /// <summary>The UNIX timestamp of the event.</summary>
        public long Ts { get; set; }

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

        /// <summary>The size of the message being relayed.</summary>
        public string Size { get; set; }
    }
}
