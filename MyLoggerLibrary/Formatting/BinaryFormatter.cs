using MyLoggerLibrary.Events;
using MyLoggerLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using bin = System.Runtime.Serialization.Formatters.Binary;

namespace MyLoggerLibrary.Formatting
{
    public class BinaryFormatter : IFormatter
    {
        public void Serialize(StreamWriter streamWriter, LogEvent logEvent)
        {
            bin.BinaryFormatter formatter = new bin.BinaryFormatter();
            formatter.Serialize(streamWriter.BaseStream, logEvent);
            
        }
    }
}
