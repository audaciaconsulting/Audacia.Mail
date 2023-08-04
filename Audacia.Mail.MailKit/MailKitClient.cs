using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        /// <summary>Gets or sets the address to be used when no sender is provided on the email.</summary>
        public string DefaultSender { get; protected set; } = default!;

        /// <summary>Gets or sets the username to authenticate with.</summary>
        public string UserName { get; protected set; } = default!;

        /// <summary>Gets or sets the password to authenticate with.</summary>
        public string Password { get; protected set; } = default!;

        /// <summary>Gets the host server address.</summary>
        public string Host { get; }

        /// <summary>Gets the port through which to connect to the host.</summary>
        public int Port { get; }

        private SmtpClient? _client = new SmtpClient();

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
        /// <param name="settings">Settings required to connect to SMTP server.</param>
        /// <returns>A <see cref="MailKitClient"/>.</returns>
        public static MailKitClient Connect(SmtpSettings settings)
        {
            var client = new MailKitClient(settings);
            client.ConnectToClient();

            return client;
        }

        /// <summary>ConnectToSmtpServer and authenticate with the SMTP server.</summary>
        /// <exception cref="InvalidOperationException">Throws is <see cref="_client"/> is null.</exception>
        private void ConnectToClient()
        {
            if (_client == null)
            {
                throw new InvalidOperationException($"Client is null and therefore cannot connect.");
            }

            if (!_client.IsConnected)
            {
                _client.Connect(Host, Port, SecureSocketOptions.None);
            }

            if (!_client.IsAuthenticated && _client.AuthenticationMechanisms.Any())
            {
                _client.Authenticate(UserName, Password);
            }
        }

        /// <summary>ConnectToSmtpServer and authenticate with the SMTP server asynchronously.</summary>
        /// <returns>A <see cref="Task"/> representing the completion of the action.</returns>
        /// <exception cref="InvalidOperationException">Throws is <see cref="_client"/> is null.</exception>
        private async Task ConnectToClientAsync()
        {
            if (_client == null)
            {
                throw new InvalidOperationException($"Client is null and therefore cannot connect.");
            }

            if (!_client.IsConnected)
            {
                await _client.ConnectAsync(Host, Port).ConfigureAwait(false);
            }

            if (!_client.IsAuthenticated && _client.AuthenticationMechanisms.Any())
            {
                await _client.AuthenticateAsync(UserName, Password).ConfigureAwait(false);
            }
        }

        /// <summary>Sends the specified message asynchronously.</summary>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException"><paramref name="message"/> is <see langword="null"/>.</exception>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public virtual async Task SendAsync(MailMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            if (string.IsNullOrWhiteSpace(message.Sender?.Address) &&
               !string.IsNullOrWhiteSpace(DefaultSender))
            {
                message.Sender = new MailAddress(DefaultSender);
            }

            var mimeMessage = CreateMimeMessage(message);

            await ConnectToClientAsync().ConfigureAwait(false);
            await _client!.SendAsync(FormatOptions.Default, mimeMessage, CancellationToken.None).ConfigureAwait(false);
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

            AddAttachments(body, mailMessage.Attachments);

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

        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP004:Don't ignore created IDisposable", Justification = "Stream needs to be initialised inline to prevent an ObjectDisposedException being thrown when the email is sent.")]
        private static void AddAttachments(Multipart body, IList<MailAttachment> attachments)
        {
            foreach (var attachment in attachments)
            {
                var part = new MimePart(attachment.ContentType)
                {
                    // Note: The memory stream must be created inline to prevent an ObjectDisposedException being thrown when the email is sent by the mail client.
                    Content = new MimeContent(new MemoryStream(attachment.Bytes.ToArray())),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = attachment.FileName
                };

                body.Add(part);
            }
        }
    }
}