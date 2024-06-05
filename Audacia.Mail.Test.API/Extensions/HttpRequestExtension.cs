namespace Audacia.Mail.Test.API.Extensions;

public static class HttpRequestExtension
{
    public static bool TryParseCustomHeaderValueIntoBoolean(this HttpRequest request, string headerName, out bool headerValue)
    {
        if (request.Headers.TryGetValue(headerName, out var headerStringValue))
        {
            return bool.TryParse(headerStringValue, out headerValue);
        }

        return headerValue = false;
    }
}
