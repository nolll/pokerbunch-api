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
        SetDefaultSecurityHeaders(httpContext);
        SetCspSecurityHeaders(httpContext);
        await next(httpContext);
    }

    private static void SetDefaultSecurityHeaders(HttpContext httpContext)
    {
        httpContext.AddHeader("X-Content-Type-Options", "nosniff");
        httpContext.AddHeader("X-Frame-Options", "DENY");
        httpContext.AddHeader("X-XSS-Protection", "1; mode=block");
        httpContext.AddHeader("Strict-Transport-Security", "max-age=63072000; includeSubDomains");
    }

    private static void SetCspSecurityHeaders(HttpContext httpContext) => 
        httpContext.AddHeader("Content-Security-Policy", GetCsp());

    private static string GetCsp() => string.Join("; ", GetDefaultCspValues());

    private static string[] GetDefaultCspValues() =>
    [
        "default-src 'self'",
        "script-src 'self' 'unsafe-inline'",
        "img-src 'self' data:",
        "style-src 'self' 'unsafe-inline'"
    ];
}