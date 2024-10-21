using System.Data.SqlClient;

namespace ProjectManagement.Database
{
    internal class DBConnection
    {
        private static readonly SqlConnection connection = new(Properties.Settings.Default.conStr);

        public static SqlConnection GetConnection() { return connection; }
                    
    }
}
