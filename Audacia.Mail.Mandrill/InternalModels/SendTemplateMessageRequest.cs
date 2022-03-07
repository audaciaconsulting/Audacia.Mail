using System;
using System.Collections.Generic;
using Audacia.Mail.Mandrill.InternalModels;
using Audacia.Mandrill.Models;

namespace Audacia.Mail.Mandrill
{
    internal class SendTemplateMessageRequest : SendMessageRequest
    {
        public SendTemplateMessageRequest(string apiKey, MandrillMailMessage message, List<MandrillTemplate> templates, string templateName, bool async) : base(apiKey, message, async)
        {
            Message = message;
            TemplateContent = templates;
            TemplateName = templateName;
            Async = async;
            SendAt = DateTime.Now.AddMinutes(-1);
        }

        public SendTemplateMessageRequest(string apiKey, MandrillMailMessage message, string templateName, bool async) : base(apiKey, message, async)
        {
            Message = message;
            TemplateContent = new List<MandrillTemplate>();
            TemplateName = templateName;
            Async = async;
            SendAt = DateTime.Now.AddMinutes(-1);
        }

        /// <summary>
        /// Gets or sets the slug of a template
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        /// Gets or sets an array of template contents to inject
        /// </summary>
        public List<MandrillTemplate> TemplateContent { get; set; }
    }
}
