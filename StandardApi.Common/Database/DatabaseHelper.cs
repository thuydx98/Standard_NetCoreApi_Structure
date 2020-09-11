using System;
using System.Data.SqlClient;
using System.Threading;

namespace StandardApi.Common.Database
{
    public static class DatabaseHelper
    {
        public static string CreateDatabaseIfNotExists(
            string connectionString,
            string collation = "",
            int triesToConnect = 10) => !SqlServerDatabaseExists(connectionString)
                ? CreateDatabase(connectionString, collation, triesToConnect)
                : string.Empty;

        public static string CreateDatabase(string connectionString, string collation, int triesToConnect = 10)
        {
            try
            {
                var builder = new SqlConnectionStringBuilder(connectionString);
                var databaseName = builder.InitialCatalog;
                builder.InitialCatalog = "master";
                var masterCatalogConnectionString = builder.ToString();
                var query = string.Format("CREATE DATABASE [{0}]", databaseName);
                if (!string.IsNullOrWhiteSpace(collation))
                {
                    query = string.Format("{0} COLLATE {1}", query, collation);
                }
                using (var conn = new SqlConnection(masterCatalogConnectionString))
                {
                    conn.Open();
                    using (var command = new SqlCommand(query, conn))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                if (triesToConnect > 0)
                {
                    for (var i = 0; i <= triesToConnect; i++)
                    {
                        if (i == triesToConnect)
                        {
                            throw new ArgumentOutOfRangeException("Unable to connect to the new database. Please try one more time");
                        }

                        if (!SqlServerDatabaseExists(connectionString))
                        {
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                return string.Empty;
            }
            catch (Exception)
            {
                return "Database Creation Error";
            }
        }

        public static bool SqlServerDatabaseExists(string connectionString)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
