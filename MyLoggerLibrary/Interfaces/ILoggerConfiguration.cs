using System;
using System.Collections.Generic;
using System.Text;

namespace MyLoggerLibrary.Interfaces
{
    public interface ILoggerConfiguration
    {
        public ILogger CreateLoggger();
    }
}
