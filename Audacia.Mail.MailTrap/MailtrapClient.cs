using Audacia.Mail.MailKit;

namespace Audacia.Mail.MailTrap
{
	/// <summary>Sends mail using the Mailtrap SMTP endpoint.</summary>
	public class MailtrapClient : MailKitClient
	{
		private static SmtpSettings GetSettings(string username, string password, HostType hostType)
		{
			return new SmtpSettings
			{
				Host = MapHostTypes(hostType),
				UserName = username,
				Password = password,
				Port = 587
			};
		}

        /// <summary>Initializes a new instance of the <see cref="MailtrapClient"/> class.</summary>
        /// <param name="username">The username for Mailtrap.</param>
        /// <param name="password">The password for Mailtrap.</param>
        public MailtrapClient(string username, string password)
			: base(GetSettings(username, password, HostType.Test))
		{
        }

        /// <summary>Initializes a new instance of the <see cref="MailtrapClient"/> class.</summary>
        /// <param name="username">The username for Mailtrap.</param>
        /// <param name="password">The password for Mailtrap.</param>
		/// <param name="hostType">The host type for Mailtrap.</param>
        public MailtrapClient(string username, string password, HostType hostType) 
			: base(GetSettings(username, password, hostType))
		{		
		}

        /// <summary>Initializes a new instance of the <see cref="MailtrapClient"/> class.</summary>
        /// <param name="username">The username for Mailtrap.</param>
        /// <param name="password">The password for Mailtrap.</param>
        /// <param name="defaultSender">The default sender address.</param>
		public MailtrapClient(string username, string password, string defaultSender)
			: this(username, password)
		{
			DefaultSender = defaultSender;
		}

        /// <summary> Map the host type to the correct host value. </summary>
        /// <param name="hostType">Type of Mailtrap host.</param>
        /// <returns>Host value.</returns>
        private static string MapHostTypes(HostType hostType) 
		{
			return hostType switch
			{
				HostType.Test => "smtp.mailtrap.io",
				HostType.Production => "live.smtp.mailtrap.io",
				_ => "smtp.mailtrap.io"
            };
		}
	}
}