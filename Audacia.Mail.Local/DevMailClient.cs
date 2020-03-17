﻿using System;
using System.Threading.Tasks;
using Audacia.Mail.MailKit;

namespace Audacia.Mail.Local
{
	/// <summary>Sends mail to the local machine for testing purposes.</summary>
	public class DevMailClient : MailKitClient
	{
		/// <summary>The type of local SMTP server to be used. This will automatically be installed using chocolatey and started with the application.</summary>
		public ServerType ServerType { get; }

        /// <summary>Initializes a new instance of the <see cref="DevMailClient"/> class.</summary>
		public DevMailClient(ServerType serverType, int port = 25, string defaultSender = null)
			: base(GetSettings(port))
		{
			ServerType = serverType;
			DefaultSender = defaultSender;

			if (serverType == ServerType.Smtp4dev)
			{
				UserName = Guid.NewGuid().ToString();
				Password = Guid.NewGuid().ToString();
			}
		}

		private static SmtpSettings GetSettings(int port)
		{
			return new SmtpSettings
			{
				Host = "localhost",
				Port = port
			};
		}

		/// <inheritdoc />
		public override async Task SendAsync(MailMessage message)
		{
			switch (ServerType)
			{
				case ServerType.None:
					break;
				case ServerType.Papercut:
					await Launcher.LaunchAsync("papercut").ConfigureAwait(false);
					break;
				case ServerType.Smtp4dev:
					await Launcher.LaunchAsync("smtp4dev").ConfigureAwait(false);
					break;
				default: throw new NotSupportedException("Specified server type is not implemented.");
			}

			await base.SendAsync(message).ConfigureAwait(false);
		}
	}
}