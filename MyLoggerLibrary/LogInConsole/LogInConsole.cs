using MyLoggerLibrary.Configs;
using MyLoggerLibrary.Events;
using MyLoggerLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLoggerLibrary.LogInConsole
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
            _consoleConfig.TextWriter.WriteLine(logEvent.ToString());
        }

    }
}
