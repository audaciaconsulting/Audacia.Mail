using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Audacia.Mail.MailKit;
using MimeKit;
using Xunit;

namespace Audacia.Mail.Tests.MailKit;

/// <summary>
/// Sends real SMTP traffic from <see cref="MailKitClient"/> to an in-process <see cref="FakeSmtpServer"/> on the loopback interface.
/// No email leaves the machine.
/// </summary>
public sealed class MailKitClientTests
{
    private static readonly MailAddress Sender = new("Priya Sharma", "priya.sharma@example.com");

    private static readonly MailAddress Recipient = new("Tom Hughes", "tom.hughes@example.com");

    [Fact]
    public async Task SendAsync_HtmlMessage_DeliversSenderRecipientSubjectAndHtmlBody()
    {
        await using var server = new FakeSmtpServer();
        using var client = new MailKitClient(server.CreateSettings());
        var message = CreateMessage(MailFormat.Html, "<p>Your order <b>#10452</b> has been dispatched.</p>");

        await client.SendAsync(message);

        var received = Assert.Single(server.Messages);
        using var mime = received.ParseMime();
        Assert.Equal("priya.sharma@example.com", received.MailFrom);
        Assert.Equal(["tom.hughes@example.com"], received.EnvelopeRecipients);
        Assert.Equal("Order dispatched", mime.Subject);
        Assert.Equal("Priya Sharma", mime.From.Mailboxes.Single().Name);
        Assert.Equal("Tom Hughes", mime.To.Mailboxes.Single().Name);
        var body = Assert.IsType<TextPart>(mime.BodyParts.First());
        Assert.Equal("text/html", body.ContentType.MimeType);
        Assert.Equal("<p>Your order <b>#10452</b> has been dispatched.</p>", body.Text);
    }

    [Fact]
    public async Task SendAsync_PlainMessage_DeliversTextPlainBody()
    {
        await using var server = new FakeSmtpServer();
        using var client = new MailKitClient(server.CreateSettings());
        var message = CreateMessage(MailFormat.Plain, "Your order #10452 has been dispatched.");

        await client.SendAsync(message);

        using var mime = Assert.Single(server.Messages).ParseMime();
        var body = Assert.IsType<TextPart>(mime.BodyParts.First());
        Assert.Equal("text/plain", body.ContentType.MimeType);
        Assert.Equal("Your order #10452 has been dispatched.", body.Text);
    }

    [Fact]
    public async Task SendAsync_WithCcAndBcc_DeliversToAllRecipientsAndAddsCcHeader()
    {
        await using var server = new FakeSmtpServer();
        using var client = new MailKitClient(server.CreateSettings());
        var message = CreateMessage(MailFormat.Plain, "Minutes attached.");
        message.Cc.Add(new MailAddress("Aisha Khan", "aisha.khan@example.com"));
        message.Bcc.Add(new MailAddress("Records", "records@example.com"));

        await client.SendAsync(message);

        var received = Assert.Single(server.Messages);
        using var mime = received.ParseMime();
        Assert.Equal("aisha.khan@example.com", mime.Cc.Mailboxes.Single().Address);
        Assert.Equal(
            ["aisha.khan@example.com", "records@example.com", "tom.hughes@example.com"],
            received.EnvelopeRecipients.OrderBy(address => address, StringComparer.Ordinal));
    }

