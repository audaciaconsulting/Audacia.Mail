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
            _smtpOptions.EmailDryRunHeaderName ?? string.Empty,
            out bool emailDryRun
            );

        return emailDryRun
            ? new NoopMailClient()
            : _smtpOptions.EmailClientType switch
            {
                EmailClientType.None => new NoopMailClient(),
                _ => new MailKitClient(_smtpOptions)
            };
    }
}
