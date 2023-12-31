﻿using Audacia.Mail.MailKit;

namespace Audacia.Mail.MailTrap
{
	/// <summary>Sends mail using the Mailtrap SMTP endpoint.</summary>
	public class MailtrapClient : MailKitClient
	{
		private static SmtpSettings GetSettings(string username, string password)
		{
			return new SmtpSettings
			{
				Host = "smtp.mailtrap.io",
				UserName = username,
				Password = password,
				Port = 587
			};
		}

        /// <summary>Initializes a new instance of the <see cref="MailtrapClient"/> class.</summary>
        /// <param name="username">The username for Mailtrap.</param>
        /// <param name="password">The password for Mailtrap.</param>
        public MailtrapClient(string username, string password)
			: base(GetSettings(username, password))
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
	}
}