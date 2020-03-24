using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Audacia.Mail.MailKit
{
    /// <summary>
    /// Sends emails using the SMTP transfer protocol. Implemented using MailKit and MimeKit.
    /// </summary>
    /// <seealso cref="IMailClient" />
    public class MailKitClient : IMailClient
    {
        /// <summary>The address to be used when no sender is provided on the email.</summary>
        public string DefaultSender { get; protected set; }

        /// <summary>the username to authenticate with.</summary>
        public string UserName { get; protected set; }

        /// <summary>The password to authenticate with.</summary>
        public string Password { get; protected set; }

        /// <summary>The host server address.</summary>
        public string Host { get; }

        /// <summary>The port through which to connect to the host.</summary>
        public int Port { get; }

        private SmtpClient _client = new SmtpClient();

        /// <summary>Initializes a new instance of the <see cref="MailKitClient"/> class.</summary>
        /// <param name="settings">The settings.</param>
        public MailKitClient(SmtpSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            Host = settings.Host;
            Port = settings.Port;
            UserName = settings.UserName;
            Password = settings.Password;
            DefaultSender = settings.DefaultSender;
        }

        /// <summary>Initializes a new instance of the <see cref="MailKitClient"/>, then connects and authenticates with the SMTP server.</summary>
        public static MailKitClient Connect(SmtpSettings settings)
        {
            var client = new MailKitClient(settings);
            client.Connect();
            return client;
        }

        /// <summary>Connect and authenticate with the SMTP server.</summary>
        public void Connect()
        {
            if (!_client.IsConnected)
            {
                _client.Connect(Host, Port, SecureSocketOptions.None);
            }

            if (!_client.IsAuthenticated && _client.AuthenticationMechanisms.Any())
            {
                _client.Authenticate(UserName, Password);
            }
        }

        /// <summary>Sends the specified message.</summary>
        /// <param name="message">The message.</param>
        public void Send(MailMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            if (string.IsNullOrWhiteSpace(message.Sender?.Address) &&
               !string.IsNullOrWhiteSpace(DefaultSender))
            {
                message.Sender = new MailAddress(DefaultSender);
            }

            var mimeMessage = CreateMimeMessage(message);

            if (!_client.IsConnected)
            {
                _client.Connect(Host, Port);
            }

            if (!_client.IsAuthenticated && _client.AuthenticationMechanisms.Any())
            {
                _client.Authenticate(UserName, Password);
            }

            _client.Send(FormatOptions.Default, mimeMessage, CancellationToken.None);
        }

        /// <summary>Sends the specified message asynchronously.</summary>
        /// <param name="message">The message.</param>
        public virtual async Task SendAsync(MailMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            if (string.IsNullOrWhiteSpace(message.Sender?.Address) &&
               !string.IsNullOrWhiteSpace(DefaultSender))
            {
                message.Sender = new MailAddress(DefaultSender);
            }

            var mimeMessage = CreateMimeMessage(message);

            if (!_client.IsConnected)
            {
                await _client.ConnectAsync(Host, Port).ConfigureAwait(false);
            }

            if (!_client.IsAuthenticated && _client.AuthenticationMechanisms.Any())
            {
                await _client.AuthenticateAsync(UserName, Password).ConfigureAwait(false);
            }

            await _client.SendAsync(FormatOptions.Default, mimeMessage, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Releases the unmanaged resources used by the <see cref="System.Net.Http.HttpMessageInvoker" /> and optionally disposes of the managed resources.</summary>
        /// <param name="disposing"><see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to releases only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _client?.Dispose();
                _client = null;
            }
        }

        private static MimeMessage CreateMimeMessage(MailMessage mailMessage)
        {
            var body = new Multipart("mixed")
            {
                new TextPart(mailMessage.Format.ToString())
                {
                    Text = mailMessage.Body ?? string.Empty
                }
            };

            foreach (var attachment in mailMessage.Attachments)
            {
                var part = new MimePart(attachment.ContentType)
                {
                    Content = new MimeContent(new MemoryStream(attachment.Bytes.ToArray())),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = attachment.FileName
                };

                body.Add(part);
            }

            var sender = new MailboxAddress(mailMessage.Sender.Name, mailMessage.Sender.Address);
            var recipients = mailMessage.Recipients.Select(s => new MailboxAddress(s.Name, s.Address));
            var message = new MimeMessage(new[] { sender }, recipients, mailMessage.Subject, body);

            foreach (var cc in mailMessage.Cc)
            {
                message.Cc.Add(new MailboxAddress(cc.Name, cc.Address));
            }

            foreach (var bcc in mailMessage.Bcc)
            {
                message.Bcc.Add(new MailboxAddress(bcc.Name, bcc.Address));
            }

            return message;
        }
    }
}