using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Audacia.Mail.Mandrill.InternalModels.WebhookJsonDeserialisation
{
    /// <summary>The details of the message that triggered the webhook.</summary>
    public class WebhookMessageItem
    {
        /// <summary>Gets or sets the UNIX timestamp of the event.</summary>
        public long Ts { get; set; }

        /// <summary>Gets or sets the subject line of the email.</summary>
        public string Subject { get; set; }

        /// <summary>Gets or sets the email address of the recipient.</summary>
        public string Email { get; set; }

        /// <summary>Gets or sets the email address of the sender.</summary>
        public string Sender { get; set; }

        /// <summary>Gets or sets the tag names that are applied to the message, if any.</summary>
        public List<string> Tags { get; set; }

        /// <summary>Gets or sets the details of each SMTP response.</summary>
        [JsonPropertyName("Smtp_events")]
        public List<WebhookSmtpDetailsItem> SmtpEvents { get; set; }

        /// <summary>Gets or sets the details of each time the message was opened.</summary>
        public List<WebhookOpenDetailsItem> Opens { get; set; }

        /// <summary>Gets or sets the details of each time a link click was recorded.</summary>
        public List<WebhookClickDetailsItem> Clicks { get; set; }

        /// <summary>Gets or sets the state of the message, eg Sent or Rejected.</summary>
        public string State { get; set; }

        /// <summary>Gets or sets the metadata key:value pairs attached to the message.</summary>
        public Dictionary<string, object> Metadata { get; set; }

        /// <summary>Gets or sets the ID of the event.</summary>
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        /// <summary>Gets or sets the version of the email.</summary>
        [JsonPropertyName("_version")]
        public string Version { get; set; }

        /// <summary>Gets or sets thesubaccount the message originated from.</summary>
        public string Subaccount { get; set; }

        /// <summary>Gets or sets the SMTP response code for bounced messages.</summary>
        public string Diag { get; set; }

        /// <summary>Gets or sets the description for bounced messages.</summary>
        [JsonPropertyName("Bounce_description")]
        public string BounceDescription { get; set; }

        /// <summary>Gets or sets the slug of the template used.</summary>
        public string Template { get; set; }
    }
}
