namespace Audacia.Mail.Log
{
    /// <summary>
    /// Logs an email to the console.
    /// </summary>
    public class ConsoleLogMailClient : LogMailClient
    {
        /// <summary>
        /// Initializes a new <see cref="ConsoleLogMailClient"/>.
        /// </summary>
        public ConsoleLogMailClient() : base(System.Console.WriteLine)
        {
        }
    }
}