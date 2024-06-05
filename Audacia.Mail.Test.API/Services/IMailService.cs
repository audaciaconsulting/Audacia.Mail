using Audacia.Mail.Test.API.Models;

namespace Audacia.Mail.Test.API.Services;

public interface IMailService
{
    Task SendMailAsync(SendMailRequest request, IMailClient mailClient);
}