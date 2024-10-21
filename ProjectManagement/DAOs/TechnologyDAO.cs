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
    internal class TechnologyDAO
    {
        public static List<Technology> SelectListByProject(string projectId)
        {
            string sqlStr = string.Format("SELECT T.* FROM {0} AS T " +
                "JOIN (SELECT technologyId FROM {1} WHERE projectId = @ProjectId) AS PT ON T.technologyId = PT.technologyId",
                DBTableNames.Technology, DBTableNames.ProjectTechnology);

            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@ProjectId", projectId) };

            return DBGetModel.GetModelList(sqlStr, parameters, new TechnologyMapper());
        }
        public static List<Technology> SelectListByField(string fieldId)
        {
            string sqlStr = string.Format("SELECT T.* FROM {0} AS T " +
                "JOIN (SELECT technologyId FROM {1} WHERE fieldId = @FieldId) AS PT ON T.technologyId = PT.technologyId",
                DBTableNames.Technology, DBTableNames.FieldTechnology);

            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@FieldId", fieldId) };

            return DBGetModel.GetModelList(sqlStr, parameters, new TechnologyMapper());
        }
        public static string GetListTechnology(string projectId)
        {
            List<Technology> technologies = SelectListByProject(projectId);
            return string.Join(", ", technologies.Select(t => t.Name));
        }
        public static Dictionary<string, int> TopTechnology()
        {
            string query = @"
            SELECT TOP 5 t.name AS TechnologyName, COUNT(ft.technologyId) AS ProjectCount
            FROM FieldTechnology ft
            JOIN Technology t ON ft.technologyId = t.TechnologyId
            GROUP BY t.name
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
                        string technologyName = reader["TechnologyName"].ToString();
                        int projectCount = Convert.ToInt32(reader["ProjectCount"]);

                        results.Add(technologyName, projectCount);
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
