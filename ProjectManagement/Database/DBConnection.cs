using System.Data.SqlClient;

namespace ProjectManagement.Database
{
    internal class DBConnection
    {
        private static SqlConnection connection = new SqlConnection(Properties.Settings.Default.conStr);

        public static SqlConnection GetConnection() { return connection; }
                    
    }
}
