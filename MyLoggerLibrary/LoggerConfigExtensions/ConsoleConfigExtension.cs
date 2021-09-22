using MyLoggerLibrary.Configs;
using MyLoggerLibrary.Interfaces;
using MyLoggerLibrary.Services;
using MyLoggerLibrary.Formatting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MyLoggerLibrary.LoggerConfigExtensions
{
    public static class ConsoleConfigExtension
    {
        public static LoggerConfiguration Console(this LoggerSinkConfiguration sinkConfiguration,
            IFormatter formatter = null/*, IFormatProvider formatProvider*/)
        {
            //formatProvider.GetFormat(typeof(CultureInfo));
            //sinkConfiguration.consoleFormatProvider = CultureInfo.CurrentCulture;
            //sinkConfiguration.WritesTo.Add(WriteTo.Console);
            ConsoleConfig consoleConfig = new ConsoleConfig();
            if(formatter is null)
            {
                consoleConfig.Formatter = new ConsoleFormatter();
            }
            else
            {
                consoleConfig.Formatter = formatter;
            }
            return sinkConfiguration.AddConsoleConfig(consoleConfig);
        }
    }
}
