namespace Audacia.Mail
{
    /// <summary>An email address; recipient of an SMTP message.</summary>
    public class MailAddress
    {
        /// <summary>Initializes a new instance of the <see cref="MailAddress"/> class.</summary>
        public MailAddress() { }

        /// <summary>Initializes a new instance of the <see cref="MailAddress"/> class.</summary>
        /// <param name="address">The address.</param>
        public MailAddress(string address)
        {
            Name = address;
            Address = address;
        }

        /// <summary>Initializes a new instance of the <see cref="MailAddress"/> class.</summary>
        /// <param name="name">The name.</param>
        /// <param name="address">The address.</param>
        public MailAddress(string name, string address)
        {
            Name = name;
            Address = address;
        }

        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; set; } = default!;

        /// <summary>Gets or sets the address.</summary>
        /// <value>The address.</value>
        public string Address { get; set; } = default!;
    }
}