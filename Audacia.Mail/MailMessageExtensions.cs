using System;
using System.Collections.Generic;
using System.IO;

namespace Audacia.Mail
{
    /// <summary>
    /// Fluent extensions for <see cref="MailMessage" />.
    /// </summary>
    public static class MailMessageExtensions
    {
        /// <summary>
        /// Attaches the given file stream to the mail message.
        /// </summary>
        public static MailMessage Attach(this MailMessage msg, Stream stream, string filename, string mimeType)
        {
            if (msg == null) throw new ArgumentNullException(nameof(msg));

            if (stream == null) throw new ArgumentNullException(nameof(stream));

            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                msg.Attachments.Add(new MailAttachment(filename, mimeType, ms.ToArray()));
            }

            return msg;
        }

        /// <summary>
        /// Replaces the body of the given mail message with html.
        /// </summary>
        public static MailMessage WithHtml(this MailMessage msg, string body)
        {
            if (msg == null) throw new ArgumentNullException(nameof(msg));
            msg.Body = body;
            msg.Format = MailFormat.Html;
            return msg;
        }

        /// <summary>
        /// Replaces the body of the given mail message with plain text.
        /// </summary>
        public static MailMessage WithPlainText(this MailMessage msg, string body)
        {
            if (msg == null) throw new ArgumentNullException(nameof(msg));
            msg.Body = body;
            msg.Format = MailFormat.Plain;
            return msg;
        }

        /// <summary>
        /// Replaces the subject of the given mail message.
        /// </summary>
        public static MailMessage WithSubject(this MailMessage msg, string subject)
        {
            if (msg == null) throw new ArgumentNullException(nameof(msg));
            msg.Subject = subject;
            return msg;
        }

        /// <summary>
        /// Replaces any existing To recipients.
        /// </summary>
        public static MailMessage WithTo(this MailMessage msg, string addresses)
        {
            if (msg == null) throw new ArgumentNullException(nameof(msg));

            msg.Recipients.Clear();
            msg.Recipients.AppendAll(addresses);

            return msg;
        }

        /// <summary>
        /// Replaces any existing To recipients.
        /// </summary>
        public static MailMessage WithTo(this MailMessage msg, IEnumerable<string> recipients)
        {
            if (msg == null) throw new ArgumentNullException(nameof(msg));

            if (recipients == null) throw new ArgumentNullException(nameof(recipients));

            msg.Recipients.Clear();
            foreach (var email in recipients)
            {
                msg.Recipients.Add(new MailAddress(email));
            }

            return msg;
        }

        /// <summary>
        /// Replaces any existing CC recipients.
        /// </summary>
        public static MailMessage WithCc(this MailMessage msg, string addresses)
        {
            if (msg == null) throw new ArgumentNullException(nameof(msg));

            msg.Cc.Clear();
            msg.Cc.AppendAll(addresses);

            return msg;
        }

        /// <summary>
        /// Replaces any existing Cc recipients.
        /// </summary>
        public static MailMessage WithCc(this MailMessage msg, IEnumerable<string> recipients)
        {
            if (msg == null) throw new ArgumentNullException(nameof(msg));

            if (recipients == null) throw new ArgumentNullException(nameof(recipients));

            msg.Cc.Clear();
            foreach (var email in recipients)
            {
                msg.Cc.Add(new MailAddress(email));
            }

            return msg;
        }

        /// <summary>
        /// Replaces any existing Bcc recipients.
        /// </summary>
        public static MailMessage WithBcc(this MailMessage msg, string addresses)
        {
            if (msg == null) throw new ArgumentNullException(nameof(msg));

            msg.Bcc.Clear();
            msg.Bcc.AppendAll(addresses);

            return msg;
        }

        /// <summary>
        /// Replaces any existing Bcc recipients.
        /// </summary>
        public static MailMessage WithBcc(this MailMessage msg, IEnumerable<string> recipients)
        {
            if (msg == null) throw new ArgumentNullException(nameof(msg));

            if (recipients == null) throw new ArgumentNullException(nameof(recipients));

            msg.Bcc.Clear();
            foreach (var email in recipients)
            {
                msg.Bcc.Add(new MailAddress(email));
            }

            return msg;
        }

        /// <summary>
        /// Replaces the sender address.
        /// </summary>
        public static MailMessage WithSender(this MailMessage msg, string address)
        {
            if (msg == null) throw new ArgumentNullException(nameof(msg));

            msg.Sender = new MailAddress(address);

            return msg;
        }

        /// <summary>
        /// Splits the string into multiple email addresses and appends all to the collection.
        /// </summary>
        public static void AppendAll(this IList<MailAddress> collection, string addresses)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            if (string.IsNullOrWhiteSpace(addresses)) return;

            var recipients = addresses.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var email in recipients)
            {
                collection.Add(new MailAddress(email));
            }
        }
    }
}
