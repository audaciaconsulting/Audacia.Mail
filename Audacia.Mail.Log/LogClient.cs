using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audacia.Mail.Log
{
    /// <summary>
    /// Logs an email either to the console, or if configured to a sink.
    /// </summary>
    public class LogClient : IMailClient
    {
        private readonly Action<string> _loggingAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogClient"/> class.
        /// </summary>
        /// <param name="loggingAction">
        /// Optional action that is called with the email
        /// if you want to log it differently that via the console.
        /// For example, you could use Serilog to log the email or you could write to a file.
        /// </param>
        public LogClient(Action<string> loggingAction)
        {
            _loggingAction = loggingAction;
        }

        /// <summary>
        /// Log an email to the console or configured sink.
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

            if (_loggingAction == null)
            {
                Console.WriteLine(logMessage);
            }
            else
            {
                _loggingAction.Invoke(logMessage);
            }

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