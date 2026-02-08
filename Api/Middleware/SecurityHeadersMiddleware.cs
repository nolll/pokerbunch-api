using Api.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace Api.Middleware;

[UsedImplicitly]
public class SecurityHeadersMiddleware(RequestDelegate next)
{
    [UsedImplicitly]
    public async Task InvokeAsync(HttpContext httpContext)
    {
        httpContext.AddHeader("X-Content-Type-Options", "nosniff");
        httpContext.AddHeader("X-Frame-Options", "DENY");
        httpContext.AddHeader("X-XSS-Protection", "1; mode=block");
        httpContext.AddHeader("Strict-Transport-Security", "max-age=63072000; includeSubDomains");
        httpContext.AddHeader("Content-Security-Policy", Csp);
        await next(httpContext);
    }

    private static string Csp => string.Join("; ", CspValues);

    private static string[] CspValues =>
    [
        "default-src 'self'",
        "script-src 'self' 'unsafe-inline'",
        "img-src 'self' data:",
        "style-src 'self' 'unsafe-inline'"
    ];
}