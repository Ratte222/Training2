using MyLoggerLibrary.Configs;
using MyLoggerLibrary.Events;
using MyLoggerLibrary.Interfaces;
using System;
using System.IO;
using System.Threading;

namespace MyLoggerLibrary.LogInFile
{
    class LogInFile:ILog, IDisposable
    {
        private FileConfig _fileConfig;
        StreamWriter _streamWriter = null;
        private bool _isDisposed = false;
        private string _fileNameWithoutExtension;
        private Timer _timerStreamWriterFlush = null;
        public LogInFile(FileConfig fileConfig)
        {
            _fileConfig = fileConfig;
            

            
        }

        private string GenerateFileName()
        {
            string temp = Path.GetFileNameWithoutExtension(_fileConfig.Path);
            if (_fileConfig.RollingInterval == RollingInterval.Year)
            { temp = $"{temp}{DateTime.Now.ToString("yyyy")}"; }
            else if (_fileConfig.RollingInterval == RollingInterval.Month)
            { temp = $"{temp}{DateTime.Now.ToString("yyyyMM")}"; }
            else if (_fileConfig.RollingInterval == RollingInterval.Day)
            { temp = $"{temp}{DateTime.Now.ToString("yyyyMMdd")}"; }
            else if (_fileConfig.RollingInterval == RollingInterval.Hour)
            { temp = $"{temp}{DateTime.Now.ToString("yyyyMMddhh")}"; }
            else if (_fileConfig.RollingInterval == RollingInterval.Minute)
            { temp = $"{temp}{DateTime.Now.ToString("yyyyMMddhhmm")}"; }
            _fileNameWithoutExtension = $"{temp}{Path.GetExtension(_fileConfig.Path)}";
            return _fileNameWithoutExtension;
        }

        public void Dispose()
        {
            if(!_isDisposed)
            {
                _timerStreamWriterFlush?.Dispose();
                _streamWriter?.Flush();
                _streamWriter?.Dispose();
                _isDisposed = true;
            }
        }

        public void Log(LogEvent logEvent)
        {
            PrepareToSave();
            _fileConfig.Formatter.Serialize(_streamWriter, logEvent);
            AfterSave();
        }

        private void AfterSave()
        {
            if ((_timerStreamWriterFlush is null) &&
                (_fileConfig.FlushToDiskInterval.HasValue))
            {
                _timerStreamWriterFlush = new Timer(new TimerCallback(StreamWriterFlush), null,
                   1000, Convert.ToInt32(_fileConfig.FlushToDiskInterval.Value.TotalSeconds));
            }
            else
            {
                StreamWriterFlush(null);
            }
        }

        private void PrepareToSave()
        {
            if (_streamWriter is null)
            {
                string path = Path.GetDirectoryName(_fileConfig.Path);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                _streamWriter = new StreamWriter(Path.Combine(path, GenerateFileName()),
                    true, _fileConfig.Encoding, 2048);
            }
            //_streamWriter.WriteLine(content);
            
        }
        void StreamWriterFlush(object state)
        {
            _streamWriter?.Flush();
        }
    }
}