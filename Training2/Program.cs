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
using NoNameLogger.AspNetCore.Config;
using System.IO;
using System.Reflection;
using NoNameLogger.Formatting;
using NoNameLogger.Services;
using NoNameLogger;
using NoNameLogger.LoggerConfigExtensions;
using NoNameLoggerMsSql;

namespace Training2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string connection = "Server=(localdb)\\mssqllocaldb;Database=Training2;Trusted_Connection=True;MultipleActiveResultSets=true";
            var myLoggerConfig = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    @"logs", "mylog.json"), new JsonFormatter(), rollingInterval: RollingInterval.Day,
                    fileSizeLimitBytes: 5120, rollOnFileSizeLimit: true)
                .WriteTo.File(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    @"logs", "mylog.bin"), new BinaryFormatter(), NoNameLogger.LogLevel.Info,
                    NoNameLogger.LogLevel.Info, RollingInterval.Hour)
                .WriteTo.File(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                        @"logs", "mylog.xml"), new XmlFormatter(), NoNameLogger.LogLevel.Warning, rollingInterval: RollingInterval.Minute,
                        fileSizeLimitBytes: 5120, rollOnFileSizeLimit: true, encoding: System.Text.Encoding.Unicode)
                .WriteTo.MsSQLServer(connection, "Logs");
            NoNameLogger.Interfaces.ILogger logger = myLoggerConfig.CreateLoggger();
            logger.LogDebug("Config host");
            logger.LogInformation("Log info");
            logger.LogWarning("Log warning");
            logger.LogError("Log errore");
            logger.LogCritical("Log critical");
            var host = CreateHostBuilder(args, logger).Build();
            var logger_ = host.Services.GetRequiredService<ILogger<Program>>();

            logger_.LogDebug(1, "Does this line get hit?");    // Not logged
            logger_.LogInformation(3, "Nothing to see here."); // Logs in ConsoleColor.Green
            logger_.LogWarning(5, "Warning... that was odd."); // Logs in ConsoleColor.DarkMagenta
            logger_.LogError(7, "Oops, there was an error.");  // Logs in ConsoleColor.Red
            host.Run();
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
                //log.AddColorConsoleLogger(configuration =>
                //{
                //    configuration.LogLevels.Add(
                //        LogLevel.Warning, ConsoleColor.DarkMagenta);
                //    configuration.LogLevels.Add(
                //        LogLevel.Error, ConsoleColor.Red);
                //});//https://docs.microsoft.com/ru-ru/dotnet/core/extensions/custom-logging-provider
                //log.AddNoNameFileLogger(configuration =>
                //{
                //    configuration.Path = Path.Combine(Path.GetDirectoryName(
                //       Assembly.GetExecutingAssembly().Location), @"logs", "asplog.json");
                //    configuration.Formatter = new JsonFormatter();
                //});
                //log.AddNoNameFileLogger(configuration =>
                //{
                //    configuration.Path = Path.Combine(Path.GetDirectoryName(
                //       Assembly.GetExecutingAssembly().Location), @"logs", "asplog.xml");
                //    configuration.Formatter = new XmlFormatter();
                //    configuration.Encoding = System.Text.Encoding.Unicode;
                //    configuration.FileSizeLimitBytes = 5120;
                //    configuration.RollingInterval = NoNameLogger.RollingInterval.Minute;
                //    configuration.RollOnFileSizeLimit = true;
                //});


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
