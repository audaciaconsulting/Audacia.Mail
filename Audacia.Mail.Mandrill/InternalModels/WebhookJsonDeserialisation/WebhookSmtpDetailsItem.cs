using System.Text.Json.Serialization;

namespace Audacia.Mail.Mandrill.InternalModels.WebhookJsonDeserialisation
{
    /// <summary>
    /// Class modelling an SMTP event.
    /// </summary>
    public class WebhookSmtpDetailsItem
    {
        /// <summary>Gets or sets the UNIX timestamp of the event.</summary>
        public long Ts { get; set; }

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

        /// <summary>Gets or sets the size of the message being relayed.</summary>
        public string Size { get; set; } = default!;
    }
}
