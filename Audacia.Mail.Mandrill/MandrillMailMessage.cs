using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Audacia.Mail.Mandrill
{
    /// <summary>
    /// Class for creating Mandrill mail messages.
    /// </summary>
    public class MandrillMailMessage
    {
        /// <summary>
        /// Constructor for <see cref="MandrillMailMessage"/>.
        /// </summary>
        /// <param name="mailMessage"><see cref="MailMessage"/> for constructing <see cref="MandrillMailMessage"/>.</param>
        public MandrillMailMessage(MailMessage mailMessage)
        {
            if (mailMessage == null) throw new ArgumentNullException(nameof(mailMessage));

            To = mailMessage.Recipients.Select(address => new MandrillMailAddress(address.Name, address.Address, "to"))
            .Concat(mailMessage.Cc.Select(address => new MandrillMailAddress(address.Name, address.Address, "cc")))
            .Concat(mailMessage.Bcc.Select(address => new MandrillMailAddress(address.Name, address.Address, "bcc")))
            .ToList();

            Attachments = mailMessage.Attachments.Select(attachment => new MandrillMailAttachment(attachment.ContentType, attachment.FileName, Convert.ToBase64String(attachment.Bytes.ToArray()))).ToList();

            FromEmail = mailMessage.Sender.Address;
            FromName = mailMessage.Sender.Name;
            Subject = mailMessage.Subject;
            if (mailMessage.Format == MailFormat.Html)
            {
                Html = mailMessage.Body;
            }
            else
            {
                Text = mailMessage.Body;
            }
        }

        /// <summary>
        /// Gets or sets list of recipients for message.
        /// </summary>
        [JsonPropertyName("to")]
        public List<MandrillMailAddress> To { get; set; }

        /// <summary>
        /// Gets or sets a list of attachments for message.
        /// </summary>
        [JsonPropertyName("attachments")]
        public List<MandrillMailAttachment> Attachments { get; set; }

        /// <summary>
        /// Gets or sets the sender email address.
        /// </summary>
        [JsonPropertyName("from_email")]
        public string FromEmail { get; set; }

        /// <summary>
        /// Gets or sets the sender name.
        /// </summary>
        [JsonPropertyName("from_name")]
        public string FromName { get; set; }

        /// <summary>
        /// Gets or sets the message subject.
        /// </summary>
        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the full HTML content to be sent.
        /// </summary>
        [JsonPropertyName("html")]
        public string Html { get; set; }

        /// <summary>
        /// Gets or sets the full text content to be sent.
        /// </summary>
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
