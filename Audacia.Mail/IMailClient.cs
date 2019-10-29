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
        Task SendAsync(MailMessage message);
    }
}