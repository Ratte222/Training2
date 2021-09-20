using MyLoggerLibrary.Configs;
using MyLoggerLibrary.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MyLoggerLibrary.LoggerConfigExtensions
{
    public static class ConsoleConfigExtension
    {
        public static LoggerSinkConfiguration Console(this LoggerSinkConfiguration sinkConfiguration/*, IFormatProvider formatProvider*/)
        {
            //formatProvider.GetFormat(typeof(CultureInfo));
            //sinkConfiguration.consoleFormatProvider = CultureInfo.CurrentCulture;
            sinkConfiguration.WritesTo.Add(WriteTo.Console);
            return sinkConfiguration;
        }
    }
}
