using System.Net.Http;
using System.Threading.Tasks;

namespace Audacia.Mail.Mandrill.Services
{
    public class MandrillService : IMandrillService
    {
        private HttpClient HttpClient { get; set; }

        public MandrillService(HttpClient client)
        {
            HttpClient = client;
        }

        public async Task<HttpResponseMessage> SendEmailAsync(string requestUri, HttpContent content)
        {
            return await HttpClient.PostAsync(requestUri, content);
        }
    }
}
