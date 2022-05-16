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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "AV1704:Identifier contains one or more digits in its name", Justification = "This is the name of the application.")]
        Smtp4dev
	}
}