using System;
using System.Collections.Generic;
using System.Text;

namespace MyLoggerLibrary.Events
{
    public class LogEvent
    {
        public LogEvent(DateTimeOffset timestamp, LogLevel level, Exception exception, string message)
        {
            (Timestamp, LogLevel, Exception, Message) = (timestamp, level, exception, message);
        }
        public DateTimeOffset Timestamp { get; }
        public LogLevel LogLevel { get; }
        public Exception Exception { get; }
        public string Message { get; }

        public override string ToString()
        {
            //return base.ToString();
            return $"{Timestamp} LogLevel: {LogLevel}, {Message}, " +
                $"{(Exception is null ? null : Exception.ToString()) }";
        }
    }
}
