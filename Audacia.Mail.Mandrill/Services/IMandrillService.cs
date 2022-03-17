using System.Net.Http;

namespace Audacia.Mail.Mandrill.Services
{
    public interface IMandrillService
    {
        HttpClient HttpClient { get; set; }
    }
}
