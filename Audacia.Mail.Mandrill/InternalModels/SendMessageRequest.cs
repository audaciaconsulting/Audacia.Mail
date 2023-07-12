using System;

namespace Audacia.Mail.Mandrill.InternalModels
{
    /// <summary>
    /// Base class for sending a Mandrill message.
    /// </summary>
    internal class SendMessageRequest
    {
        /// <summary>
        /// Gets or sets the Api Key.
        /// </summary>
        public string Key { get; set; } = default!;

        /// <summary>
        /// Gets or sets the information to send.
        /// </summary>
        public MandrillMailMessage Message { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable a background sending mode that is optimized for bulk sending.
        /// Defaults to false for messages with less than 10 recipients.
        /// </summary>
        public AsyncMode SendAsync { get; set; }

        /// <summary>
        /// Gets or sets the name of the dedicated ip pool that should be used to send the message.
        /// If you do not have any dedicated IPs, this parameter has no effect.
        /// If you specify a pool that does not exist, your default pool will be used instead.
        /// </summary>
        public string IpPool { get; set; } = default!;

        /// <summary>
        /// Gets or sets the SendAt <see cref="DateTime"/>
        /// When the message should be sent as a UTC timestampe in YYYY-MM-DD HH:MM:SSSS format
        /// Specifying a time in the past will send the message immediately.
        /// </summary>
        public DateTime SendAt { get; set; }

        /// <summary>
        /// Constructor for <see cref="SendMessageRequest"/>.
        /// </summary>
        /// <param name="apiKey">The api key for Mandrill.</param>
        /// <param name="message">The <see cref="MandrillMailMessage"/> to send.</param>
        /// <param name="asyncMode">A <see cref="AsyncMode"/> determining the value of <see cref="SendAsync"/>.</param>
        public SendMessageRequest(string apiKey, MandrillMailMessage message, AsyncMode asyncMode)
        {
            Key = apiKey;
            Message = message;
            SendAsync = asyncMode;
            SendAt = DateTime.Now.AddMinutes(-1);
        }
    }

    /// <summary>
    /// Options for whether to send messages asynchronously.
    /// </summary>
    internal enum AsyncMode
    {
        /// <summary>
        /// Not asynchronous.
        /// </summary>
        Disabled,

        /// <summary>
        /// Asynchronous.
        /// </summary>
        Enabled
    }
}
