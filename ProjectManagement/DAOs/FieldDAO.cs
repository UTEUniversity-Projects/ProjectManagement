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
    internal class FieldDAO
    {
        public static Field SelectOnlyById(string fieldId)
        {
            return DBGetModel.GetModel(DBTableNames.Field, "fieldId", fieldId, new FieldMapper());
        }
        public static List<Field> SelectList()
        {
            string sqlStr = string.Format("SELECT * FROM {0}", DBTableNames.Field);

            return DBGetModel.GetModelList(sqlStr, new List<SqlParameter>(), new FieldMapper());
        }
        public static Dictionary<string, int> TopField()
        {
            string sqlStr = "SELECT * FROM FUNC_GetTopField() ORDER BY ProjectCount DESC";

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, new List<SqlParameter>(), string.Empty);
            Dictionary<string, int> result = new Dictionary<string, int>();

            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    string fieldName = row["FieldName"].ToString();
                    int projectCount = Convert.ToInt32(row["ProjectCount"]);

                    result.Add(fieldName, projectCount);
                }
            }

            return result;
        }
    }
}
