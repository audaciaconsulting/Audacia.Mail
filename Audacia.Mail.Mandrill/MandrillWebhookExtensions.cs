using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using Audacia.Mail.Mandrill.InternalModels.WebhookJsonDeserialisation;

namespace Audacia.Mail.Mandrill
{
    internal static class MandrillWebhookExtensions
    {
        public static IEnumerable<StringContent> SelectWebhooksToDelete(
            this IEnumerable<RetrievedWebhookItem> retrievedWebhookItems, string apiKey,
            JsonSerializerOptions options) => retrievedWebhookItems.Select(existingWebhook => new StringContent(JsonSerializer.Serialize(
                new
            {
                existingWebhook.Id,
                Key = apiKey
            }, options)));
    }
}
