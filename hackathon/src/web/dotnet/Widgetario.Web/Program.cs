using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Prometheus;
using Serilog;

namespace Widgetario.Web
{
    public class Program
    {
        private static readonly Gauge _InfoGauge = 
            Metrics.CreateGauge("app_info", "Application info", "dotnet_version", "assembly_name", "app_version");

        public static void Main(string[] args)
        {
            var appVersion = Environment.GetEnvironmentVariable("APP_VERSION");
            var dotnetVersion = Environment.GetEnvironmentVariable("DOTNET_VERSION");
            _InfoGauge.Labels(dotnetVersion, "Widgetario.Web", appVersion).Set(1);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((builderContext, config) =>
                {
                    config.ReadFrom.Configuration(builderContext.Configuration);
                })
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    config.AddJsonFile("appsettings.json")
                          .AddEnvironmentVariables()
                          .AddJsonFile("config/serilog.json", optional: true, reloadOnChange: true)
                          .AddJsonFile("config/logging.json", optional: true, reloadOnChange: true)
                          .AddJsonFile("secrets/api.json", optional: true, reloadOnChange: true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
