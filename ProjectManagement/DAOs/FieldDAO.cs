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
            string query = @"
            SELECT TOP 5 f.name AS FieldName, COUNT(p.FieldId) AS ProjectCount
            FROM Project p
            JOIN Field f ON p.FieldId = f.fieldId
            GROUP BY f.name
            ORDER BY ProjectCount DESC;";
            var results = new Dictionary<string, int>();
            SqlConnection connection = DBConnection.GetConnection();

            try
            {
                connection.Open();

                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string fieldName = reader["FieldName"].ToString();
                        int projectCount = Convert.ToInt32(reader["ProjectCount"]);

                        results.Add(fieldName, projectCount);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }

            return results;
        }
    }
}
