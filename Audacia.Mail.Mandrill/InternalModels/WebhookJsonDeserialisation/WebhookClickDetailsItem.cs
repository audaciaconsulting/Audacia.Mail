using System;

namespace Audacia.Mail.Mandrill.InternalModels.WebhookJsonDeserialisation
{
    /// <summary>An item holding details for the webhook link being clicked.</summary>
    public class WebhookClickDetailsItem
    {
        /// <summary>
        /// Gets or sets the UNIX timestamp of the event.
        /// </summary>
        public long Ts { get; set; }

        /// <summary>
        /// Gets or sets the IP address where the open occurred.
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// Gets or sets the approximated geolocation of the IP where the open occurred.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the operating system and browser that the link was opened in.
        /// </summary>
        public string Ua { get; set; }

        /// <summary>
        /// Gets or sets the URL of the sent link.
        /// </summary>
        public Uri Url { get; set; }
    }
}
