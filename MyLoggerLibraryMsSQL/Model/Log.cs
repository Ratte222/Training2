using System;
using System.Collections.Generic;
using System.Text;

namespace MyLoggerLibraryMsSQL.Model
{
    class Log
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public string MessageTemplate { get; set; }
        //[StringLength(128)]
        public string Level { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Exception { get; set; }
        public string Properties { get; set; }
    }
}
