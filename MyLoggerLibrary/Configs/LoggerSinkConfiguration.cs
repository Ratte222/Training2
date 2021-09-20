using System;
using System.Collections.Generic;
using System.Text;

namespace MyLoggerLibrary.Configs
{
    internal enum WriteTo
    {
        Console,
        File
    }
    public class LoggerSinkConfiguration
    {
        #region fileConfiguration
        internal List<FileConfig> FileConfigs { get; set; } = new List<FileConfig>();
        //internal IFormatProvider fileFormatProvider;
        #endregion
        #region consoleConfiguration
        //internal IFormatProvider consoleFormatProvider;
        #endregion
        internal List<WriteTo> WritesTo { get; set; } = new List<WriteTo>();
    }
}
