using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using Audacia.Mail.Mandrill.InternalModels.WebhookJsonDeserialisation;

namespace Audacia.Mail.Mandrill
{
    /// <summary>
    /// Extensions class for managing Mandrill webhooks.
    /// </summary>
    internal static class MandrillWebhookExtensions
    {
        /// <summary>
        /// Converts <see cref="IEnumerable{RetrievedWebhookItem}"/> to <see cref="IEnumerable{StringContent}"/> for subsequent deletion.
        /// </summary>
        /// <param name="retrievedWebhookItems">Collection of webhooks to delete.</param>
        /// <param name="apiKey">Mandrill api key.</param>
        /// <param name="options"><see cref="JsonSerializerOptions"/> for conversion of <see cref="RetrievedWebhookItem"/> to <see cref="StringContent"/>.</param>
        /// <returns>A <see cref="IEnumerable{RetrievedWebhookItem}"/>.</returns>
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
