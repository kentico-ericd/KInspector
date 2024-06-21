using System.Data.SqlClient;

using KInspector.Core.Models;

namespace KInspector.Core.Helpers
{
    public static class DatabaseHelper
    {
        public static SqlConnection GetSqlConnection(DatabaseSettings databaseSettings)
        {
            if (!string.IsNullOrEmpty(databaseSettings.ConnectionString))
            {
                return new SqlConnection(databaseSettings.ConnectionString);
            }
            else
            {
                SqlConnectionStringBuilder sb = new();
                if (databaseSettings.IntegratedSecurity)
                {
                    sb.IntegratedSecurity = true;
                }
                else
                {
                    sb.UserID = databaseSettings.User;
                    sb.Password = databaseSettings.Password;
                }

                sb["Server"] = databaseSettings.Server;
                sb["Database"] = databaseSettings.Database;

                return new SqlConnection(sb.ConnectionString);
            }
        }
    }
}