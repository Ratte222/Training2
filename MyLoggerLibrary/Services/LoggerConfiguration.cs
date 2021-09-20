using MyLoggerLibrary.Configs;
using MyLoggerLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLoggerLibrary.Services
{
    public class LoggerConfiguration:ILoggerConfiguration
    {
        public LoggerSinkConfiguration WriteTo { get; set; } = new LoggerSinkConfiguration();



        public LoggerConfiguration()
        {

        }

        public ILogger CreateLoggger()
        {
            throw new NotImplementedException();
        }
    }
}
