using System;
using System.Collections.Generic;
using System.Text;

namespace MyLoggerLibrary.Interfaces
{
    public interface ILogger
    {
        public void LogDebug(string message, params object?[]? args);
        public void LogInfo(string message, params object?[]? args);
        public void LogWarning(string message, params object?[]? args);
        public void LogErrore(string message, params object?[]? args);
        public void LogCritical(string message, params object?[]? args);
        public void Log(LogLevel logLevel, string message, Exception exception = null, params object?[]? args);
    }
}
