using MyLoggerLibrary.Events;
using MyLoggerLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyLoggerLibrary.Services
{
    public class Logger : ILogger
    {
        //delegate void ActionLog(LogEvent logEvent, TextWriter textWriter);
        event Action<LogEvent> _Log;
        public Logger(List<ILog> logs)
        {
            foreach(var act in logs)
            {
                _Log += act.Log;
            }
        }

        public void Log(LogLevel logLevel, string message, Exception exception = null, params object?[]? args)
        {
            if (message is null) return;
            if (args != null &&
                args.GetType() != typeof(object[]))
                args = new object[] { args };
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"{nameof(message)}: {message} ");
            int i = 0;
            foreach(var arg in args)
            {
                stringBuilder.Append($"{nameof(arg)}{i++.ToString()}: {arg.ToString()} ");
            }
            LogEvent logEvent = new LogEvent(DateTime.Now, logLevel, exception, stringBuilder.ToString());
            _Log?.Invoke(logEvent);
        }

        public void LogCritical(string message, params object?[]? args)
        {
            Log(LogLevel.Critical, message, null, args);
        }

        public void LogDebug(string message, params object?[]? args)
        {
            Log(LogLevel.Debug, message, null, args);
        }

        public void LogErrore(string message, params object?[]? args)
        {
            Log(LogLevel.Errore, message, null, args);
        }

        public void LogInfo(string message, params object?[]? args)
        {
            Log(LogLevel.Info, message, null, args);
        }

        public void LogWarning(string message, params object?[]? args)
        {
            Log(LogLevel.Warning, message, null, args);
        }
    }
}
