using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
            var logMessage = $"Sending email: {JsonConvert.SerializeObject(message)}";

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