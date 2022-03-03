using System;
using Newtonsoft.Json;

namespace Audacia.Mandrill.Models.WebhookJsonDeserialisation
{
    /// <summary>An item passed in the JSON from a webhook event occurring.</summary>
    public class WebhookReturnItem<T> where T : Enum
    {
        /// <summary>The type of event that caused the webhook to be triggered.</summary>
        public T Event { get; set; }

        /// <summary>The details of the message that triggered the webhook.</summary>
        public WebhookMessageItem Msg { get; set; }

        /// <summary>The ID of the event.</summary>
        [JsonProperty("_id")]
        public string Id { get; set; }

        /// <summary>The UNIX timestamp of the event.</summary>
        public long Ts { get; set; }

        /// <summary>If relevant, the IP address where the event originated.</summary>
        public string Ip { get; set; }

        /// <summary>If relevant, the browser or email client where the event occurred.</summary>
        [JsonProperty("User_agent")]
        public string UserAgent { get; set; }

        /// <summary>The URL of the sent link.</summary>
        public string Url { get; set; }

        /// <summary>The location information of the email stamp if discoverable.</summary>
        public WebhookLocationItem Location { get; set; }

        /// <summary>Parsed information for an open or click event.</summary>
        [JsonProperty("User_agent_parsed")]
        public WebhookParsedItem UserAgentParsed { get; set; }
    }

    public class WebhookReturnItem : WebhookReturnItem<EmailEventType>
    {
    }
}
