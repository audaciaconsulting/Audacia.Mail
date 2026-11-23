using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Audacia.Mail.Tests.Mandrill;

/// <summary>An <see cref="HttpMessageHandler"/> that records requests and always returns 200 OK without touching the network.</summary>
internal sealed class StubHttpMessageHandler : HttpMessageHandler
{
    private readonly ConcurrentQueue<CapturedHttpRequest> _requests = new();

    /// <summary>Gets the captured requests, in order.</summary>
    public IReadOnlyList<CapturedHttpRequest> Requests => [.. _requests];

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var body = request.Content == null ? string.Empty : await request.Content.ReadAsStringAsync(cancellationToken);
        _requests.Enqueue(new CapturedHttpRequest(
            request.Method,
            request.RequestUri!,
            [.. request.Headers.Accept.Select(header => header.MediaType ?? string.Empty)],
            request.Content?.Headers.ContentType?.MediaType ?? string.Empty,
            body));

        return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("[]") };
    }
}
