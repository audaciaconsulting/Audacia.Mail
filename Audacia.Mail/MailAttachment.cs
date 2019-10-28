using System.Collections.Generic;

namespace Audacia.Mail
{
    /// <summary>
    /// An attachment on an email message.
    /// </summary>
    public class MailAttachment
    {
        /// <summary>Initializes a new instance of the <see cref="MailAttachment"/> class.</summary>
        public MailAttachment() { }

        /// <summary>Initializes a new instance of the <see cref="MailAttachment"/> class.</summary>
        /// <param name="fileName">Name of the attached file.</param>
        /// <param name="contentType">MIME content type of the attached file.</param>
        /// <param name="bytes">The file bytes.</param>
        public MailAttachment(string fileName, string contentType, byte[] bytes)
        {
            FileName = fileName;
            ContentType = contentType;
            Bytes = bytes;
        }

        /// <summary>Gets or sets the name of the attached file.</summary>
        /// <value>The name of the file.</value>
        public string FileName { get; set; }

        /// <summary>Gets or sets the MIME content type of the file.</summary>
        /// <value>The type of the content.</value>
        public string ContentType { get; set; }

        /// <summary>Gets or sets the file bytes.</summary>
        /// <value>The bytes.</value>
        public IList<byte> Bytes { get; }
    }
}