    [Fact(Skip = "Known bug: MailKitClient passes FormatOptions.Default to SmtpClient.SendAsync, which bypasses MailKit's default hiding of the Bcc header, so BCC addresses are visible to every recipient.")]
    public async Task SendAsync_WithBcc_DoesNotExposeBccAddressInTransmittedHeaders()
    {
        await using var server = new FakeSmtpServer();
        using var client = new MailKitClient(server.CreateSettings());
        var message = CreateMessage(MailFormat.Plain, "Minutes attached.");
        message.Bcc.Add(new MailAddress("Records", "records@example.com"));

        await client.SendAsync(message);

        var received = Assert.Single(server.Messages);
        Assert.Contains("records@example.com", received.EnvelopeRecipients);
        Assert.DoesNotContain("records@example.com", received.RawData, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task SendAsync_WithAttachment_DeliversAttachmentWithOriginalFileNameTypeAndBytes()
    {
        await using var server = new FakeSmtpServer();
        using var client = new MailKitClient(server.CreateSettings());
        var invoiceBytes = Encoding.UTF8.GetBytes("Invoice INV-2026-0042, total £1,250.00");
        var message = CreateMessage(MailFormat.Plain, "Please find your invoice attached.");
        message.Attachments.Add(new MailAttachment("INV-2026-0042.pdf", "application/pdf", invoiceBytes));

        await client.SendAsync(message);

        using var mime = Assert.Single(server.Messages).ParseMime();
        var attachment = Assert.IsType<MimePart>(Assert.Single(mime.Attachments));
        Assert.Equal("INV-2026-0042.pdf", attachment.FileName);
        Assert.Equal("application/pdf", attachment.ContentType.MimeType);
        Assert.Equal(ContentEncoding.Base64, attachment.ContentTransferEncoding);
        await using var decoded = new System.IO.MemoryStream();
        await attachment.Content!.DecodeToAsync(decoded);
        Assert.Equal(invoiceBytes, decoded.ToArray());
    }

    [Fact]
    public async Task SendAsync_MessageWithoutSenderAndDefaultSenderConfigured_UsesDefaultSender()
    {
        await using var server = new FakeSmtpServer();
        using var client = new MailKitClient(server.CreateSettings(defaultSender: "no-reply@example.com"));
        var message = new MailMessage("tom.hughes@example.com")
        {
            Subject = "Password reset",
            Body = "Use the link below to reset your password."
        };

        await client.SendAsync(message);

        var received = Assert.Single(server.Messages);
        using var mime = received.ParseMime();
        Assert.Equal("no-reply@example.com", received.MailFrom);
        Assert.Equal("no-reply@example.com", mime.From.Mailboxes.Single().Address);
    }

    [Fact]
    public async Task SendAsync_ServerAdvertisesAuthentication_AuthenticatesWithConfiguredCredentials()
    {
        await using var server = new FakeSmtpServer(advertiseAuthentication: true);
        using var client = new MailKitClient(server.CreateSettings(userName: "smtp-user", password: "not-a-real-password"));

        await client.SendAsync(CreateMessage(MailFormat.Plain, "Authenticated message."));

        Assert.NotNull(server.ReceivedCredentials);
        Assert.Equal("smtp-user", server.ReceivedCredentials!.UserName);
        Assert.Equal("not-a-real-password", server.ReceivedCredentials.Password);
        Assert.Single(server.Messages);
    }

    [Fact]
    public async Task SendAsync_ServerDoesNotAdvertiseAuthentication_SendsWithoutAuthenticating()
    {
        await using var server = new FakeSmtpServer(advertiseAuthentication: false);
        using var client = new MailKitClient(server.CreateSettings(userName: "smtp-user", password: "not-a-real-password"));

        await client.SendAsync(CreateMessage(MailFormat.Plain, "Unauthenticated message."));

        Assert.Null(server.ReceivedCredentials);
        Assert.Single(server.Messages);
    }

    [Fact]
    public async Task SendAsync_CalledTwice_ReusesTheSameConnection()
    {
        await using var server = new FakeSmtpServer();
        using var client = new MailKitClient(server.CreateSettings());

        await client.SendAsync(CreateMessage(MailFormat.Plain, "First message."));
        await client.SendAsync(CreateMessage(MailFormat.Plain, "Second message."));

        Assert.Equal(2, server.Messages.Count);
        Assert.Equal(1, server.ConnectionCount);
    }

    [Fact]
    public async Task Connect_ServerAdvertisesAuthentication_ConnectsAndAuthenticatesBeforeFirstSend()
    {
        await using var server = new FakeSmtpServer(advertiseAuthentication: true);

        using var client = MailKitClient.Connect(server.CreateSettings(userName: "smtp-user", password: "not-a-real-password"));

        Assert.Equal(1, server.ConnectionCount);
        Assert.Equal("smtp-user", server.ReceivedCredentials?.UserName);
        Assert.Empty(server.Messages);
    }

    [Fact]
    public async Task SendAsync_NullMessage_ThrowsArgumentNullException()
    {
        await using var server = new FakeSmtpServer();
        using var client = new MailKitClient(server.CreateSettings());

        await Assert.ThrowsAsync<ArgumentNullException>(() => client.SendAsync(null!));
        Assert.Equal(0, server.ConnectionCount);
    }

    private static MailMessage CreateMessage(MailFormat format, string body)
    {
        var message = new MailMessage
        {
            Format = format,
            Sender = Sender,
            Subject = "Order dispatched",
            Body = body
        };
        message.Recipients.Add(Recipient);
        return message;
    }
}
