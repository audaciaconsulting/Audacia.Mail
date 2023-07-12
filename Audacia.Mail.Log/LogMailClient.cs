using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audacia.Mail.Log
{
    /// <summary>
    /// Logs an email to a specified sink.
    /// </summary>
    public class LogMailClient : IMailClient
    {
        private readonly Action<string> _loggingAction;

        /// <summary>
        /// Method that creates and returns a client for console logging.
        /// </summary>
        /// <returns>A <see cref="IMailClient"/> of type <see cref="ConsoleLogMailClient"/>.</returns>
        public static IMailClient Console()
        {
            return new ConsoleLogMailClient();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogMailClient"/> class.
        /// </summary>
        /// <param name="loggingAction">
        /// Action that is called to log the email.
        /// For example, you could use Serilog to log the email or you could write to a file.
        /// </param>
        public LogMailClient(Action<string> loggingAction)
        {
            _loggingAction = loggingAction;
        }

        /// <summary>
        /// Log an email using the configured action.
        /// </summary>
        /// <param name="message">The email to log.</param>
        /// <returns>A completed Task.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="message"/> is <see langword="null"/>.</exception>
        public Task SendAsync(MailMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            var logMessageBuilder = new StringBuilder();

            logMessageBuilder.AppendLine("Sending email");
            logMessageBuilder.AppendLine("Subject: ").Append(message.Subject);
            logMessageBuilder.AppendLine("Sender: ").Append(message.Sender.Address);
            logMessageBuilder.AppendLine("Recipients: ").AppendJoin(",", message.Recipients.Select(r => r.Address));
            logMessageBuilder.AppendLine("Body: ").Append(message.Body);

            var logMessage = logMessageBuilder.ToString();

            _loggingAction.Invoke(logMessage);

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