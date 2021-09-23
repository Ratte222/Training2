using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using MyLoggerLibraryUI;
using MyLoggerLibraryUI.Interface;
using MyLoggerMsSQLServerDataProvider.Config;
using MyLoggerMsSQLServerDataProvider.Services;

namespace MyLoggerMsSQLServerDataProvider
{
    public static class MyLibraryUiBuilderOptions
    {
        public static void UseSqlServer(
            this MyLoggerUiOptionsBuilder optionsBuilder,
            string connectionString,
            string tableName,
            string schemaName = "dbo"
        )
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));
            var config = new MsSqlServerConfig()
            {
                ConnectionString = connectionString,
                TableName = tableName,
                SchemaName = schemaName
            };
            ((IMyLoggerUiOptionsBuilder)optionsBuilder).Services.AddSingleton(config);
            ((IMyLoggerUiOptionsBuilder)optionsBuilder).Services.AddScoped<IDataProvider, MsSQLServerDataProvider>();
        }
    }
}
