using System.Text.Json.Serialization;

namespace Audacia.Mail.Mandrill.InternalModels.WebhookJsonDeserialisation
{
    /// <summary>An item holding details for the webhook being opened.</summary>
    public class WebhookOpenDetailsItem
    {
        /// <summary>Gets or sets the UNIX timestamp of the event.</summary>
        public long Ts { get; set; }

        /// <summary>Gets or sets the IP address where the open occurred.</summary>
        public string Ip { get; set; } = default!;

        /// <summary>Gets or sets the approximated geolocation of the IP where the open occurred.</summary>
        public string Location { get; set; } = default!;

        /// <summary>Gets or sets the operating system and browser for the open.</summary>
        public string Ua { get; set; } = default!;

        /// <summary>Gets or sets the type of SMTP event.</summary>
        public string Type { get; set; } = default!;

        /// <summary>Gets or sets the SMTP diagnostic or response event.</summary>
        public string Diag { get; set; } = default!;

        /// <summary>Gets or sets the remote IP address of the server Mandrill was connected to.</summary>
        [JsonPropertyName("Destination_ip")]
        public string DestinationIp { get; set; } = default!;

        /// <summary>Gets or sets the Mandrill IP address that was attempting to send the message.</summary>
        [JsonPropertyName("Source_ip")]
        public string SourceIp { get; set; } = default!;
    }
}
