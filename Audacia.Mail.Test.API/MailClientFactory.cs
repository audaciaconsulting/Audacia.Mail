using Audacia.Mail.MailKit;
using Audacia.Mail.Noop;
using Audacia.Mail.Test.API.Extensions;
using Audacia.Mail.Test.API.Settings;

namespace Audacia.Mail.Test.API;

public class MailClientFactory : IMailClientFactory
{
    private readonly SmtpOptions _smtpOptions;

    public MailClientFactory(
        SmtpOptions smtpOptions
        )
    {
        _smtpOptions = smtpOptions ?? throw new ArgumentNullException(nameof(smtpOptions));
    }

    public IMailClient CreateMailClient(HttpRequest request)
    {
        request.TryParseCustomHeaderValueIntoBoolean(
            _smtpOptions.DontSendEmailHeaderName ?? string.Empty,
            out bool dontSendEmail
            );

        return dontSendEmail
            ? new NoopMailClient()
            : _smtpOptions.EmailClientType switch
            {
                EmailClientType.None => new NoopMailClient(),
                _ => new MailKitClient(_smtpOptions)
            };
    }
}
