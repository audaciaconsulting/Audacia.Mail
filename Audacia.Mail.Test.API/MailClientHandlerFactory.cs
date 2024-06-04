using Audacia.Mail.MailKit;
using Audacia.Mail.Noop;
using Audacia.Mail.Test.API.Extensions;
using Audacia.Mail.Test.API.Settings;

namespace Audacia.Mail.Test.API;

public class MailClientHandlerFactory : IMailClientHandlerFactory
{
    private readonly SmtpOptions _smtpOptions;

    public MailClientHandlerFactory(
        SmtpOptions smtpOptions
        )
    {
        _smtpOptions = smtpOptions ?? throw new ArgumentNullException(nameof(smtpOptions));
    }

    public IMailClient CreateMailClient(HttpRequest request)
    {
        request.TryParseCustomHeaderValueIntoBoolean(
            _smtpOptions.DontForwardMessageToProviderCustomHeaderName ?? string.Empty,
            out bool dontForwardMessageToProvider
            );

        return dontForwardMessageToProvider
            ? (IMailClient)Activator.CreateInstance(typeof(NoopMailClient))
            : _smtpOptions.EmailClientType switch
            {
                EmailClientType.None => (IMailClient)Activator.CreateInstance(typeof(NoopMailClient)),
                _ => (IMailClient)Activator.CreateInstance(typeof(MailKitClient), _smtpOptions)
            };
    }
}
