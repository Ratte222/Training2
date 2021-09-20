using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyLoggerLibrary.Configs
{
    internal class FileConfig
    {
        internal Encoding Encoding { get; set; }
        internal TimeSpan? FlushToDiskInterval { get; set; }
        internal long? FileSizeLimitBytes { get; set; }
        internal string Path { get; set; }
        internal RollingInterval RollingInterval { get; set; }
        internal bool RollOnFileSizeLimit { get; set; }
        public TextWriter TextWriter { get; set; }
    }
}
