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
        /// <param name="sendMessageRequest">The <see cref="SendMessageRequest"/> detailing message properties.</param>
        /// <param name="templates">A collection of <see cref="MandrillTemplate"/>s specifying content for editable sections of the template.</param>
        /// <param name="templateName">The template in Mandrill to use.</param>
        public SendTemplateMessageRequest(SendMessageRequest sendMessageRequest, List<MandrillTemplate> templates, string templateName) : base(sendMessageRequest.Key, sendMessageRequest.Message, sendMessageRequest.SendAsync)
        {
            Message = sendMessageRequest.Message;
            TemplateContent = templates;
            TemplateName = templateName;
            SendAsync = sendMessageRequest.SendAsync;
            SendAt = DateTime.Now.AddMinutes(-1);
        }

        /// <summary>
        /// Constructor for <see cref="SendTemplateMessageRequest"/>.
        /// </summary>
        /// <param name="sendMessageRequest">The <see cref="SendMessageRequest"/> detailing message properties.</param>
        /// <param name="templateName">The template in Mandrill to use.</param>
        public SendTemplateMessageRequest(SendMessageRequest sendMessageRequest, string templateName) : base(sendMessageRequest.Key, sendMessageRequest.Message, sendMessageRequest.SendAsync)
        {
            Message = sendMessageRequest.Message;
            TemplateContent = new List<MandrillTemplate>();
            TemplateName = templateName;
            SendAsync = sendMessageRequest.SendAsync;
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
