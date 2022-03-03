using System;

namespace Audacia.Mail.Mandrill.InternalModels
{
    internal class SendMessageRequest
    {
        /// <summary>
        /// Gets or sets the Api Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the information to send
        /// </summary>
        public MailMessage Message { get; set; }

        /// <summary>
        /// Enable a background sending mode that is optimized for bulk sending.
        /// Defaults to false for messages with less than 10 recipients
        /// </summary>
        public bool Async { get; set; }

        /// <summary>
        /// The name of the dedicated ip pool that should be used to send the message.
        /// If you do not have any dedicated IPs, this parameter has no effect.
        /// If you specify a pool that does not exist, your default pool will be used instead
        /// </summary>
        public string IpPool { get; set; }

        /// <summary>
        /// When the message should be sent as a UTC timestampe in YYYY-MM-DD HH:MM:SSSS format
        /// Specifying a time in the past will send the message immediately
        /// </summary>
        public DateTime SendAt { get; set; }

        public SendMessageRequest(string apiKey, MailMessage message)
        {
            Key = apiKey;
            Message = message;
            Async = true;
            SendAt = DateTime.Now.AddMinutes(-1);
        }
    }
}
