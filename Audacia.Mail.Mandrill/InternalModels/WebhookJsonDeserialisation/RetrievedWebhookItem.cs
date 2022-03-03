using System;
using System.Collections.Generic;

namespace Audacia.Mail.Mandrill.InternalModels.WebhookJsonDeserialisation
{
    internal class RetrievedWebhookItem
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastSentAt { get; set; }

        public int BatchesSent { get; set; }

        public int EventsSent { get; set; }

        public string AuthKey { get; set; }

        public string LastError { get; set; }

        public List<string> Events { get; set; }
    }
}
