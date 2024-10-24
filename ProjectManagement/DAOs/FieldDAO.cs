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
            string sqlStr = "SELECT * FROM FUNC_SelectFieldById(@FieldId)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@FieldId", fieldId)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                FieldMapper fieldMapper = new FieldMapper();
                return fieldMapper.MapRow(dataTable.Rows[0]);
            }

            return null;
        }
        public static List<Field> SelectList()
        {
            string sqlStr = "SELECT * FROM FUNC_SelectAllFields()";

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, new List<SqlParameter>(), string.Empty);

            List<Field> fields = new List<Field>();

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                FieldMapper fieldMapper = new FieldMapper();

                foreach (DataRow row in dataTable.Rows)
                {
                    Field field = fieldMapper.MapRow(row);
                    fields.Add(field);
                }
            }

            return fields;
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
