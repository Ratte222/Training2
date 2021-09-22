using AuxiliaryLib.WorkWithJson;
using MyLoggerLibrary.Events;
using MyLoggerLibrary.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.IO;

namespace MyLoggerLibrary.Formatting
{
    public class JsonFormatter: IFormatter
    {
        
        JsonSerializer serializer = null;
        JsonTextWriter writer = null;

        //bool isDisposed = false;
        //public void Dispose()
        //{
        //    if(!isDisposed)
        //    {
                
        //    }
        //}

        public void Serialize(StreamWriter streamWriter, LogEvent logEvent)
        {
            if(serializer is null)
            {
                serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;
            }
            if(writer is null)
            {
                writer = new JsonTextWriter(streamWriter);
            }
            
            
            //writer.Formatting = Newtonsoft.Json.Formatting.Indented;
            //writer.Indentation = 4;
            //writer.IndentChar = ' ';
            serializer.Serialize(writer, logEvent);
            
        }

        public void Serialize(TextWriter textWriter, LogEvent logEvent)
        {
            if (serializer is null)
            {
                serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;
            }
            if (writer is null)
            {
                writer = new JsonTextWriter(textWriter);
            }

            serializer.Serialize(writer, logEvent);
        }
    }
}