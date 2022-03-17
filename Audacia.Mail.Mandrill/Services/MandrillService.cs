using System.Net.Http;

namespace Audacia.Mail.Mandrill.Services
{
    public class MandrillService : IMandrillService
    {
        public HttpClient HttpClient { get; set; }

        public MandrillService(HttpClient client)
        {
            HttpClient = client;
        }
    }
}
