using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Audacia.Mandrill.Models.WebhookJsonDeserialisation
{
    /// <summary>The details of the message that triggered the webhook.</summary>
    public class WebhookMessageItem
    {
        /// <summary>The UNIX timestamp of the event.</summary>
        public long Ts { get; set; }

        /// <summary>The subject line of the email.</summary>
        public string Subject { get; set; }

        /// <summary>The email address of the recipient.</summary>
        public string Email { get; set; }

        /// <summary>The email address of the sender.</summary>
        public string Sender { get; set; }

        /// <summary>The tag names that are applied to the message, if any.</summary>
        public List<string> Tags { get; set; }

        /// <summary>An item for details of each SMTP response.</summary>
        [JsonPropertyName("Smtp_events")]
        public List<WebhookSmtpDetailsItem> SmtpEvents { get; set; }

        /// <summary>An item for details of each time the message was opened.</summary>
        public List<WebhookOpenDetailsItem> Opens { get; set; }

        /// <summary>An item for details of each time a link click was recorded.</summary>
        public List<WebhookClickDetailsItem> Clicks { get; set; }

        /// <summary>The state of the message, eg Sent or Rejected.</summary>
        public string State { get; set; }

        /// <summary>The metadata key:value pairs attached to the message.</summary>
        public Dictionary<string, object> Metadata { get; set; }

        /// <summary>The ID of the event.</summary>
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        /// <summary>The version of the email.</summary>
        [JsonPropertyName("_version")]
        public string Version { get; set; }

        /// <summary>The subaccount the message originated from.</summary>
        public string Subaccount { get; set; }

        /// <summary>The SMTP response code for bounced messages.</summary>
        public string Diag { get; set; }

        /// <summary>The description for bounced messages.</summary>
        [JsonPropertyName("Bounce_description")]
        public string BounceDescription { get; set; }

        /// <summary>The slug of the template used.</summary>
        public string Template { get; set; }
    }
}
