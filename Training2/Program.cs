using BLL.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Training2.BackgroundService;
using BenchmarkDotNet.Running;
using NoNameLogger.AspNetCore;

namespace Training2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var logger = host.Services.GetRequiredService<ILogger<Program>>();

            logger.LogDebug(1, "Does this line get hit?");    // Not logged
            logger.LogInformation(3, "Nothing to see here."); // Logs in ConsoleColor.Green
            logger.LogWarning(5, "Warning... that was odd."); // Logs in ConsoleColor.DarkMagenta
            logger.LogError(7, "Oops, there was an error.");  // Logs in ConsoleColor.Red
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices(services =>
                {
                    services.AddHostedService<UpdateRedisService>();
                })
                .ConfigureLogging(log=>
                {
                    log.ClearProviders();
                    log.AddColorConsoleLogger();//https://docs.microsoft.com/ru-ru/dotnet/core/extensions/custom-logging-provider
                });
    }
}
