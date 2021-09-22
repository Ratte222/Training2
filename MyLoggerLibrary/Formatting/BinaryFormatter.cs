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

        public void Serialize(TextWriter textWriter, LogEvent logEvent)
        {
            using(var stream = new MemoryStream())
            {
                bin.BinaryFormatter formatter = new bin.BinaryFormatter();
                formatter.Serialize(stream, logEvent);
                using (var reader = new StreamReader(stream))
                {
                    textWriter.WriteLine(reader.ReadToEnd());
                }
            }
        }
    }
}
