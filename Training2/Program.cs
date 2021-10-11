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
using NoNameLogger.Formatting;
using NoNameLogger.Services;
using NoNameLogger.Enums;
using NoNameLogger.LoggerConfigExtensions;
using NoNameLoggerMySQL;
using System.IO;
using System.Reflection;

namespace Training2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var noNameLoggerConfig = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    @"logs", "mylog.json"), new JsonFormatter(), rollingInterval: RollingInterval.Day)
                .WriteTo.MySQL("server=localhost;user=artur;password=12345678;database=trainingdb2;", "Logs");
            NoNameLogger.Interfaces.ILogger logger = noNameLoggerConfig.CreateLoggger();
            try
            {
                var host = CreateHostBuilder(args, logger).Build();
                host.Run();
            }
            catch (Exception ex)
            {
                logger.LogCritical($"{ex?.Message} " +
                    $"{ex?.StackTrace} {ex?.HResult}");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args, NoNameLogger.Interfaces.ILogger logger) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureLogging(log =>
            {
                log.ClearProviders();
                log.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                log.AddNoNameLogger(configuration =>
                {
                    configuration.Logger = logger;
                });
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .ConfigureServices(services =>
            {
                services.AddHostedService<UpdateRedisService>();
            });
    }
}
