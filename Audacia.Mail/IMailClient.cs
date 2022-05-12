using System;
using System.Threading.Tasks;

namespace Audacia.Mail
{
    /// <summary>Sends mail using the simple mail transfer protocol (SMTP).</summary>
    /// <seealso cref="System.IDisposable" />
    public interface IMailClient : IDisposable
    {
        /// <summary>Sends the specified message asynchronously.</summary>
        /// <param name="message">The message.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task SendAsync(MailMessage message);
    }
}