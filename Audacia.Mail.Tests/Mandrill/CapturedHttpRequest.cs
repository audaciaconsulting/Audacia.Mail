using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Audacia.Mail.Tests.Mandrill;

/// <summary>A request captured by <see cref="StubHttpMessageHandler"/>.</summary>
/// <param name="Method">The HTTP method.</param>
/// <param name="Uri">The absolute request URI.</param>
/// <param name="Accept">The Accept header media types.</param>
/// <param name="ContentType">The request content media type.</param>
/// <param name="Body">The request body.</param>
internal sealed record CapturedHttpRequest(HttpMethod Method, Uri Uri, IReadOnlyList<string> Accept, string ContentType, string Body);
