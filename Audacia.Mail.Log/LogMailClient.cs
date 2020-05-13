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
        public Task SendAsync(MailMessage message)
        {
            var logMessageBuilder = new StringBuilder();

            logMessageBuilder.AppendLine("Sending email");
            logMessageBuilder.AppendLine($"Subject: {message.Subject}");
            logMessageBuilder.AppendLine($"Sender: {message.Sender.Address}");
            logMessageBuilder.AppendLine($"Recipients: {string.Join(",", message.Recipients.Select(r => r.Address))}");
            logMessageBuilder.AppendLine($"Body: {message.Body}");

            var logMessage = logMessageBuilder.ToString();

            _loggingAction.Invoke(logMessage);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Dispose - this doesn't really do anything in this context, but its required by the interface.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}