using System.Net.Http;
using System.Threading.Tasks;

namespace Audacia.Mail.Mandrill.Services
{
    public interface IMandrillService
    {
        Task<HttpResponseMessage> SendEmailAsync(string requestUri, HttpContent content);
    }
}
