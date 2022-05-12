namespace Audacia.Mail
{
	/// <summary>Specifies the format of the content of a <see cref="MailMessage"/>.</summary>
	public class MailFormat
	{
		/// <summary>Gets a <see cref="MailFormat"/> where the <see cref="MimeType"/> is set to plain text.</summary>
		public static MailFormat Plain { get; } = new MailFormat("plain");

        /// <summary>Gets a <see cref="MailFormat"/> where the <see cref="MimeType"/> is set to HTML.</summary>
        public static MailFormat Html { get; } = new MailFormat("html");

		private string MimeType { get; }

		private MailFormat(string mimeType) => MimeType = mimeType;

		/// <inheritdoc />
		public override string ToString() => MimeType;
	}
}