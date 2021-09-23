using MyLoggerLibrary.Configs;
using MyLoggerLibrary.Events;
using MyLoggerLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyLoggerLibrary.Services
{
    internal class LogInConsole:ILog
    {
        private readonly ConsoleConfig _consoleConfig;

        public LogInConsole(ConsoleConfig consoleConfig)
        {
            _consoleConfig = consoleConfig;
        }

        public void Log(LogEvent logEvent)
        {
            try
            { _consoleConfig.Formatter.Serialize(Console.Out, logEvent); }
            catch(Exception ex)
            { }
            //_consoleConfig.TextWriter.WriteLine(logEvent.ToString());
        }

    }
}
