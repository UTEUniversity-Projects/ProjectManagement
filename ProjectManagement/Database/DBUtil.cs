using ProjectManagement.Mappers;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace ProjectManagement.Database
{
    internal class DBUtil
    {
        public static List<SqlParameter> GetSqlParameters<T>(T model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                string paramName = "@" + property.Name;
                object value = property.GetValue(model);
                SqlParameter parameter = new SqlParameter(paramName, value ?? DBNull.Value);

                parameters.Add(parameter);
            }

            return parameters;
        }

        public static int GetMaxId(string tableName, string columnName, string condition = "")
        {
            string query = string.Format("SELECT MAX({0}) AS MaxID FROM {1}", columnName, tableName);

            if (!string.IsNullOrEmpty(condition))
            {
                query += " WHERE " + condition;
            }

            return GetLastId(query);
        }

        private static int GetLastId(string sqlStr)
        {
            DataTable dataTable = DBExecution.ExecuteQuery(sqlStr, new List<SqlParameter>());
            string str = dataTable.Rows[0]["MaxID"].ToString();
            return Convert.ToInt32(str.Substring(str.Length - 5));
        }
    }
}
