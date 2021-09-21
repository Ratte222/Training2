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
        
        JsonSerializer serializer;
        JsonTextWriter writer;

        //bool isDisposed = false;
        //public void Dispose()
        //{
        //    if(!isDisposed)
        //    {
                
        //    }
        //}

        public void Serialize(StreamWriter streamWriter, LogEvent jsonData)
        {
            serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            writer = new JsonTextWriter(streamWriter);
            
            writer.Formatting = Newtonsoft.Json.Formatting.Indented;
            writer.Indentation = 4;
            writer.IndentChar = ' ';
            serializer.Serialize(writer, jsonData);
            
        }
    }
}