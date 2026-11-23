using System;
using System.Linq;
using System.Text.Json;

namespace Audacia.Mail.Tests.Mandrill;

/// <summary>A request captured by <see cref="FakeMandrillService"/>.</summary>
/// <remarks><para>Initializes a new instance of the <see cref="RecordedRequest"/> class.</para></remarks>
/// <param name="url">The relative request URL.</param>
/// <param name="body">The request body.</param>
internal sealed class RecordedRequest(string url, string body)
{
    /// <summary>Gets the relative request URL.</summary>
    public string Url { get; } = url;

    /// <summary>Gets the raw request body.</summary>
    public string Body { get; } = body;

    /// <summary>Parses <see cref="Body"/> as JSON and returns a copy of its root element.</summary>
    /// <returns>The root element.</returns>
    public JsonElement Json()
    {
        using var document = JsonDocument.Parse(Body);
        return document.RootElement.Clone();
    }

    /// <summary>Gets a top-level property, ignoring case.</summary>
    /// <param name="name">The property name.</param>
    /// <returns>The property value.</returns>
    public JsonElement PropertyIgnoringCase(string name)
    {
        return Json().EnumerateObject()
            .Single(property => string.Equals(property.Name, name, StringComparison.OrdinalIgnoreCase))
            .Value;
    }
}
