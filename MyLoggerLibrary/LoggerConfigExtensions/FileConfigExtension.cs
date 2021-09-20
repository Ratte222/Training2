using MyLoggerLibrary.Configs;
using MyLoggerLibrary.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLoggerLibrary.LoggerConfigExtensions
{
    public static class FileConfigExtension
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileConfig"></param>
        /// <param name="path">Path to the file.</param>
        /// <param name="rollingInterval">The interval at which logging will roll over to a new file.</param>
        /// <param name="flushToDiskInterval">If provided, a full disk flush will be performed periodically at the specified
        //     interval.</param>
        /// <param name="fileSizeLimitBytes">The approximate maximum size, in bytes, to which a log file will be allowed to
        //     grow. For unrestricted growth, pass null. The default is 1 GB. To avoid writing
        //     partial events, the last event within the limit will be written in full even
        //     if it exceeds the limit.</param>
        /// <param name="encoding">Character encoding used to write the text file. The default is UTF-8 without
        //     BOM.</param>
        /// <param name="rollOnFileSizeLimit">If
        //     true
        //     , a new file will be created when the file size limit is reached. Filenames will
        //     have a number appended in the format
        //     _NNN
        //     , with the first filename given no number.</param>
        /// <returns></returns>
        public static LoggerConfiguration File(this LoggerSinkConfiguration sinkConfiguration,
            string path, RollingInterval rollingInterval = RollingInterval.Infinite, TimeSpan? flushToDiskInterval = null,
            long fileSizeLimitBytes = 1073741824, Encoding encoding = null, bool rollOnFileSizeLimit = true)
        {
            FileConfig fileConfig = new FileConfig();
            if (String.IsNullOrEmpty(path)) throw new ArgumentNullException("Path is null or empty");
            else fileConfig.Path = path;
            fileConfig.RollingInterval = rollingInterval;
            if (flushToDiskInterval.HasValue)
            { fileConfig.FlushToDiskInterval = flushToDiskInterval; }
            else
            { fileConfig.FlushToDiskInterval = TimeSpan.FromMinutes(1); }
            fileConfig.FileSizeLimitBytes = fileSizeLimitBytes;
            if (encoding is null)
            { fileConfig.Encoding = Encoding.UTF8; }
            else
            { fileConfig.Encoding = encoding; }
            fileConfig.RollOnFileSizeLimit = rollOnFileSizeLimit;
            
            return sinkConfiguration.AddFileConfig(fileConfig);
        }
    }
}
