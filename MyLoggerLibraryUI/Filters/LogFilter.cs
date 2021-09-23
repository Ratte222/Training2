using System;
using System.Collections.Generic;
using System.Text;

namespace MyLoggerLibraryUI.Filters
{
    public class LogFilter
    {
        public string OrderBy { get; set; }
        public string OrderByField { get; set; }
        public int Take { get; set; } = 10;
        public int Skip { get; set; } = 0;
    }
}
