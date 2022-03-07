using System;
using System.Collections.Generic;

namespace Audacia.Mail.Mandrill.InternalModels.WebhookJsonDeserialisation
{
    internal class RetrievedWebhookItem
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the creation time.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the last time this was sent.
        /// </summary>
        public DateTime LastSentAt { get; set; }

        /// <summary>
        /// Gets or sets the number of batches sent.
        /// </summary>
        public int BatchesSent { get; set; }

        /// <summary>
        /// Gets or sets the number of events sent.
        /// </summary>
        public int EventsSent { get; set; }

        /// <summary>
        /// Gets or sets the auth key.
        /// </summary>
        public string AuthKey { get; set; }

        /// <summary>
        /// Gets or sets the last error thrown.
        /// </summary>
        public string LastError { get; set; }

        /// <summary>
        /// Gets or sets a list of events.
        /// </summary>
        public List<string> Events { get; set; }
    }
}
