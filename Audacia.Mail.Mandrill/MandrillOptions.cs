namespace Audacia.Mail.Mandrill
{
    /// <summary>
    /// A configuration class for <see cref="MandrillWebhookProvider"/>.
    /// </summary>
    public class MandrillOptions
    {
        /// <summary>
        /// Gets or sets the from email.
        /// </summary>
        public string FromEmail { get; set; } = default!;

        /// <summary>
        /// Gets or sets the from name.
        /// </summary>
        public string FromName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the api key.
        /// </summary>
        public string ApiKey { get; set; } = default!;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is async or not.
        /// </summary>
        public bool Async { get; set; } = default!;
    }
}
