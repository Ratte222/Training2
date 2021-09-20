using System;
using System.Collections.Generic;
using System.Text;

namespace MyLoggerLibrary.Interfaces
{
    public interface ILogger
    {
        public void LogDebug(string message, object[] args);
        public void LogInfo(string message, object[] args);
        public void LogWarning(string message, object[] args);
        public void LogErrore(string message, object[] args);
        public void LogCritical(string message, object[] args);
        public void Log(LogLevel logLevel, string message, object[] args);
    }
}
