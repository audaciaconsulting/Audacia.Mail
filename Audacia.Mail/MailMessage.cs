using System;
using System.Collections.Generic;
using static Audacia.Mail.MailMessageExtensions;

namespace Audacia.Mail
{
    /// <summary>An email message, sent via SMTP.</summary>
    public class MailMessage
    {
        private MailFormat _format = MailFormat.Plain;
        private MailAddress _sender = new MailAddress();

        /// <summary>Initializes a new instance of the <see cref="MailMessage"/> class.</summary>
        public MailMessage(params string[] recipients)
        {
            foreach (var recipient in recipients)
            {
                Recipients.Add(new MailAddress(recipient, recipient));
            }
        }

        /// <summary>Gets or sets the message subject.</summary>
        public string Subject { get; set; }

        /// <summary>The format for the text in this message.</summary>
        public MailFormat Format
        {
            get { return _format; }

            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _format = value;
            }
        }

        /// <summary>Gets the sender addresses.</summary>
        public MailAddress Sender
        {
            get { return _sender; }

            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                if (value.Address == null) throw new ArgumentNullException(nameof(value.Address));
                _sender = new MailAddress(value.Address.Trim());
            }
        }

        /// <summary>Gets the recipient addresses.</summary>
        /// <value>The recipients.</value>
        public IList<MailAddress> Recipients { get; } = new List<MailAddress>();

        /// <summary>Gets the addresses to be CC'd into the email.</summary>
        public IList<MailAddress> Cc { get; } = new List<MailAddress>();

        /// <summary>Gets the addresses to be BCC'd into the email.</summary>
        public IList<MailAddress> Bcc { get; } = new List<MailAddress>();

        /// <summary>Gets the attachments.</summary>
        public IList<MailAttachment> Attachments { get; } = new List<MailAttachment>();

        /// <summary>Gets or sets the body of the email message.</summary>
        /// <value>The body.</value>
        public string Body { get; set; }

        /// <summary>
        /// Creates an empty mail message.
        /// </summary>
        public static MailMessage Create() => new MailMessage();
    }
}