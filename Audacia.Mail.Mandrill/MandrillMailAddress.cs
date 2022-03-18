namespace Audacia.Mail.Mandrill
{
    public class MandrillMailAddress
    {
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
        /// Recipient type.
        /// </summary>
        public string Type { get; set; }
    }
}
