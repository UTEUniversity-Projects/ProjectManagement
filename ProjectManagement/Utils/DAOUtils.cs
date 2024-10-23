using ProjectManagement.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Utils
{
    public class DAOUtils
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
        public static bool CheckStartDate(DateTime startAt, string fieldName)
        {
            string sqlStr = "SELECT IsValid FROM dbo.FUNC_CheckStartDate(@startAt, @fieldName)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@startAt", startAt),
                new SqlParameter("@fieldName", fieldName)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                return Convert.ToBoolean(dataTable.Rows[0]["IsValid"]);
            }
            return false;
        }
        public static bool CheckEndDate(DateTime startDate, DateTime endDate, string fieldName)
        {
            string sqlStr = "SELECT IsValid, Message FROM FUNC_CheckEndDate(@StartAt, @EndAt, @FieldName)";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@StartAt", startDate),
                new SqlParameter("@EndAt", endDate),
                new SqlParameter("@FieldName", fieldName)
            };
            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);
            return dataTable.Rows.Count > 0 && Convert.ToBoolean(dataTable.Rows[0]["IsValid"]);
        }
        public static bool CheckNonExist(string tableName, string field, string information)
        {
            string sqlStr = "EXEC PROC_CheckNonExist @TableName, @Field, @Information";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@TableName", tableName),
                new SqlParameter("@Field", field),
                new SqlParameter("@Information", information)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            if (dataTable.Rows.Count > 0)
            {
                bool isValid = Convert.ToBoolean(dataTable.Rows[0]["IsValid"]);
                return isValid;
            }

            return false;
        }
        public static bool CheckIsValidAge(DateTime dateOfBirth, string fieldName)
        {
            string sqlStr = "SELECT * FROM FUNC_CheckAge(@DateOfBirth, @FieldName)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@DateOfBirth", dateOfBirth),
                new SqlParameter("@FieldName", fieldName)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            if (dataTable.Rows.Count > 0)
            {
                bool isValid = Convert.ToBoolean(dataTable.Rows[0]["IsValid"]);
                return isValid;
            }
            return false;
        }
    }
}
