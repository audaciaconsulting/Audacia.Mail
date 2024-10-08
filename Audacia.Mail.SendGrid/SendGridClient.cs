﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SendGrid.Helpers.Mail;

namespace Audacia.Mail.SendGrid
{
    /// <summary>Sends mail using the SendGrid API.</summary>
    public class SendGridClient : IMailClient
    {
        /// <summary>Initializes a new instance of the <see cref="SendGridClient"/> class.</summary>
        /// <param name="apiKey">The api key for making requests to SendGrid.</param>
        public SendGridClient(string apiKey)
        {
            _client = new global::SendGrid.SendGridClient(apiKey);
        }

        /// <summary>Initializes a new instance of the <see cref="SendGridClient"/> class.</summary>
        /// <param name="apiKey">The api key for making requests to SendGrid.</param>
        /// <param name="defaultSender">The default sender address.</param>
        public SendGridClient(string apiKey, string defaultSender)
            : this(apiKey)
        {
            _defaultSender = defaultSender;
        }

        private readonly global::SendGrid.SendGridClient _client;
        private readonly string _defaultSender = default!;

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Releases the unmanaged resources used by the <see cref="System.Net.Http.HttpMessageInvoker" /> and optionally disposes of the managed resources.</summary>
        /// <param name="disposing"><see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to releases only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing) { }

        /// <inheritdoc />
        public Task SendAsync(MailMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            if (string.IsNullOrWhiteSpace(message.Sender?.Address) &&
               !string.IsNullOrWhiteSpace(_defaultSender))
            {
                message.Sender = new MailAddress(_defaultSender);
            }

            var sendGridMessage = new SendGridMessage
            {
                Contents = new List<Content>
                {
                    new Content { Type = $"text/{message.Format}", Value = message.Body }
                },
                From = new EmailAddress(message.Sender?.Address, message.Sender?.Name),
                Subject = message.Subject
            };

            if (message.Attachments.Any())
            {
                sendGridMessage.Attachments = message.Attachments.Select(a => new Attachment
                {
                    Content = Convert.ToBase64String(a.Bytes.ToArray())
                }).ToList();
            }

            AddRecipients(sendGridMessage, message);

            return _client.SendEmailAsync(sendGridMessage);
        }

        private static void AddRecipients(SendGridMessage sendGridMessage, MailMessage mailMessage) 
        {
            var recipients = mailMessage.Recipients.Select(r => new EmailAddress
            {
                Name = r.Name,
                Email = r.Address
            }).ToList();

            sendGridMessage.AddTos(recipients);

            var cc = mailMessage.Cc.Select(b => new EmailAddress
            {
                Name = b.Name,
                Email = b.Address
            }).ToList();

            if (cc.Any()) 
            {
                sendGridMessage.AddCcs(cc);
            }

            var bcc = mailMessage.Bcc.Select(b => new EmailAddress
            {
                Name = b.Name,
                Email = b.Address
            }).ToList();

            if (bcc.Any()) 
            {
                sendGridMessage.AddBccs(bcc);
            }
        }
    }
}