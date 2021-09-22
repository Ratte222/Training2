using MyLoggerLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyLoggerLibrary.Configs
{
    internal class ConsoleConfig
    {
        public IFormatter Formatter { get; set; }
    }
}
