using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Audacia.Mail.Mandrill;
using Audacia.Mail.Mandrill.InternalModels;
using Xunit;

namespace Audacia.Mail.Tests.Mandrill;

/// <summary>
/// Checks the JSON that <see cref="MandrillClient"/> sends to the Mandrill API. Requests go to a <see cref="FakeMandrillService"/>, never to Mandrill.
/// </summary>
public sealed class MandrillClientTests
{
    private const string ApiKey = "test-api-key-not-real";

    private readonly FakeMandrillService _service = new();

    [Fact]
    public async Task SendAsync_HtmlMessage_PostsToMessagesSendEndpoint()
    {
        using var client = CreateClient();

        await client.SendAsync(CreateMessage(MailFormat.Html, "<p>Welcome aboard.</p>"));

        Assert.Equal("messages/send", Assert.Single(_service.Requests).Url);
    }

    [Fact]
    public async Task SendAsync_HtmlMessage_SerialisesApiKeySenderSubjectAndHtmlBody()
    {
        using var client = CreateClient();

        await client.SendAsync(CreateMessage(MailFormat.Html, "<p>Welcome aboard.</p>"));

        var json = Assert.Single(_service.Requests).Json();
        var message = json.GetProperty("message");
        Assert.Equal(ApiKey, json.GetProperty("key").GetString());
        Assert.Equal("priya.sharma@example.com", message.GetProperty("from_email").GetString());
        Assert.Equal("Priya Sharma", message.GetProperty("from_name").GetString());
        Assert.Equal("Welcome to Audacia", message.GetProperty("subject").GetString());
        Assert.Equal("<p>Welcome aboard.</p>", message.GetProperty("html").GetString());
    }

    [Fact]
    public async Task SendAsync_PlainMessage_SerialisesBodyAsText()
    {
        using var client = CreateClient();

        await client.SendAsync(CreateMessage(MailFormat.Plain, "Welcome aboard."));

        var message = Assert.Single(_service.Requests).Json().GetProperty("message");
        Assert.Equal("Welcome aboard.", message.GetProperty("text").GetString());
    }

    [Fact]
    public async Task SendAsync_WithCcAndBcc_SerialisesEachRecipientWithItsType()
    {
        using var client = CreateClient();
        var mailMessage = CreateMessage(MailFormat.Plain, "Minutes attached.");
        mailMessage.Cc.Add(new MailAddress("Aisha Khan", "aisha.khan@example.com"));
        mailMessage.Bcc.Add(new MailAddress("Records", "records@example.com"));

        await client.SendAsync(mailMessage);

        var recipients = Assert.Single(_service.Requests).Json().GetProperty("message").GetProperty("to").EnumerateArray()
            .Select(recipient => (
                Email: recipient.GetProperty("email").GetString() ?? string.Empty,
                Name: recipient.GetProperty("name").GetString() ?? string.Empty,
                Type: recipient.GetProperty("type").GetString() ?? string.Empty))
            .ToList();
        Assert.Equal(
            new[]
            {
                ("tom.hughes@example.com", "Tom Hughes", "to"),
                ("aisha.khan@example.com", "Aisha Khan", "cc"),
                ("records@example.com", "Records", "bcc")
            },
            recipients);
    }

    [Fact]
    public async Task SendAsync_WithAttachment_SerialisesAttachmentWithEncodedContent()
    {
        using var client = CreateClient();
        var invoiceBytes = Encoding.UTF8.GetBytes("Invoice INV-2026-0042, total £1,250.00");
        var mailMessage = CreateMessage(MailFormat.Plain, "Please find your invoice attached.");
        mailMessage.Attachments.Add(new MailAttachment("INV-2026-0042.pdf", "application/pdf", invoiceBytes));

        await client.SendAsync(mailMessage);

        var attachment = Assert.Single(
            Assert.Single(_service.Requests).Json().GetProperty("message").GetProperty("attachments").EnumerateArray().ToList());
        Assert.Equal("application/pdf", attachment.GetProperty("type").GetString());
        Assert.Equal("INV-2026-0042.pdf", attachment.GetProperty("name").GetString());
        Assert.Equal(invoiceBytes, Convert.FromBase64String(attachment.GetProperty("content").GetString()!));
    }

    [Fact]
    public async Task SendTemplateMessageAsync_ServiceReturnsSuccess_PostsToSendTemplateEndpointAndReturnsTrue()
    {
        using var client = CreateClient();
        var templates = new List<MandrillTemplate> { new() { Name = "first_name", Content = "Tom" } };

        var result = await client.SendTemplateMessageAsync(CreateMessage(MailFormat.Html, string.Empty), "welcome-email", templates);

        Assert.True(result);
        Assert.Equal("messages/send-template.json", Assert.Single(_service.Requests).Url);
    }

    [Fact]
    public async Task SendTemplateMessageAsync_ServiceReturnsError_ReturnsFalse()
    {
        _service.RespondTo("messages/send-template.json", HttpStatusCode.InternalServerError, "{\"status\":\"error\"}");
        using var client = CreateClient();

        var result = await client.SendTemplateMessageAsync(CreateMessage(MailFormat.Html, string.Empty), "welcome-email");

        Assert.False(result);
    }

    [Fact]
    public void Constructor_NullOptions_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            using var client = new MandrillClient(null!, _service);
        });
    }

    private MandrillClient CreateClient()
    {
        return new MandrillClient(new MandrillOptions { ApiKey = ApiKey }, _service);
    }

    private static MailMessage CreateMessage(MailFormat format, string body)
    {
        var message = new MailMessage
        {
            Format = format,
            Sender = new MailAddress("Priya Sharma", "priya.sharma@example.com"),
            Subject = "Welcome to Audacia",
            Body = body
        };
        message.Recipients.Add(new MailAddress("Tom Hughes", "tom.hughes@example.com"));
        return message;
    }
}
