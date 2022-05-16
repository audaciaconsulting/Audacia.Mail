namespace Audacia.Mail.Mandrill
{
    /// <summary>
    /// Class for Mandrill mail attachments.
    /// </summary>
    public class MandrillMailAttachment
    {
        /// <summary>
        /// Constructor for <see cref="MandrillMailAttachment"/>.
        /// </summary>
        /// <param name="type">MIME type of attachment.</param>
        /// <param name="name">File name of the attachment.</param>
        /// <param name="content">Content of the attachment.</param>
        public MandrillMailAttachment(string type, string name, string content)
        {
            Type = type;
            Name = name;
            Content = content;
        }

        /// <summary>
        /// Gets or sets the MIME Type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the filename.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        public string Content { get; set; }
    }
}
