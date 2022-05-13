namespace Audacia.Mail.Mandrill
{
    /// <summary>
    /// Class representing a Mandrill mail address.
    /// </summary>
    public class MandrillMailAddress
    {
        /// <summary>
        /// Constructor for <see cref="MandrillMailAddress"/>.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="email">Email.</param>
        /// <param name="type">Type.</param>
        public MandrillMailAddress(string name, string email, string type)
        {
            Name = name;
            Email = email;
            Type = type;
        }

        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the recipient type.
        /// </summary>
        public string Type { get; set; }
    }
}
