using System;
using System.Threading.Tasks;

namespace Audacia.Mail.Noop
{
    /// <summary>
    /// Client for sending NOOP SMTP commands.
    /// </summary>
    public class NoopMailClient : IMailClient
    {
        /// <inheritdoc />
        public Task SendAsync(MailMessage message)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Dispose - this doesn't really do anything in this context, but its required by the interface.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="System.Net.Http.HttpMessageInvoker" /> and optionally disposes of the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to releases only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
        }
    }
}