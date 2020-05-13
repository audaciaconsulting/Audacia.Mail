using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Audacia.Mail.Noop
{
    public class NoopMailClient : IMailClient
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Task SendAsync(MailMessage message)
        {
            return Task.CompletedTask;
        }
    }
}