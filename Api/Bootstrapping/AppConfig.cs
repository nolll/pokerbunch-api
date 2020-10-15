using Api.Middlewares;
using Api.Routes;
using Api.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Api.Bootstrapping
{
    public class AppConfig
    {
        private readonly AppSettings _settings;
        private readonly IApplicationBuilder _app;
        private readonly IHostingEnvironment _env;

        private bool IsDev => _env.IsDevelopment();
        private bool IsProd => !IsDev;

        public AppConfig(AppSettings settings, IApplicationBuilder app, IHostingEnvironment env)
        {
            _settings = settings;
            _app = app;
            _env = env;
        }

        public void Configure()
        {
            ConfigureCors();
            ConfigureCompression();
            ConfigureHttps();
            ConfigureErrors();
            ConfigureSwagger();
            ConfigureAuth();
            ConfigureMvc();
        }

        private void ConfigureCompression()
        {
            _app.UseResponseCompression();
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
            if (_settings.Error.DetailedErrors)
            {
                _app.UseDeveloperExceptionPage();
            }
            else
            {
                var errorUrl = $"/{ApiRoutes.Error}";
                _app.UseStatusCodePagesWithReExecute(errorUrl);
                _app.UseExceptionHandler(errorUrl);
                _app.UseMiddleware<ExceptionLoggingMiddleware>();
            }

        }
    }
}