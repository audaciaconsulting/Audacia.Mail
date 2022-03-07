using System;
using System.Collections.Generic;
using System.Linq;

namespace Audacia.Mail.Mandrill
{
    public class MandrillMailMessage
    {
        public MandrillMailMessage(MailMessage mailMessage)
        {
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
        public List<MandrillMailAddress> To { get; set; }

        /// <summary>
        /// Gets or sets a list of attachments for message.
        /// </summary>
        public List<MandrillMailAttachment> Attachments { get; set; }

        /// <summary>
        /// Gets or sets the sender email address.
        /// </summary>
        public string FromEmail { get; set; }

        /// <summary>
        /// Gets or sets the sender name.
        /// </summary>
        public string FromName { get; set; }

        /// <summary>
        /// Gets or sets the message subject.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the full HTML content to be sent.
        /// </summary>
        public string Html { get; set; }

        /// <summary>
        /// Gets or sets the full text content to be sent.
        /// </summary>
        public string Text { get; set; }
    }
}
