using ProjectManagement.Mappers;
using ProjectManagement.Utils;
using System.Data;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ProjectManagement.Database
{
    internal class DBExecution
    {

        #region SQL EXECUTION QUERY

        public static DataTable SQLExecuteQuery(string sqlStr, List<SqlParameter> parameters, string typeExecution)
        {
            DataTable dataTable = new DataTable();
            SqlConnection connection = DBConnection.GetConnection();

            try
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sqlStr, connection))
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }

                if (!string.IsNullOrEmpty(typeExecution))
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

        #endregion

        #region SQL EXECUTION NON QUERY

        public static void SQLExecuteNonQuery(string sqlStr, List<SqlParameter> parameters, string typeExecution)
        {
            SqlConnection connection = DBConnection.GetConnection();

            try
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sqlStr, connection))
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }

                    if (cmd.ExecuteNonQuery() > 0 && !string.IsNullOrEmpty(typeExecution))
                    {
                        WinformControlUtil.ShowMessage("Notification", typeExecution + " successfully");
                    }
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

        #endregion

        #region CRUD OPERATION

        public static void Insert<T>(T model, string tableName)
        {
            string sqlStr = $"INSERT INTO {tableName} ({string.Join(", ", typeof(T).GetProperties().Select(p => char.ToLower(p.Name[0]) + p.Name.Substring(1)))}) " +
                            $"VALUES ({string.Join(", ", typeof(T).GetProperties().Select(p => "@" + p.Name))})";

            List<SqlParameter> parameters = DBUtil.GetSqlParameters(model);
            SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }
        public static void Insert<T>(T model, string tableName, string typeExecution)
        {
            string sqlStr = $"INSERT INTO {tableName} ({string.Join(", ", typeof(T).GetProperties().Select(p => char.ToLower(p.Name[0]) + p.Name.Substring(1)))}) " +
                            $"VALUES ({string.Join(", ", typeof(T).GetProperties().Select(p => "@" + p.Name))})";

            List<SqlParameter> parameters = DBUtil.GetSqlParameters(model);
            SQLExecuteNonQuery(sqlStr, parameters, typeExecution);
        }

        public static void Update<T>(T model, string tableName, string primaryKeyName, string primaryKeyValue)
        {
            string sqlStr = $"UPDATE {tableName} SET {string.Join(", ", typeof(T).GetProperties().Select(p => char.ToLower(p.Name[0]) + p.Name.Substring(1) + " = @" + p.Name))} " +
                            $"WHERE {primaryKeyName} = @{primaryKeyName + "KEY"}";

            List<SqlParameter> parameters = DBUtil.GetSqlParameters(model);
            parameters.Add(new SqlParameter("@" + primaryKeyName + "KEY", primaryKeyValue));

            SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }

        public static void Delete(string tableName, string primaryKeyName, object primaryKeyValue)
        {
            string sqlStr = $"DELETE FROM {tableName} WHERE {primaryKeyName} = @{primaryKeyName}";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@" + primaryKeyName, primaryKeyValue)
            };

            SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }

        #endregion
    }
}
