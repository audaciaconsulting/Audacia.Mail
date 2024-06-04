namespace Audacia.Mail.Test.API;

public interface IMailClientHandlerFactory
{
    IMailClient CreateMailClient(HttpRequest request);
}