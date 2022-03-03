using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Audacia.Mail.Mandrill.InternalModels.WebhookJsonDeserialisation;
using Newtonsoft.Json;

namespace Audacia.Mail.Mandrill
{
    internal static class MandrillWebhookExtensions
    {
        public static IEnumerable<StringContent> SelectWebhooksToDelete(
            this IEnumerable<RetrievedWebhookItem> retrievedWebhookItems, string apiKey,
            JsonSerializerSettings camelCaseSerialiserSettings) => retrievedWebhookItems.Select(existingWebhook => new StringContent(JsonConvert.SerializeObject(
                new
            {
                existingWebhook.Id,
                Key = apiKey
            }, camelCaseSerialiserSettings)));
    }
}
