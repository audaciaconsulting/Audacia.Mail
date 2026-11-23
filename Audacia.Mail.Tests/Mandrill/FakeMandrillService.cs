using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Audacia.Mail.Mandrill.Services;

namespace Audacia.Mail.Tests.Mandrill;

/// <summary>
/// An in-memory <see cref="IMandrillService"/> that records each request and returns canned responses.
/// It makes no network calls.
/// </summary>
internal sealed class FakeMandrillService : IMandrillService
{
    private readonly Dictionary<string, Queue<(HttpStatusCode Status, string Body)>> _responses = [];

    /// <summary>Gets the requests received, in order.</summary>
    public List<RecordedRequest> Requests { get; } = [];

    /// <summary>Queues a response to return the next time <paramref name="requestUrl"/> is called. Unqueued calls return 200 with an empty JSON array.</summary>
    /// <param name="requestUrl">The relative request URL.</param>
    /// <param name="status">The status code to return.</param>
    /// <param name="body">The response body.</param>
    /// <returns>This instance, for chaining.</returns>
    public FakeMandrillService RespondTo(string requestUrl, HttpStatusCode status, string body = "[]")
    {
        if (!_responses.TryGetValue(requestUrl, out var queue))
        {
            queue = new Queue<(HttpStatusCode Status, string Body)>();
            _responses[requestUrl] = queue;
        }

        queue.Enqueue((status, body));
        return this;
    }

    /// <inheritdoc />
    public async Task<HttpResponseMessage> SendEmailAsync(string requestUrl, HttpContent content)
    {
        // Read the body now: callers dispose the content as soon as this method returns.
        var body = await content.ReadAsStringAsync();
        Requests.Add(new RecordedRequest(requestUrl, body));

        var (status, responseBody) = _responses.TryGetValue(requestUrl, out var queue) && queue.Count > 0
            ? queue.Dequeue()
            : (HttpStatusCode.OK, "[]");

        return new HttpResponseMessage(status) { Content = new StringContent(responseBody) };
    }
}
