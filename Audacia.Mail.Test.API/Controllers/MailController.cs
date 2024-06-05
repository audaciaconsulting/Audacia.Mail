using Audacia.Mail.Test.API.Models;
using Audacia.Mail.Test.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Audacia.Mail.Test.API.Controllers;
[ApiController]
[Route("[controller]")]
public class MailController : Controller
{
    private readonly IMailService _mailService;
    private readonly IMailClientFactory _mailClientFactory;

    public MailController(
        IMailService mailService,
        IMailClientFactory mailClientFactory
        )
    {
        _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
        _mailClientFactory = mailClientFactory ?? throw new ArgumentNullException(nameof(mailClientFactory));
    }

    [HttpPost(Name = "SendMail")]
    public async Task<IActionResult> SendMailAsync([FromBody]SendMailRequest sendMailRequest)
    {
        var mailClient = _mailClientFactory.CreateMailClient(HttpContext.Request);

        await _mailService.SendMailAsync(sendMailRequest, mailClient);

        return Ok();
    }
}
