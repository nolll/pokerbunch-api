using Microsoft.AspNetCore.Http;

namespace Api.Extensions
{
    public static class HeaderExtensions
    {
        public static void AddHeader(this HttpContext httpContext, string header, string value)
        {
            httpContext.Response.Headers.Remove(header);
            httpContext.Response.Headers.Add(header, value);
        }
    }
}