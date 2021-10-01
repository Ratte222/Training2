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
using NoNameLogger.Enums;
using NoNameLogger.LoggerConfigExtensions;
using NoNameLoggerMsSql;
using NoNameLoggerMySQL;
using NoNameLogger.Configs.Notification;

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
                    @"logs", "mylog.json"), new JsonFormatter(), rollingInterval: RollingInterval.Day)
                .WriteTo.File(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    @"logs", "mylog.bin"), new BinaryFormatter(), rollingInterval: RollingInterval.Hour)
                .WriteTo.File(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                        @"logs", "mylog.xml"), new XmlFormatter(), rollingInterval: RollingInterval.Minute)
                .WriteTo.MsSQLServer(connection, "Logs")
                .WriteTo.MySQL("server=localhost;user=artur;password=12345678;database=trainingdb2;", "Logs")
                .WriteTo.Email(configuration => 
                {
                    configuration.EmailFrom = "emailFrom@gmail.com";
                    configuration.EmailTo = "emailTo@gmail.com";
                    configuration.Password = "password";
                    configuration.SmtpHost = "smtp.gmail.com";
                    configuration.SmtpPort = 587;
                    configuration.MinLogLevel = NoNameLogger.Enums.LogLevel.Critical;
                    configuration.MaxLogLevel = NoNameLogger.Enums.LogLevel.Critical;
                    configuration.Formatter = new JsonFormatter();
                    return configuration;
                });
            NoNameLogger.Interfaces.ILogger logger = myLoggerConfig.CreateLoggger();
            try
            {
                var host = CreateHostBuilder(args, logger).Build();
                host.Run();
            }
            catch(Exception ex)
            {
                logger.LogCritical($"{ex?.Message} " +
                    $"{ex?.StackTrace} {ex?.HResult}");
            }
            //logger.Dispose();
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
