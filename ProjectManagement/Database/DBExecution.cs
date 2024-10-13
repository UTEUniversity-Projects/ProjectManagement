using ProjectManagement.Mappers;
using ProjectManagement.Utils;
using System.Data;
using System.Data.SqlClient;

namespace ProjectManagement.Database
{
    internal class DBExecution
    {
        private static SqlConnection connection = DBConnection.GetConnection();

        #region SQL EXECUTION QUERY

        private static DataTable SQLExecuteQuery(string sqlStr, List<SqlParameter> parameters, string typeExecution, bool flag)
        {
            DataTable dataTable = new DataTable();

            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlStr, connection);

                foreach (var param in parameters)
                {
                    cmd.Parameters.Add(param);
                }

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dataTable);
                }

                if (flag)
                {
                    WinformControlUtil.ShowMessage("Notification", typeExecution + " successfully");
                }
            }
            catch (Exception ex)
            {
                WinformControlUtil.ShowMessage("Notification", typeExecution + " failed: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dataTable;

        }        
        
        public static DataTable ExecuteQuery(string sqlStr, List<SqlParameter> parameters)
        {
            return SQLExecuteQuery(sqlStr, parameters, string.Empty, false);
        }
        public static DataTable ExecuteQuery(string sqlStr, List<SqlParameter> parameters, string typeExecution)
        {
            return SQLExecuteQuery(sqlStr, parameters, typeExecution, true);
        }

        #endregion

        #region SQL EXECUTION NON QUERY

        private static void SQLExecuteNonQuery(string sqlStr, List<SqlParameter> parameters, string typeExecution, bool flag)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlStr, connection);

                foreach (var param in parameters)
                {
                    cmd.Parameters.Add(param);
                }

                if (cmd.ExecuteNonQuery() > 0 && flag)
                {
                    WinformControlUtil.ShowMessage("Notification", typeExecution + " successfully");
                }
            }
            catch (Exception ex)
            {
                WinformControlUtil.ShowMessage("Notification", typeExecution + " failed: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public static void ExecuteNonQuery(string sqlStr, List<SqlParameter> parameters)
        {
            SQLExecuteNonQuery(sqlStr, parameters, string.Empty, false);
        }
        public static void ExecuteNonQuery(string sqlStr, List<SqlParameter> parameters, string typeExecution)
        {
            SQLExecuteNonQuery(sqlStr, parameters, typeExecution, true);
        }

        #endregion

        #region CRUD OPERATION

        public static void Insert<T>(T model, string tableName)
        {
            string sqlStr = $"INSERT INTO {tableName} ({string.Join(", ", typeof(T).GetProperties().Select(p => p.Name))}) " +
                            $"VALUES ({string.Join(", ", typeof(T).GetProperties().Select(p => "@" + p.Name))})";

            List<SqlParameter> parameters = DBUtil.GetSqlParameters(model);
            ExecuteNonQuery(sqlStr, parameters);
        }

        public static void Update<T>(T model, string tableName, string primaryKeyName, object primaryKeyValue)
        {
            string sqlStr = $"UPDATE {tableName} SET {string.Join(", ", typeof(T).GetProperties().Select(p => p.Name + " = @" + p.Name))} " +
                            $"WHERE {primaryKeyName} = @{primaryKeyName}";

            List<SqlParameter> parameters = DBUtil.GetSqlParameters(model);
            parameters.Add(new SqlParameter("@" + primaryKeyName, primaryKeyValue));

            ExecuteNonQuery(sqlStr, parameters);
        }

        public static void Delete(string tableName, string primaryKeyName, object primaryKeyValue)
        {
            string sqlStr = $"DELETE FROM {tableName} WHERE {primaryKeyName} = @{primaryKeyName}";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@" + primaryKeyName, primaryKeyValue)
            };

            ExecuteNonQuery(sqlStr, parameters);
        }

        #endregion

    }
}
