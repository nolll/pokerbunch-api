using Microsoft.AspNetCore.Http;

namespace Api.Extensions;

public static class ResponseExtensions
{
    public static void AddHeader(this HttpContext httpContext, string header, string value)
    {
        httpContext.Response.Headers.Remove(header);
        httpContext.Response.Headers.Add(header, value);
    }
}