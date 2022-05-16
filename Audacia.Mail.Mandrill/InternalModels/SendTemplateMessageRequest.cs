using System;
using System.Collections.Generic;

namespace Audacia.Mail.Mandrill.InternalModels
{
    /// <summary>
    /// Class for triggering a template message to be sent.
    /// </summary>
    internal class SendTemplateMessageRequest : SendMessageRequest
    {
        /// <summary>
        /// Constructor for <see cref="SendTemplateMessageRequest"/>.
        /// </summary>
        /// <param name="apiKey">The api key for Mandrill.</param>
        /// <param name="message">The <see cref="MandrillMailMessage"/> detailing message properties.</param>
        /// <param name="templates">A collection of <see cref="MandrillTemplate"/>s specifying content for editable sections of the template.</param>
        /// <param name="templateName">The template in Mandrill to use.</param>
        /// <param name="asyncMode">Indicates whether to send the message asynchronously or not.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "ACL1003:Signature contains too many parameters", Justification = "Would be a breaking change to package to change parameters.")]
        public SendTemplateMessageRequest(string apiKey, MandrillMailMessage message, List<MandrillTemplate> templates, string templateName, AsyncMode asyncMode) : base(apiKey, message, asyncMode)
        {
            Message = message;
            TemplateContent = templates;
            TemplateName = templateName;
            SendAsync = asyncMode;
            SendAt = DateTime.Now.AddMinutes(-1);
        }

        /// <summary>
        /// Constructor for <see cref="SendTemplateMessageRequest"/>.
        /// </summary>
        /// <param name="apiKey">The api key for Mandrill.</param>
        /// <param name="message">The <see cref="MandrillMailMessage"/> detailing message properties.</param>
        /// <param name="templateName">The template in Mandrill to use.</param>
        /// <param name="asyncMode">Indicates whether to send the message asynchronously or not.</param>
        public SendTemplateMessageRequest(string apiKey, MandrillMailMessage message, string templateName, AsyncMode asyncMode) : base(apiKey, message, asyncMode)
        {
            Message = message;
            TemplateContent = new List<MandrillTemplate>();
            TemplateName = templateName;
            SendAsync = asyncMode;
            SendAt = DateTime.Now.AddMinutes(-1);
        }

        /// <summary>
        /// Gets or sets the slug of a template.
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        /// Gets or sets an array of template contents to inject.
        /// </summary>
        public List<MandrillTemplate> TemplateContent { get; set; }
    }
}
