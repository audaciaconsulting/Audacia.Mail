namespace Audacia.Mail.Mandrill
{
    public class MandrillMailAttachment
    {
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
