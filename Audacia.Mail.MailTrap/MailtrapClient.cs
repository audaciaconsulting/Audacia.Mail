using Audacia.Mail.MailKit;

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
				Password = password
			};
		}

        /// <summary>Initializes a new instance of the <see cref="MailtrapClient"/> class.</summary>
		public MailtrapClient(string username, string password) : base(GetSettings(username, password)) { }
	}
}