using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Audacia.Mail.Mandrill.InternalModels.WebhookJsonDeserialisation;
using Xunit;

namespace Audacia.Mail.Tests.Mandrill;

/// <summary>
/// Checks that the public webhook models deserialise a Mandrill webhook event with System.Text.Json,
/// using the case-insensitive options a consuming application would typically use.
/// </summary>
public sealed class WebhookEventDeserialisationTests
{
    /// <summary>
    /// Shape taken from the Mandrill webhook event format documentation.
    /// </summary>
    private const string OpenEventJson = """
        {
          "event": "open",
          "_id": "8b6a0c2f4e1d4a7b9c3e5f7a9b1c3d5e",
          "ts": 1785412800,
          "ip": "203.0.113.24",
          "user_agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) Firefox/140.0",
          "location": {
            "country_short": "GB",
            "country_long": "United Kingdom",
            "region": "England",
            "city": "Leeds",
            "postal_code": "LS1",
            "timezone": "+00:00",
            "latitude": 53.8,
            "longitude": -1.55
          },
          "user_agent_parsed": {
            "type": "Browser",
            "ua_family": "Firefox",
            "ua_name": "Firefox 140.0",
            "os_family": "Windows",
            "os_name": "Windows 11",
            "mobile": false
          },
          "msg": {
            "ts": 1785409200,
            "_id": "8b6a0c2f4e1d4a7b9c3e5f7a9b1c3d5e",
            "_version": "v1",
            "subject": "Order dispatched",
            "email": "tom.hughes@example.com",
            "sender": "priya.sharma@example.com",
            "state": "sent",
            "tags": ["orders"],
            "metadata": { "order_id": "10452" },
            "opens": [ { "ts": 1785412800, "ip": "203.0.113.24", "location": "Leeds, GB", "ua": "Windows/Firefox" } ],
            "clicks": [],
            "smtp_events": [ { "ts": 1785409205, "type": "sent", "diag": "250 2.0.0 OK", "source_ip": "198.51.100.7", "destination_ip": "203.0.113.50" } ]
          }
        }
        """;

    private static readonly JsonSerializerOptions ConsumerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    [Fact]
    public void Deserialize_OpenEvent_MapsEventTypeAndTopLevelFields()
    {
        var item = JsonSerializer.Deserialize<WebhookReturnItem>(OpenEventJson, ConsumerOptions)!;

        Assert.Equal(EmailEventType.Open, item.Event);
        Assert.Equal("8b6a0c2f4e1d4a7b9c3e5f7a9b1c3d5e", item.Id);
        Assert.Equal(1785412800, item.Ts);
        Assert.Equal("203.0.113.24", item.Ip);
    }

    [Fact]
    public void Deserialize_OpenEvent_MapsSnakeCaseLocationAndUserAgentFields()
    {
        var item = JsonSerializer.Deserialize<WebhookReturnItem>(OpenEventJson, ConsumerOptions)!;

        Assert.Equal("GB", item.Location.CountryShort);
        Assert.Equal("United Kingdom", item.Location.CountryLong);
        Assert.Equal("LS1", item.Location.PostalCode);
        Assert.Equal("Firefox", item.UserAgentParsed.UaFamily);
        Assert.Equal("Windows", item.UserAgentParsed.OsFamily);
    }

    [Fact]
    public void Deserialize_OpenEvent_MapsMessageOpensAndSmtpEvents()
    {
        var message = JsonSerializer.Deserialize<WebhookReturnItem>(OpenEventJson, ConsumerOptions)!.Msg;

        Assert.Equal("tom.hughes@example.com", message.Email);
        Assert.Equal("v1", message.Version);
        Assert.Equal(["orders"], message.Tags);
        Assert.Equal("10452", message.Metadata["order_id"].ToString());
        Assert.Equal("203.0.113.24", Assert.Single(message.Opens).Ip);
        var smtpEvent = Assert.Single(message.SmtpEvents);
        Assert.Equal("250 2.0.0 OK", smtpEvent.Diag);
        Assert.Equal("198.51.100.7", smtpEvent.SourceIp);
    }

    [Fact]
    public void Deserialize_SendEvent_MapsEventTypeToSend()
    {
        var json = OpenEventJson.Replace("\"event\": \"open\"", "\"event\": \"send\"", System.StringComparison.Ordinal);

        var item = JsonSerializer.Deserialize<WebhookReturnItem>(json, ConsumerOptions)!;

        Assert.Equal(EmailEventType.Send, item.Event);
        Assert.Empty(item.Msg.Clicks.ToList());
    }
}
