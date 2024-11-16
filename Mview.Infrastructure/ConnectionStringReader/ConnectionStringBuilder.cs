using System;
using System.Data;
using System.Data.SqlClient;



namespace Mview.Infrastructure.ConnectionStringReader
{
    internal static class ConnectionStringBuilder
    {
        public static async Task<string> GetConnectionString(string connectionString)
        {

            var connectionStringBuilder = new SqlConnectionStringBuilder();

            connectionStringBuilder.ConnectionString = connectionString;

            return connectionStringBuilder.ConnectionString;
        }
    }
}
