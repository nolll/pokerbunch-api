using System;
using System.Threading.Tasks;
using Core.Exceptions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Api.Middleware;

[UsedImplicitly]
public class ExceptionLoggingMiddleware
{
    private readonly ILogger<ExceptionLoggingMiddleware> _logger;
    private readonly IHostingEnvironment _env;
    private readonly RequestDelegate _next;

    public ExceptionLoggingMiddleware(RequestDelegate next, ILogger<ExceptionLoggingMiddleware> logger, IHostingEnvironment env)
    {
        _logger = logger;
        _env = env;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (AccessDeniedException)
        {
            throw;
        }
        catch (AuthException)
        {
            throw;
        }
        catch (ValidationException)
        {
            throw;
        }
        catch (ConflictException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}