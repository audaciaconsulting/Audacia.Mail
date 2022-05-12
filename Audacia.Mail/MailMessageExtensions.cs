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
        /// Attaches the given file data to the mail message.
        /// </summary>
        /// <param name="msg">The <see cref="MailMessage"/>.</param>
        /// <param name="bytes">The content of the message as a byte array.</param>
        /// <param name="filename">The name of the file.</param>
        /// <param name="mimeType">The mime type.</param>
        /// <returns>The provided <see cref="MailMessage"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="msg"/> or <paramref name="bytes"/> is <see langword="null"/>.</exception>
        public static MailMessage Attach(this MailMessage msg, byte[] bytes, string filename, string mimeType)
        {
            if (msg == null) throw new ArgumentNullException(nameof(msg));

            if (bytes == null) throw new ArgumentNullException(nameof(bytes));

            msg.Attachments.Add(new MailAttachment(filename, mimeType, bytes));

            return msg;
        }

        /// <summary>
        /// Attaches the given file stream to the mail message.
        /// </summary>
        /// <param name="msg">The <see cref="MailMessage"/>.</param>
        /// <param name="stream">The content of the message as a stream.</param>
        /// <param name="filename">The name of the file.</param>
        /// <param name="mimeType">The mime type.</param>
        /// <returns>The provided <see cref="MailMessage"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="msg"/> or <paramref name="stream"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="stream"/> cannot be read.</exception>
        /// <exception cref="ArgumentOutOfRangeException">End of the stream reached.</exception>
        public static MailMessage Attach(this MailMessage msg, Stream stream, string filename, string mimeType)
        {
            if (msg == null) throw new ArgumentNullException(nameof(msg));

            if (stream == null) throw new ArgumentNullException(nameof(stream));

            if (!stream.CanRead)
            {
                throw new ArgumentException("Unable to read from stream.", nameof(stream));
            }

            if (stream.Length <= stream.Position)
            {
                throw new ArgumentOutOfRangeException(nameof(stream), "End of stream reached.");
            }

            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                stream.Position = 0;
                return msg.Attach(ms.ToArray(), filename, mimeType);
            }
        }

        /// <summary>
        /// Replaces the body of the given mail message with html.
        /// </summary>
        /// <param name="msg">The <see cref="MailMessage"/> to modify.</param>
        /// <param name="body">The HTML body to replace the mail message with.</param>
        /// <returns>The provided <see cref="MailMessage"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="msg"/> is <see langword="null"/>.</exception>
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
        /// <param name="msg">The <see cref="MailMessage"/> to modify.</param>
        /// <param name="body">The plain text body to replace the mail message with.</param>
        /// <returns>The provided <see cref="MailMessage"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="msg"/> is <see langword="null"/>.</exception>
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
        /// <param name="msg">The <see cref="MailMessage"/> to modify.</param>
        /// <param name="subject">The subject.</param>
        /// <returns>The provided <see cref="MailMessage"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="msg"/> is <see langword="null"/>.</exception>
        public static MailMessage WithSubject(this MailMessage msg, string subject)
        {
            if (msg == null) throw new ArgumentNullException(nameof(msg));
            msg.Subject = subject;
            return msg;
        }

        /// <summary>
        /// Replaces any existing To recipients.
        /// </summary>
        /// <param name="msg">The <see cref="MailMessage"/> to modify.</param>
        /// <param name="addresses">The recipient addresses.</param>
        /// <returns>The provided <see cref="MailMessage"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="msg"/> is <see langword="null"/>.</exception>
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
        /// <param name="msg">The <see cref="MailMessage"/> to modify.</param>
        /// <param name="recipients">The HTML body to replace the mail message with.</param>
        /// <returns>The provided <see cref="MailMessage"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="msg"/> is <see langword="null"/>.</exception>
        public static MailMessage WithTo(this MailMessage msg, IEnumerable<string> recipients)
        {
            if (msg == null) throw new ArgumentNullException(nameof(msg));

            if (recipients == null) throw new ArgumentNullException(nameof(recipients));

            msg.Recipients.Clear();
            foreach (var email in recipients)
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    continue;
                }

                msg.Recipients.Add(new MailAddress(email.Trim()));
            }

            return msg;
        }

        /// <summary>
        /// Replaces any existing CC recipients.
        /// </summary>
        /// <param name="msg">The <see cref="MailMessage"/> to modify.</param>
        /// <param name="addresses">The CC addresses.</param>
        /// <returns>The provided <see cref="MailMessage"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="msg"/> is <see langword="null"/>.</exception>
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
        /// <param name="msg">The <see cref="MailMessage"/> to modify.</param>
        /// <param name="recipients">The CC addresses.</param>
        /// <returns>The provided <see cref="MailMessage"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="msg"/> is <see langword="null"/>.</exception>
        public static MailMessage WithCc(this MailMessage msg, IEnumerable<string> recipients)
        {
            if (msg == null) throw new ArgumentNullException(nameof(msg));

            if (recipients == null) throw new ArgumentNullException(nameof(recipients));

            msg.Cc.Clear();
            foreach (var email in recipients)
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    continue;
                }

                msg.Cc.Add(new MailAddress(email.Trim()));
            }

            return msg;
        }

        /// <summary>
        /// Replaces any existing Bcc recipients.
        /// </summary>
        /// <param name="msg">The <see cref="MailMessage"/> to modify.</param>
        /// <param name="addresses">The BCC addresses.</param>
        /// <returns>The provided <see cref="MailMessage"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="msg"/> is <see langword="null"/>.</exception>
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
        /// <param name="msg">The <see cref="MailMessage"/> to modify.</param>
        /// <param name="recipients">The BCC addresses.</param>
        /// <returns>The provided <see cref="MailMessage"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="msg"/> is <see langword="null"/>.</exception>
        public static MailMessage WithBcc(this MailMessage msg, IEnumerable<string> recipients)
        {
            if (msg == null) throw new ArgumentNullException(nameof(msg));

            if (recipients == null) throw new ArgumentNullException(nameof(recipients));

            msg.Bcc.Clear();
            foreach (var email in recipients)
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    continue;
                }

                msg.Bcc.Add(new MailAddress(email.Trim()));
            }

            return msg;
        }

        /// <summary>
        /// Replaces the sender address.
        /// </summary>
        /// <param name="msg">The <see cref="MailMessage"/> to modify.</param>
        /// <param name="address">The sender address.</param>
        /// <returns>The provided <see cref="MailMessage"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="msg"/> is <see langword="null"/>.</exception>
        public static MailMessage WithSender(this MailMessage msg, string address)
        {
            if (msg == null) throw new ArgumentNullException(nameof(msg));

            msg.Sender = new MailAddress(address);

            return msg;
        }

        /// <summary>
        /// Splits the string into multiple email addresses and appends all to the collection.
        /// </summary>
        /// <param name="collection">The collection of <see cref="MailAddress"/> to add recipient addresses to.</param>
        /// <param name="addresses">The BCC addresses.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="collection"/> is <see langword="null"/>.</exception>
        public static void AppendAll(this IList<MailAddress> collection, string addresses)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            if (string.IsNullOrWhiteSpace(addresses))
            {
                return;
            }

            var recipients = addresses.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var email in recipients)
            {
                collection.Add(new MailAddress(email.Trim()));
            }
        }
    }
}
