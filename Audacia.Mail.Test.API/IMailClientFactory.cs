namespace Audacia.Mail.Test.API;

public interface IMailClientFactory
{
    IMailClient CreateMailClient(HttpRequest request);
}