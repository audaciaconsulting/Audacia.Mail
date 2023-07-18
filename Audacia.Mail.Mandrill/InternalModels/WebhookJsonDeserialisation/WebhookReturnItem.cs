using System;
using System.Text.Json.Serialization;

namespace Audacia.Mail.Mandrill.InternalModels.WebhookJsonDeserialisation
{
    /// <summary>An item passed in the JSON from a webhook event occurring.</summary>
    /// <typeparam name="T">The type of event that caused the webhook to be triggered.</typeparam>
    public class WebhookReturnItem<T> where T : Enum
    {
        /// <summary>Gets or sets the type of event that caused the webhook to be triggered.</summary>
        public T Event { get; set; } = default!;

        /// <summary>Gets or sets the details of the message that triggered the webhook.</summary>
        public WebhookMessageItem Msg { get; set; } = default!;

        /// <summary>Gets or sets the ID of the event.</summary>
        [JsonPropertyName("_id")]
        public string Id { get; set; } = default!;

        /// <summary>Gets or sets the UNIX timestamp of the event.</summary>
        public long Ts { get; set; }

        /// <summary>Gets or sets the IP address where the event originated.</summary>
        public string Ip { get; set; } = default!;

        /// <summary>Gets or sets the browser or email client where the event occurred.</summary>
        [JsonPropertyName("User_agent")]
        public string UserAgent { get; set; } = default!;

        /// <summary>Gets or sets the URL of the sent link.</summary>
        public Uri Url { get; set; } = default!;

        /// <summary>Gets or sets the location information of the email stamp if discoverable.</summary>
        public WebhookLocationItem Location { get; set; } = default!;

        /// <summary>Gets or sets the parsed information for an open or click event.</summary>
        [JsonPropertyName("User_agent_parsed")]
        public WebhookParsedItem UserAgentParsed { get; set; } = default!;
    }

    /// <summary>
    /// Parameter-less constructor for <see cref="WebhookReturnItem"/>.
    /// </summary>
    public class WebhookReturnItem : WebhookReturnItem<EmailEventType>
    {
    }
}
