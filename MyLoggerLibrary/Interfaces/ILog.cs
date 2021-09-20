using MyLoggerLibrary.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLoggerLibrary.Interfaces
{
    public interface ILog
    {
        public void Log(LogEvent logEvent);
    }
}
