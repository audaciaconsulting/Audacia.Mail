namespace Audacia.Mail.Mandrill
{
    public class MandrillOptions
    {
        /// <summary>
        /// Gets or sets the from email.
        /// </summary>
        public string FromEmail { get; set; }

        /// <summary>
        /// Gets or sets the from name.
        /// </summary>
        public string FromName { get; set; }

        /// <summary>
        /// Gets or sets the api key.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is async or not.
        /// </summary>
        public bool Async { get; set; }
    }
}
