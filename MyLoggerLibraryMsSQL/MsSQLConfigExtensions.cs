using MyLoggerLibrary.Configs;
using MyLoggerLibrary.Formatting;
using MyLoggerLibrary.Interfaces;
using MyLoggerLibrary.Services;
using MyLoggerLibraryMsSQL.Configs;
using MyLoggerLibraryMsSQL.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLoggerLibraryMsSQL
{
    public static class MsSQLConfigExtensions
    {
        public static LoggerConfiguration MsSQLServer(this LoggerSinkConfiguration sinkConfiguration, string connectionString,
            string tableName, string schemaName = "dbo", IFormatter formatter = null)
        {
            MsSQLConfig config = new MsSQLConfig();
            if (String.IsNullOrEmpty(connectionString)) throw new ArgumentNullException($"{nameof(connectionString)} is null or empty");
            if (String.IsNullOrEmpty(tableName)) throw new ArgumentNullException($"{nameof(tableName)} is null or empty");
            if (String.IsNullOrEmpty(schemaName)) throw new ArgumentNullException($"{nameof(schemaName)} is null or empty");
            config.ConnectionString = connectionString;
            config.TableName = tableName;
            config.SchemaName = schemaName;
            if(formatter is null)
            {
                config.Formatter = new ConsoleFormatter();
            }
            else
            {
                config.Formatter = formatter;
            }
            return sinkConfiguration.AddAction(new LogInMsSQL(config));
        }
    }
}
