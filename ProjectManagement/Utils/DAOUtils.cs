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
        public static bool CheckIsNotEmpty(string input, string fieldName)
        {
            string sqlStr = "SELECT * FROM dbo.FUNC_IsNotEmpty(@Input, @FieldName)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Input", input),
                new SqlParameter("@FieldName", fieldName)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            return dataTable.Rows.Count > 0 && Convert.ToBoolean(dataTable.Rows[0]["IsValid"]);

        }
    }
}
