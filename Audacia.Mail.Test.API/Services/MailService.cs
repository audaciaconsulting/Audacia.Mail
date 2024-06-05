using Audacia.Mail.Test.API.Models;
using Audacia.Mail.Test.API.Settings;

namespace Audacia.Mail.Test.API.Services;

public class MailService : IMailService
{
    private readonly EmailSenderDetails _senderDetails;

    public MailService(
        EmailSenderDetails senderDetails
        )
    {
        _senderDetails = senderDetails ?? throw new ArgumentNullException(nameof(senderDetails));
    }

    public async Task SendMailAsync(SendMailRequest request, IMailClient mailClient)
    {
        var email = new MailMessage()
        {
            Sender = _senderDetails.Address,
            Subject = request?.Subject ?? string.Empty,
            Body = request?.Message ?? string.Empty,
            Format = MailFormat.Html
        };

        await mailClient.SendAsync(email);
    }
}
