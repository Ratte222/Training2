using MyLoggerLibrary.Events;
using MyLoggerLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace MyLoggerLibrary.Formatting
{
    public class XmlFormatter : IFormatter
    {
        public void Serialize(StreamWriter streamWriter, LogEvent logEvent)
        {
            var serializer = new XmlSerializer(typeof(LogEvent));
                serializer.Serialize(streamWriter, logEvent);            
        }
    }
}
