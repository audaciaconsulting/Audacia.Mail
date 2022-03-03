namespace Audacia.Mandrill.Models.WebhookJsonDeserialisation
{
    /// <summary>An item holding details for the webhook link being clicked.</summary>
    public class WebhookClickDetailsItem
    {
        /// <summary>
        /// Get or sets the UNIX timestamp of the event.
        /// </summary>
        public long Ts { get; set; }

        /// <summary>
        /// Get or sets the IP address where the open occurred.
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// Get or sets the approximated geolocation of the IP where the open occurred.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Get or sets the operating system and browser that the link was opened in.
        /// </summary>
        public string Ua { get; set; }

        /// <summary>
        /// Get or sets the URL of the sent link.
        /// </summary>
        public string Url { get; set; }

    }
}
