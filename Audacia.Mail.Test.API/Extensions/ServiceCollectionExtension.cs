using Audacia.Mail.Test.API.Settings;

namespace Audacia.Mail.Test.API.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddEmailServices(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        var smtpOptions = configuration.GetSection("SmtpOptions").Get<SmtpOptions>();

        if (smtpOptions == null)
        {
            throw new InvalidOperationException("Missing required configuration section: SmtpOptions.");
        }

        var senderAddress = !string.IsNullOrWhiteSpace(smtpOptions.FromEmailAddress)
            ? new EmailSenderDetails(smtpOptions.FromEmailAddress)
            : new EmailSenderDetails();

        return services
            .AddSingleton(smtpOptions)
            .AddSingleton(senderAddress)
            .AddSingleton<IMailClientFactory, MailClientFactory>();
    }
}
