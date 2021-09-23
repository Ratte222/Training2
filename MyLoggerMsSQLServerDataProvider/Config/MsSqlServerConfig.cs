using System;
using System.Collections.Generic;
using System.Text;

namespace MyLoggerMsSQLServerDataProvider.Config
{
    class MsSqlServerConfig
    {
        public string ConnectionString { get; set; }
        public string TableName { get; set; }
        public string SchemaName { get; set; }
    }
}
