namespace Audacia.Mail.Test.API.Settings;

public class SmtpOptions : SmtpSettings
{
    public EmailClientType EmailClientType { get; set; }

    public string? FromEmailAddress { get; set; }

    public bool EnableSsl { get; set; }

    public string? EmailDryRunHeaderName { get; set; }
}

public class EmailSenderDetails
{
    public EmailSenderDetails() =>
        Address = new MailAddress();

    public EmailSenderDetails(string fromAddress) =>
        Address = new MailAddress(fromAddress);

    public MailAddress Address { get; }
}

public enum EmailClientType
{
    // No SMTP client so no emails will be sent. Can be used for testing.
    None,

    // Use a general SMTP client.
    Smtp,

    // Use a local SMTP client.
    Local,

    // Use the MailTrap SMTP client.
    MailTrap,

    // Use SendGrid to send emails.
    SendGrid
}
