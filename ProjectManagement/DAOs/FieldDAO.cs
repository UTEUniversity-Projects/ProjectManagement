using ProjectManagement.Database;
using ProjectManagement.Mappers.Implement;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
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
