using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

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
    }
}