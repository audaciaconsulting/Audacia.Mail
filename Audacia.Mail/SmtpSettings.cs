namespace Audacia.Mail
{
    /// <summary>
    /// Settings required to connect and authenticate with an SMTP server.
    /// </summary>
    public class SmtpSettings
    {
        /// <summary>Gets or sets the username to authenticate with.</summary>
        public string UserName { get; set; } = default!;

        /// <summary>Gets or sets the password to authenticate with.</summary>
        public string Password { get; set; } = default!;

        /// <summary>Gets or sets the host server address.</summary>
        public string Host { get; set; } = default!;

        /// <summary>Gets or sets the port through which to connect to the host.</summary>
        public int Port { get; set; }

        /// <summary>Gets or sets the address to be used when no sender is provided on the email.</summary>
        public string DefaultSender { get; set; } = default!;
    }
}