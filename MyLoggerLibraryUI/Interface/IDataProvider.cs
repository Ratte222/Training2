using MyLoggerLibrary.Model;
using MyLoggerLibraryUI.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLoggerLibraryUI.Interface
{
    public interface IDataProvider
    {
        IEnumerable<Log> FetchLogs(LogFilter logFilter);
        long CountLogs(LogFilter logFilter);
    }
}
