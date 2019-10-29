namespace Audacia.Mail
{
    /// <summary>
    /// Settings required to connect and authenticate with an SMTP server.
    /// </summary>
    public class SmtpSettings
    {
        /// <summary>the username to authenticate with.</summary>
        public string UserName { get; set; }

        /// <summary>The password to authenticate with.</summary>
        public string Password { get; set; }

        /// <summary>The host server address.</summary>
        public string Host { get; set; }

        /// <summary>The port through which to connect to the host.</summary>
        public int Port { get; set; }
    }
}