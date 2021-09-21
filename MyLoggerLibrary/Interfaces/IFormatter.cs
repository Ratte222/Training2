using MyLoggerLibrary.Events;
using System.IO;

namespace MyLoggerLibrary.Interfaces
{
    public interface IFormatter
    {
        void Serialize(StreamWriter streamWriter, LogEvent logEvent);
    }
}