using MyLoggerLibrary.Configs;
using MyLoggerLibrary.Events;
using MyLoggerLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLoggerLibrary.Services
{
    public class LoggerConfiguration:ILoggerConfiguration
    {
        public LoggerSinkConfiguration WriteTo { get; set; } 



        public LoggerConfiguration()
        {
            WriteTo = new LoggerSinkConfiguration(this);
        }

        public ILogger CreateLoggger()
        {
            List<ILog> actions = new List<ILog>();
            foreach(var consoleCong in WriteTo.ConsoleConfigs)
            {
                actions.Add(new LogInConsole.LogInConsole(consoleCong));
            }
            foreach(var fileConf in WriteTo.FileConfigs)
            {
                actions.Add(new LogInFile.LogInFile(fileConf));
            }
            return new Logger(actions);
        }
    }
}
