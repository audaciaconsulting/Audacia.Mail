using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Audacia.Mail.Mandrill.InternalModels;
using Audacia.Mandrill.Models;

namespace Audacia.Mail.Mandrill
{
    /// <summary>
    /// Sends mail using the Mail Chimp/Mandrill API.
    /// </summary>
    public class MandrillClient : IMailClient
    {
        private const string _outputFormat = ".json";
        private readonly MandrillOptions _options;
        private readonly HttpClient _client;
        private readonly MandrillWebhookProvider _webhookProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MandrillClient"/> class.
        /// </summary>
        /// <param name="options">The mandrill specific options needed to set up Mandrill client.</param>
        public MandrillClient(MandrillOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options), "You need to setup MandrillOptions");
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://mandrillapp.com/api/1.0/")
            };
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _webhookProvider = new MandrillWebhookProvider(_client, _options);
        }

        /// <inheritdoc />
        public async Task SendAsync(MailMessage message)
        {
            var messageRequest = new SendMessageRequest(_options.ApiKey, message, _options.Async);
            await SendPostRequestAsync("messages/send", messageRequest);
        }

        /// <summary>
        /// Sends a post request to the Mandrill api via <see cref="_client"/>.
        /// </summary>
        /// <typeparam name="T">Any type you want to create a json string from.</typeparam>
        /// <param name="url">The url specified for your Mandrill subscription.</param>
        /// <param name="obj">The object to get a json string from.</param>
        private async Task<HttpResponseMessage> SendPostRequestAsync<T>(string url, T obj)
        {
            var test = MandrillHelper.GetJsonString(obj);
            var response = await _client.PostAsync(url, new StringContent(test));
            return response;
        }

        /// <summary>
        /// Sends a pre built template message to a particular url.
        /// </summary>
        /// <param name="message">The message that needs to be sent.</param>
        /// <param name="templateName">The name of the template.</param>
        /// <param name="templates">The list of <see cref="MandrillTemplate"/>'s to send.</param>
        public async Task<bool?> SendTemplateMessageAsync(MailMessage message, string templateName, List<MandrillTemplate> templates = null)
        {
            var messageRequest = templates != null ?
                new SendTemplateMessageRequest(_options.ApiKey, message, templates, templateName, _options.Async) :
                new SendTemplateMessageRequest(_options.ApiKey, message, templateName, _options.Async);
            using (var result = await SendPostRequestAsync($"messages/send-template{_outputFormat}", messageRequest))
            {
                return result.IsSuccessStatusCode;
            }
        }

        /// <summary> 
        /// Connect to Mandrill system, check if the Webhooks for sent emails are set up and create them if they are not 
        /// </summary>
        public Task<bool> SetUpMandrillWebhookAsync(string environmentUrl) => _webhookProvider.SetUpMandrillWebhookAsync(environmentUrl);

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="System.Net.Http.HttpMessageInvoker" /> and optionally disposes of the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to releases only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing) 
        {
            if (disposing)
            {
                _client.Dispose();
            }
        }
    }
}
