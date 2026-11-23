using System;
using System.Net.Http;
using System.Threading.Tasks;
using Audacia.Mail.Mandrill;
using Audacia.Mail.Mandrill.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Audacia.Mail.Tests.Mandrill;

/// <summary>
/// Checks the dependency injection registration end to end, through Microsoft.Extensions.Http and System.Net.Http.Json.
/// The primary HTTP handler is replaced with <see cref="StubHttpMessageHandler"/>, so nothing reaches mandrillapp.com.
/// </summary>
public sealed class MandrillServiceCollectionExtensionsTests : IDisposable
{
    private readonly StubHttpMessageHandler _handler = new();

    private readonly ServiceProvider _provider;

    public MandrillServiceCollectionExtensionsTests()
    {
        var services = new ServiceCollection();
        services.AddMandrillClient(new MandrillOptions { ApiKey = "test-api-key-not-real" });

        // Every HttpClient built by the factory uses the stub, so no request can leave the process.
        services.ConfigureHttpClientDefaults(builder => builder.ConfigurePrimaryHttpMessageHandler(() => _handler));
        _provider = services.BuildServiceProvider();
    }

    [Fact]
    public void AddMandrillClient_ResolvingMailClient_ReturnsMandrillClient()
    {
        using var scope = _provider.CreateScope();

        var client = scope.ServiceProvider.GetRequiredService<IMailClient>();

        Assert.IsType<MandrillClient>(client);
    }

    [Fact]
    public void AddMandrillClient_ResolvingMandrillService_ReturnsMandrillService()
    {
        using var scope = _provider.CreateScope();

        var service = scope.ServiceProvider.GetRequiredService<IMandrillService>();

        Assert.IsType<MandrillService>(service);
    }

    [Fact]
    public async Task AddMandrillClient_SendingMessage_PostsJsonToMandrillMessagesSendEndpoint()
    {
        using var scope = _provider.CreateScope();
        var client = scope.ServiceProvider.GetRequiredService<IMailClient>();
        var message = new MailMessage("tom.hughes@example.com")
        {
            Sender = new MailAddress("Priya Sharma", "priya.sharma@example.com"),
            Subject = "Order dispatched",
            Body = "Your order #10452 has been dispatched."
        };

        await client.SendAsync(message);

        var request = Assert.Single(_handler.Requests);
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal(new Uri("https://mandrillapp.com/api/1.0/messages/send"), request.Uri);
        Assert.Contains("application/json", request.Accept);
        Assert.Equal("application/json", request.ContentType);
        Assert.Contains("\"key\":\"test-api-key-not-real\"", request.Body, StringComparison.Ordinal);
    }

    public void Dispose()
    {
        _provider.Dispose();
        _handler.Dispose();
    }
}
