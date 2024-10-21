using ProjectManagement.Mappers;
using ProjectManagement.Utils;
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

                if (property.PropertyType.IsEnum)
                {
                    value = EnumUtil.GetDisplayName((Enum)value);
                } 
                else if (property.PropertyType == typeof(DateTime))
                {
                    DateTime dateTime = (DateTime)value;
                    value = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else if (property.PropertyType == typeof(DateTime?))
                {
                    value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else if (property.PropertyType == typeof(bool)) 
                {
                    bool boolValue = (bool)value;
                    value = boolValue ? 1 : 0;
                }
                else if (property.PropertyType == typeof(int))
                {
                    value = (int)value;
                }
                else if(property.PropertyType == typeof(long))
                {
                    value = (long)value;
                }
                else if (property.PropertyType == typeof(float))
                {
                    value = Convert.ToSingle(value);
                }
                else if (property == typeof(double))
                {
                    value = Convert.ToDouble(value);
                }

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
            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, new List<SqlParameter>(), string.Empty);
            string str = dataTable.Rows[0]["MaxID"].ToString();
            return Convert.ToInt32(str.Substring(str.Length - 5));
        }
    }
}
