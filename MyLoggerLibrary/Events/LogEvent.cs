using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace MyLoggerLibrary.Events
{
    [Serializable]
    public class LogEvent
    {
        public LogEvent() { }
        public LogEvent(DateTimeOffset timestamp, LogLevel level, Exception ex, string message)
        {
            (Timestamp, LogLevel, Exception, Message) = (timestamp.ToString(), level.ToString(),
                Exception is null ? null : $"InnerException = {ex?.InnerException?.ToString()} \r\n" +
                            $"Message = {ex?.Message?.ToString()} \r\n" +
                            $"Source = {ex?.Source?.ToString()} \r\n" +
                            $"StackTrace = {ex?.StackTrace?.ToString()} \r\n" +
                            $"TargetSite = {ex?.TargetSite?.ToString()}", message);
        }
        //[JsonConverter(typeof(Newtonsoft.Json.Converters.IsoDateTimeConverter))]
        //public DateTimeOffset Timestamp { get; set; }
        public string Timestamp { get; set; }
        public string LogLevel { get; set; }
        //[XmlIgnore]
        //public Exception Exception { get; set; }
        public string Exception { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            //return base.ToString();
            return $"{Timestamp}, {LogLevel}, {Message}, " +
                $"{Exception}";
        }
    }
}
