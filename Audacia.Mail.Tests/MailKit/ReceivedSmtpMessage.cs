using System.Collections.Generic;
using System.IO;
using System.Text;
using MimeKit;

namespace Audacia.Mail.Tests.MailKit;

/// <summary>A message captured by <see cref="FakeSmtpServer"/>, including the SMTP envelope.</summary>
/// <remarks><para>Initializes a new instance of the <see cref="ReceivedSmtpMessage"/> class.</para></remarks>
/// <param name="mailFrom">The envelope sender (MAIL FROM).</param>
/// <param name="envelopeRecipients">The envelope recipients (RCPT TO).</param>
/// <param name="rawData">The raw message data sent after DATA.</param>
internal sealed class ReceivedSmtpMessage(string mailFrom, IReadOnlyList<string> envelopeRecipients, string rawData)
{
    /// <summary>Gets the envelope sender (MAIL FROM).</summary>
    public string MailFrom { get; } = mailFrom;

    /// <summary>Gets the envelope recipients (RCPT TO), which include BCC addresses.</summary>
    public IReadOnlyList<string> EnvelopeRecipients { get; } = envelopeRecipients;

    /// <summary>Gets the raw message data.</summary>
    public string RawData { get; } = rawData;

    /// <summary>Parses <see cref="RawData"/> into a <see cref="MimeMessage"/>. The caller owns the result.</summary>
    /// <returns>The parsed message.</returns>
    public MimeMessage ParseMime()
    {
        using var stream = new MemoryStream(Encoding.Latin1.GetBytes(RawData));
        return MimeMessage.Load(stream);
    }
}
