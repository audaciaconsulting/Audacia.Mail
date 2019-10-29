namespace Audacia.Mail.Local
{
	/// <summary>The type of local SMTP server to be used..</summary>
	public enum ServerType
	{
		/// <summary>Do not use a local SMTP server.</summary>
		None,

		/// <summary>Use Papercut.</summary>
		Papercut,

		/// <summary>Use smtp4dev.</summary>
		Smtp4dev
	}
}