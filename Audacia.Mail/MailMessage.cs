using System;
using System.Collections.Generic;

namespace Audacia.Mail
{
    /// <summary>An email message, sent via SMTP.</summary>
    public class MailMessage
    {
        private MailFormat _format = MailFormat.Plain;
        private MailAddress _sender = new MailAddress();

        /// <summary>Initializes a new instance of the <see cref="MailMessage"/> class.</summary>
        /// <param name="recipients">An array of email addresses specifying the recipients of the message.</param>
        public MailMessage(params string[] recipients)
        {
            if (recipients == null) throw new ArgumentNullException(nameof(recipients));

            foreach (var recipient in recipients)
            {
                Recipients.Add(new MailAddress(recipient, recipient));
            }
        }

        /// <summary>Gets or sets the message subject.</summary>
        public string Subject { get; set; } = default!;

        /// <summary>Gets or sets the format for the text in this message.</summary>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        public MailFormat Format
        {
            get { return _format; }

            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _format = value;
            }
        }

        /// <summary>Gets or sets the sender addresses.</summary>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        public MailAddress Sender
        {
            get { return _sender; }

            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                if (value.Address == null) throw new ArgumentNullException(nameof(value));
                _sender = new MailAddress(value.Name, value.Address.Trim());
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
        public string Body { get; set; } = default!;

        /// <summary>
        /// Creates an empty mail message.
        /// </summary>
        /// <returns>Created <see cref="MailMessage"/>.</returns>
        public static MailMessage Create() => new MailMessage();
    }
}