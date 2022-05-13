using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Audacia.Mail.Mandrill.Services
{
    /// <summary>
    /// An implementation of <see cref="IMandrillService"/>.
    /// </summary>
    public class MandrillService : IMandrillService
    {
        private HttpClient HttpClient { get; set; }

        /// <summary>
        /// A constructor for <see cref="MandrillService"/>.
        /// </summary>
        /// <param name="client">A <see cref="HttpClient"/> to use for sending requests.</param>
        public MandrillService(HttpClient client)
        {
            HttpClient = client;
        }

        /// <summary>
        /// Sends a request to Mandrill to send an email.
        /// </summary>
        /// <param name="requestUrl">The Uri to send the request to.</param>
        /// <param name="content">The content of the HTTP request.</param>
        /// <returns>A <see cref="Task"/> representing the completed task.</returns>
        public async Task<HttpResponseMessage> SendEmailAsync(string requestUrl, HttpContent content)
        {
            return await HttpClient.PostAsync(requestUrl, content).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a request to Mandrill to send an email.
        /// </summary>
        /// <param name="requestUrl">The Uri to send the request to.</param>
        /// <param name="content">The content of the HTTP request.</param>
        /// <returns>A <see cref="Task"/> representing the completed task.</returns>
        public async Task<HttpResponseMessage> SendEmailAsync(Uri requestUrl, HttpContent content)
        {
            return await HttpClient.PostAsync(requestUrl, content).ConfigureAwait(false);
        }
    }
}
