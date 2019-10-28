namespace Audacia.Mail
{
	/// <summary>Specifies the format of the content of a <see cref="MailMessage"/>.</summary>
	public class MailFormat
	{
		/// <summary>Specifies that the content of a <see cref="MailMessage"/> should be plain text.</summary>
		public static MailFormat Plain { get; } = new MailFormat("plain");

		/// <summary>Specifies that the content of a <see cref="MailMessage"/> should be HTML.</summary>
		public static MailFormat Html { get; } = new MailFormat("html");

		private string MimeType { get; }

		private MailFormat(string mimeType) => MimeType = mimeType;

		/// <inheritdoc />
		public override string ToString() => MimeType;
	}
}