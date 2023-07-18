using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Audacia.Mail.Mandrill.InternalModels.WebhookJsonDeserialisation;
using Audacia.Mail.Mandrill.Services;

namespace Audacia.Mail.Mandrill
{
    /// <summary>
    /// Class for connecting to Mandrill and setting up webhooks.
    /// </summary>
    public class MandrillWebhookProvider
    {
        private const string WebhookDescription = "Email Sent";
        private readonly MandrillOptions _options;
        private readonly IMandrillService _mandrillService;
        private static readonly EmailEventType[] _webhookTrigger = Enum.GetValues(typeof(EmailEventType)).Cast<EmailEventType>().Where(v => !v.Equals(EmailEventType.None)).ToArray();

        /// <summary>
        /// Constructor for <see cref="MandrillWebhookProvider"/>.
        /// </summary>
        /// <param name="mandrillService"><see cref="IMandrillService"/> for creating webhooks in Mandrill.</param>
        /// <param name="options"><see cref="MandrillOptions"/> for specifying configuration for the <see cref="MandrillWebhookProvider"/>.</param>
        public MandrillWebhookProvider(IMandrillService mandrillService, MandrillOptions options)
        {
            _mandrillService = mandrillService;
            _options = options;
        }

        private readonly JsonSerializerOptions _camelCaseSerialiserSettings = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Connect to Mandrill system, check if the Webhooks for sent emails are set up and create them if they are not.
        /// </summary>
        /// <param name="environmentUrl">The URL for the environment.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="environmentUrl"/> is <see langword="null"/>.</exception>
        public async Task<bool> SetUpMandrillWebhookAsync(Uri environmentUrl)
        {
            if (environmentUrl == null) throw new ArgumentNullException(nameof(environmentUrl));
            return await SetUpMandrillWebhookAsync(environmentUrl.ToString()).ConfigureAwait(false);
        }

        /// <summary>Connect to Mandrill system, check if the Webhooks for sent emails are set up and create them if they are not.</summary>
        /// /// <param name="environmentUrl">The URL for the environment.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="environmentUrl"/> is <see langword="null"/>.</exception>
        public async Task<bool> SetUpMandrillWebhookAsync(string environmentUrl)
        {
            if (string.IsNullOrWhiteSpace(environmentUrl)) throw new ArgumentNullException(nameof(environmentUrl));

            // Get list of existing webhooks
            List<RetrievedWebhookItem> fetchedWebhookList;
            using var contentList = new StringContent(JsonSerializer.Serialize(
                new
                {
                    Key = _options.ApiKey
                }, _camelCaseSerialiserSettings));

            using var mandrillResponse = await _mandrillService.SendEmailAsync("webhooks/list", contentList).ConfigureAwait(false);
            if (mandrillResponse.IsSuccessStatusCode)
            {
                var fetchedWebhookData = await mandrillResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                fetchedWebhookList = JsonSerializer.Deserialize<List<RetrievedWebhookItem>>(fetchedWebhookData) 
                    ?? new List<RetrievedWebhookItem>();
            }
            else
            {
                return false;
            }

            // Set one webhook with correct key, url and trigger conditions
            return await SetWebhookAsync(fetchedWebhookList, environmentUrl).ConfigureAwait(false);
        }

        private async Task<bool> SetWebhookAsync(List<RetrievedWebhookItem> fetchedWebhookList, string environmentUrl)
        {
            switch (fetchedWebhookList.Count)
            {
                case 0:
                    return await CreateNewWebhookAsync(environmentUrl).ConfigureAwait(false);
                case 1:
                    return await UpdateWebhookAsync(fetchedWebhookList[0].Id, environmentUrl).ConfigureAwait(false);
                default:
                    var result = await DeleteWebhooksAsync(fetchedWebhookList).ConfigureAwait(false);
                    if (result)
                    {
                        result = await CreateNewWebhookAsync(environmentUrl).ConfigureAwait(false);
                    }

                    return result;
            }
        }

        private async Task<bool> CreateNewWebhookAsync(string environmentUrl)
        {
            using var contentSend = new StringContent(JsonSerializer.Serialize(
                new
                {
                    // Create new webhook for when emails are sent
                    Key = _options.ApiKey,
                    Url = environmentUrl,
                    Description = WebhookDescription,
                    Events = _webhookTrigger
                }, _camelCaseSerialiserSettings));

            using var responseSend = await _mandrillService.SendEmailAsync("webhooks/add", contentSend).ConfigureAwait(false);
            return responseSend.IsSuccessStatusCode;
        }

        private async Task<bool> UpdateWebhookAsync(int webhookId, string environmentUrl)
        {
            // Update webhook for when emails are sent
            using var contentSend = new StringContent(JsonSerializer.Serialize(
                new
                {
                    Id = webhookId,
                    Key = _options.ApiKey,
                    Url = environmentUrl,
                    Description = WebhookDescription,
                    Events = _webhookTrigger
                }, _camelCaseSerialiserSettings));

            using var responseSend = await _mandrillService.SendEmailAsync("webhooks/update", contentSend).ConfigureAwait(false);
            return responseSend.IsSuccessStatusCode;
        }

        private async Task<bool> DeleteWebhooksAsync(IEnumerable<RetrievedWebhookItem> webhooks)
        {
            var response = true;

            // Delete each existing webhook
            foreach (var contentSend in webhooks.SelectWebhooksToDelete(_options.ApiKey, _camelCaseSerialiserSettings))
            {
                using var responseSend = await _mandrillService.SendEmailAsync("webhooks/delete", contentSend).ConfigureAwait(false);
                if (!responseSend.IsSuccessStatusCode)
                {
                    response = false;
                }
            }

            return response;
        }
    }
}
