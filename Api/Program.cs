using Api.Bootstrapping;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        CreateWebHostBuilder(args).Build().Run();
    }

    //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
    //    WebHost.CreateDefaultBuilder(args)
    //        .UseStartup<Startup>();

    public static IHostBuilder CreateWebHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                var port = Environment.GetEnvironmentVariable("PORT");

                if (!string.IsNullOrEmpty(port))
                    webBuilder.UseStartup<Startup>().UseUrls("http://*:" + port);
                else
                    webBuilder.UseStartup<Startup>();
            });
}