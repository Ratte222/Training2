using System;
using System.Collections.Generic;
using System.Linq;
using MyLoggerLibrary.Model;
using MyLoggerLibraryUI.Filters;
using MyLoggerLibraryUI.Interface;
using MyLoggerMsSQLServerDataProvider.Config;
using AuxiliaryLib.Extensions;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace MyLoggerMsSQLServerDataProvider.Services
{
    class MsSQLServerDataProvider : IDataProvider
    {
        private readonly MsSqlServerConfig _config;

        public MsSQLServerDataProvider(MsSqlServerConfig config)
        {
            _config = config;
        }

        public IEnumerable<Log> FetchLogs(LogFilter logFilter)
        {
            CheckFilter(logFilter);
            string query = $"SELECT * FROM {_config.TableName}" +
                $" ORDER BY {logFilter.OrderByField} {logFilter.OrderBy} " +
                    $"OFFSET {logFilter.Skip} ROWS FETCH NEXT {logFilter.Take} ROWS ONLY";
            using (IDbConnection db = new SqlConnection(_config.ConnectionString))
            {
                return db.Query<Log>(query);
            }
        }

        public long CountLogs(LogFilter logFilter)
        {
            CheckFilter(logFilter);
            long count = 0;
            using(SqlCommand command = new SqlCommand())
            {
                using (SqlConnection sqlConnection = new SqlConnection(_config.ConnectionString))
                {
                    command.CommandText = $"SELECT Count(*) FROM {_config.TableName}";
                    sqlConnection.Open();
                    command.Connection = sqlConnection;
                    string response = "";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response = reader[0].ToString();
                        }
                    }
                    long.TryParse(response, out count);
                }
            }
            return count;
        }

        private void CheckFilter(LogFilter logFilter)
        {
            if (String.IsNullOrEmpty(logFilter.OrderBy))
            {
                logFilter.OrderBy = "desc";
            }
            else if (!String.Equals(logFilter.OrderBy, "asc") && !String.Equals(logFilter.OrderBy, "desc"))
            {
                logFilter.OrderBy = "desc";
            }
            if (!(typeof(Log).GetAllPublicGetProperty<string>().Any(i => i == logFilter.OrderByField)))
            {
                Log log = new Log();
                logFilter.OrderByField = nameof(log.TimeStamp);
            }
        }
    }
}
