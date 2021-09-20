using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyLoggerLibrary.Configs
{
    internal class ConsoleConfig
    {
        public TextWriter TextWriter { get; set; } = Console.Out;
    }
}
