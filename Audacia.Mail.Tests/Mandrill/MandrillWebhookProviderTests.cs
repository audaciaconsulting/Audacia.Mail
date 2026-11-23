using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Audacia.Mail.Mandrill;
using Xunit;

namespace Audacia.Mail.Tests.Mandrill;

/// <summary>
/// Checks how <see cref="MandrillWebhookProvider"/> reconciles webhooks. Requests go to a <see cref="FakeMandrillService"/>, never to Mandrill.
/// </summary>
public sealed class MandrillWebhookProviderTests
{
    private const string ApiKey = "test-api-key-not-real";

    private const string EnvironmentUrl = "https://app.example.com/api/mandrill/webhook";

    /// <summary>
    /// Shape taken from the Mandrill webhooks/list API documentation.
    /// </summary>
    private const string OneWebhookJson = """
        [
          {
            "id": 42,
            "url": "https://app.example.com/api/mandrill/webhook",
            "description": "Email Sent",
            "auth_key": "auth-key-not-real",
            "events": ["send", "open"],
            "created_at": "2026-01-01 15:30:42",
            "last_sent_at": "2026-01-01 15:30:49",
            "batches_sent": 42,
            "events_sent": 42,
            "last_error": null
          }
        ]
        """;

    private const string TwoWebhooksJson = """
        [
          { "id": 42, "url": "https://app.example.com/old", "events": ["send"] },
          { "id": 43, "url": "https://app.example.com/older", "events": ["open"] }
        ]
        """;

    private static readonly Uri EnvironmentUri = new(EnvironmentUrl);

    private readonly FakeMandrillService _service = new();

    [Fact]
    public async Task SetUpMandrillWebhookAsync_NoExistingWebhooks_ListsThenAddsWebhook()
    {
        _service.RespondTo("webhooks/list", HttpStatusCode.OK, "[]");

        var result = await CreateProvider().SetUpMandrillWebhookAsync(EnvironmentUri);

        Assert.True(result);
        Assert.Equal(["webhooks/list", "webhooks/add"], _service.Requests.Select(request => request.Url));
    }

    [Fact]
    public async Task SetUpMandrillWebhookAsync_OneExistingWebhook_ListsThenUpdatesWebhook()
    {
        _service.RespondTo("webhooks/list", HttpStatusCode.OK, OneWebhookJson);

        var result = await CreateProvider().SetUpMandrillWebhookAsync(EnvironmentUri);

        Assert.True(result);
        Assert.Equal(["webhooks/list", "webhooks/update"], _service.Requests.Select(request => request.Url));
    }

    [Fact]
    public async Task SetUpMandrillWebhookAsync_MultipleExistingWebhooks_DeletesEachThenAddsOne()
    {
        _service.RespondTo("webhooks/list", HttpStatusCode.OK, TwoWebhooksJson);

        var result = await CreateProvider().SetUpMandrillWebhookAsync(EnvironmentUri);

        Assert.True(result);
        Assert.Equal(
            ["webhooks/list", "webhooks/delete", "webhooks/delete", "webhooks/add"],
            _service.Requests.Select(request => request.Url));
    }

    [Fact]
    public async Task SetUpMandrillWebhookAsync_DeleteFails_ReturnsFalseWithoutAddingWebhook()
    {
        _service
            .RespondTo("webhooks/list", HttpStatusCode.OK, TwoWebhooksJson)
            .RespondTo("webhooks/delete", HttpStatusCode.InternalServerError, "{}");

        var result = await CreateProvider().SetUpMandrillWebhookAsync(EnvironmentUri);

        Assert.False(result);
        Assert.DoesNotContain(_service.Requests, request => request.Url == "webhooks/add");
    }

    [Fact]
    public async Task SetUpMandrillWebhookAsync_ListFails_ReturnsFalseWithoutChangingWebhooks()
    {
        _service.RespondTo("webhooks/list", HttpStatusCode.Unauthorized, "{\"status\":\"error\",\"name\":\"Invalid_Key\"}");

        var result = await CreateProvider().SetUpMandrillWebhookAsync(EnvironmentUri);

        Assert.False(result);
        Assert.Equal("webhooks/list", Assert.Single(_service.Requests).Url);
    }

    [Fact]
    public async Task SetUpMandrillWebhookAsync_AddRequest_IncludesApiKeyUrlAndDescription()
    {
        _service.RespondTo("webhooks/list", HttpStatusCode.OK, "[]");

        await CreateProvider().SetUpMandrillWebhookAsync(EnvironmentUri);

        var addRequest = _service.Requests.Single(request => request.Url == "webhooks/add");
        Assert.Equal(ApiKey, addRequest.PropertyIgnoringCase("key").GetString());
        Assert.Equal(EnvironmentUrl, addRequest.PropertyIgnoringCase("url").GetString());
        Assert.Equal("Email Sent", addRequest.PropertyIgnoringCase("description").GetString());
    }

    [Fact]
    public async Task SetUpMandrillWebhookAsync_NullUrl_ThrowsArgumentNullExceptionWithoutCallingMandrill()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => CreateProvider().SetUpMandrillWebhookAsync((Uri)null!));
        Assert.Empty(_service.Requests);
    }

    private MandrillWebhookProvider CreateProvider()
    {
        return new MandrillWebhookProvider(_service, new MandrillOptions { ApiKey = ApiKey });
    }
}
