using MyLoggerLibrary.Events;
using MyLoggerLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyLoggerLibrary.Formatting
{
    public class ConsoleFormatter : IFormatter
    {
        public void Serialize(StreamWriter streamWriter, LogEvent logEvent)
        {
            streamWriter.WriteLine(logEvent.ToString());
        }

        public void Serialize(TextWriter textWriter, LogEvent logEvent)
        {
            textWriter.WriteLine(logEvent.ToString());
        }
    }
}
