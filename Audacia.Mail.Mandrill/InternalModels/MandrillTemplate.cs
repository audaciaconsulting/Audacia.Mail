namespace Audacia.Mail.Mandrill.InternalModels
{
    /// <summary>
    /// Class representing a Mandrill template.
    /// </summary>
    public class MandrillTemplate
    {
        /// <summary>
        /// Gets or sets the name of the mc:edit editable region to inject to.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the content to inject.
        /// </summary>
        public string Content { get; set; }
    }
}
