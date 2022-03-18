namespace Audacia.Mandrill.Models
{
    public class MandrillTemplate
    {
        /// <summary>
        /// Name of the mc:edit editable region to inject to
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The content to inject
        /// </summary>
        public string Content { get; set; }
    }
}
