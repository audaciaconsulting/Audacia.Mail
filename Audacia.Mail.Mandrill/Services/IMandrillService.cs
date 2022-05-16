using System.Net.Http;
using System.Threading.Tasks;

namespace Audacia.Mail.Mandrill.Services
{
    /// <summary>
    /// An interface for sending requests to Mandrill.
    /// </summary>
    public interface IMandrillService
    {
        /// <summary>
        /// Method for sending requests to Mandrill to manage webhooks. 
        /// </summary>
        /// <param name="requestUrl">The URL to send the request to (may be included as part of <paramref name="content"/>.</param>
        /// <param name="content">The request content.</param>
        /// <returns>A <see cref="HttpResponseMessage"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1054:URI-like parameters should not be strings", Justification = "requestUrl may not be a complete URL")]
        Task<HttpResponseMessage> SendEmailAsync(string requestUrl, HttpContent content);
    }
}
