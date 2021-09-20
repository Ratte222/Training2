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
            if(WriteTo.ConsoleConfigs.Count > 0)
            {
                actions.Add(new LogInConsole.LogInConsole(WriteTo.ConsoleConfigs[0]));
            }
            return new Logger(actions);
        }
    }
}
