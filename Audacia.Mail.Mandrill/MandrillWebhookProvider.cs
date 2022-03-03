using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Audacia.Mail.Mandrill.InternalModels.WebhookJsonDeserialisation;
using Audacia.Mandrill.Models.WebhookJsonDeserialisation;

namespace Audacia.Mail.Mandrill
{
    public class MandrillWebhookProvider
    {
        private const string WebhookDescription = "Email Sent";
        private readonly MandrillOptions _options;
        private readonly HttpClient _client;
        private static readonly EmailEventType[] _webhookTrigger = Enum.GetValues(typeof(EmailEventType)).Cast<EmailEventType>().Where(v => !v.Equals(EmailEventType.None)).ToArray();

        public MandrillWebhookProvider(HttpClient client, MandrillOptions options)
        {
            _client = client;
            _options = options;
        }

        private readonly JsonSerializerSettings _snakeCaseSerialiserSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() }
        };

        private readonly JsonSerializerSettings _camelCaseSerialiserSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() },
            Formatting = Formatting.Indented,
            Converters = new JsonConverter[] { new StringEnumConverter(new CamelCaseNamingStrategy()) }
        };

        /// <summary> Connect to Mandrill system, check if the Webhooks for sent emails are set up and create them if they are not </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "ACL1002:Member or local function contains too many statements", Justification = "Setting up a web hook takes many statements.")]
        public async Task<bool> SetUpMandrillWebhookAsync(string environmentUrl)
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Get list of existing webhooks
            List<RetrievedWebhookItem> fetchedWebhookList;
            using (var contentList = new StringContent(JsonConvert.SerializeObject(
                new
                {
                    Key = _options.ApiKey
                }, _camelCaseSerialiserSettings)))
            using (var mandrillResponse = await _client.PostAsync("webhooks/list", contentList))
            {
                if (mandrillResponse.IsSuccessStatusCode)
                {
                    var fetchedWebhookData = await mandrillResponse.Content.ReadAsStringAsync();
                    fetchedWebhookList = JsonConvert.DeserializeObject<List<RetrievedWebhookItem>>(
                        fetchedWebhookData,
                        _snakeCaseSerialiserSettings);
                }
                else
                {
                    return false;
                }

                bool result;

                // Set one webhook with correct key, url and trigger conditions
                switch (fetchedWebhookList.Count)
                {
                    case 0:
                        result = await CreateNewWebhookAsync(environmentUrl);
                        break;
                    case 1:
                        result = await UpdateWebhookAsync(fetchedWebhookList[0].Id, environmentUrl);
                        break;
                    default:
                        result = await DeleteWebhooksAsync(fetchedWebhookList);
                        if (result)
                        {
                            result = await CreateNewWebhookAsync(environmentUrl);
                        }

                        break;
                }

                return result;
            }
        }

        private async Task<bool> CreateNewWebhookAsync(string environmentUrl)
        {
            using (var contentSend = new StringContent(JsonConvert.SerializeObject(
                new
                {
                    // Create new webhook for when emails are sent
                    Key = _options.ApiKey,
                    Url = environmentUrl,
                    Description = WebhookDescription,
                    Events = _webhookTrigger
                }, _camelCaseSerialiserSettings)))
            using (var responseSend = await _client.PostAsync("webhooks/add", contentSend))
            {
                return responseSend.IsSuccessStatusCode;
            }
        }

        private async Task<bool> UpdateWebhookAsync(int webhookId, string environmentUrl)
        {
            // Update webhook for when emails are sent
            using (var contentSend = new StringContent(JsonConvert.SerializeObject(
                new
                {
                    Id = webhookId,
                    Key = _options.ApiKey,
                    Url = environmentUrl,
                    Description = WebhookDescription,
                    Events = _webhookTrigger
                }, _camelCaseSerialiserSettings)))
            using (var responseSend = await _client.PostAsync("webhooks/update", contentSend))
            {
                return responseSend.IsSuccessStatusCode;
            }
        }

        private async Task<bool> DeleteWebhooksAsync(IEnumerable<RetrievedWebhookItem> webhooks)
        {
            var response = true;

            // Delete each existing webhook
            foreach (var contentSend in webhooks.SelectWebhooksToDelete(_options.ApiKey, _camelCaseSerialiserSettings))
            {
                using (var responseSend = await _client.PostAsync("webhooks/delete", contentSend))
                {
                    if (!responseSend.IsSuccessStatusCode)
                    {
                        response = false;
                    }
                }
            }

            return response;
        }
    }
}
