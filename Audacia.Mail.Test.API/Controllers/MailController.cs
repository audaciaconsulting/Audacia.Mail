using Audacia.Mail.Test.API.Models;
using Audacia.Mail.Test.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Audacia.Mail.Test.API.Controllers;
[ApiController]
[Route("[controller]")]
public class MailController : Controller
{
    private readonly IMailService _mailService;
    private readonly IMailClientFactory _mailClientHandlerFactory;

    public MailController(
        IMailService mailService,
        IMailClientFactory mailClientHandlerFactory
        )
    {
        _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
        _mailClientHandlerFactory = mailClientHandlerFactory ?? throw new ArgumentNullException(nameof(mailClientHandlerFactory));
    }

    [HttpPost(Name = "SendMail")]
    public async Task<IActionResult> SendMailAsync([FromBody]SendMailRequest sendMailRequest)
    {
        var mailClient = _mailClientHandlerFactory.CreateMailClient(HttpContext.Request);

        await _mailService.SendMailAsync(sendMailRequest, mailClient);

        return Ok();
    }
}
