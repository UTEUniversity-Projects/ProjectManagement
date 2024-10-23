using ProjectManagement.Database;
using ProjectManagement.Mappers.Implement;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.DAOs
{
    internal class DAOUtils : DBConnection
    {
        public static bool CheckIsNotEmpty(string intput, string fieldName)
        {
            string sqlStr = "SELECT * FROM dbo.FUNC_IsNotEmpty(@Input, @FieldName)";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Input", intput),
                new SqlParameter("@FieldName", fieldName)
            };
            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);
            return dataTable.Rows.Count > 0 && Convert.ToBoolean(dataTable.Rows[0]["IsValid"]);
        }
        public static bool CheckIsValidInRange(double value, double minValue, double maxValue, string fieldName)
        {
            string sqlStr = "SELECT * FROM dbo.FUNC_IsValidInRange(@Input, @MinValue, @MaxValue, @FieldName)";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Input", value),
                new SqlParameter("@MinValue", minValue),
                new SqlParameter("@MaxValue", maxValue),
                new SqlParameter("@FieldName", fieldName)
            };
            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);
            return dataTable.Rows.Count > 0 && Convert.ToBoolean(dataTable.Rows[0]["IsValid"]);
        }
    }
}
