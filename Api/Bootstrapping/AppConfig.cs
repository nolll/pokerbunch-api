using System;
using System.Threading.Tasks;
using Core.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Api.Bootstrapping
{
    public class AppConfig
    {
        private readonly IApplicationBuilder _app;
        private readonly IHostingEnvironment _env;

        private bool IsDev => _env.IsDevelopment();
        private bool IsProd => !IsDev;

        public AppConfig(IApplicationBuilder app, IHostingEnvironment env)
        {
            _app = app;
            _env = env;
        }

        public void Configure()
        {
            ConfigureCompression();
            ConfigureExceptions();
            ConfigureHttps();
            ConfigureErrors();
            ConfigureSwagger();
            ConfigureCors();
            ConfigureAuth();
            ConfigureMvc();
        }

        private void ConfigureCompression()
        {
            _app.UseResponseCompression();
        }

        private void ConfigureExceptions()
        {
            if(IsDev)
                _app.UseDeveloperExceptionPage();
        }

        private void ConfigureHttps()
        {
            if (IsProd)
            {
                _app.UseHsts();
                _app.UseHttpsRedirection();
            }
        }

        private void ConfigureSwagger()
        {
            _app.UseSwagger();
            _app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });
        }

        private void ConfigureCors()
        {
            _app.UseCors("CorsPolicy");
        }

        private void ConfigureAuth()
        {
            _app.UseAuthentication();
        }

        private void ConfigureMvc()
        {
            _app.UseMvc();
        }

        private void ConfigureErrors()
        {
            _app.UseStatusCodePagesWithReExecute("/Error");
            _app.UseExceptionHandler("/Error");
            _app.UseMiddleware<ExceptionLoggingMiddleware>();
        }
    }

    public class ExceptionLoggingMiddleware
    {
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _env;
        private readonly RequestDelegate _next;

        public ExceptionLoggingMiddleware(RequestDelegate next, ILogger logger, IHostingEnvironment env)
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
            catch (ConflictException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(0), ex, ex.Message);
                throw;
            }
        }
    }
}