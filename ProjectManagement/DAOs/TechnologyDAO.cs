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
    internal class TechnologyDAO
    {
        public static List<Technology> SelectListByProject(string projectId)
        {
            string sqlStr = "SELECT * FROM FUNC_SelectTechnologiesByProject(@ProjectId)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ProjectId", projectId)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            List<Technology> technologies = new List<Technology>();

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                TechnologyMapper technologyMapper = new TechnologyMapper();

                foreach (DataRow row in dataTable.Rows)
                {
                    Technology technology = technologyMapper.MapRow(row);
                    technologies.Add(technology);
                }
            }

            return technologies;
        }
        public static List<Technology> SelectListByField(string fieldId)
        {
            string sqlStr = "SELECT * FROM FUNC_SelectTechnologiesByField(@FieldId)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@FieldId", fieldId)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            List<Technology> technologies = new List<Technology>();

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                TechnologyMapper technologyMapper = new TechnologyMapper();

                foreach (DataRow row in dataTable.Rows)
                {
                    Technology technology = technologyMapper.MapRow(row);
                    technologies.Add(technology);
                }
            }

            return technologies;
        }

        public static string GetListTechnology(string projectId)
        {
            List<Technology> technologies = SelectListByProject(projectId);
            return string.Join(", ", technologies.Select(t => t.Name));
        }
        public static Dictionary<string, int> TopTechnology()
        {
            string sqlStr = "SELECT * FROM FUNC_GetTopTechnology() ORDER BY ProjectCount DESC";

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, new List<SqlParameter>(), string.Empty);
            Dictionary<string, int> result = new Dictionary<string, int>();

            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    string technologyName = row["TechnologyName"].ToString();
                    int projectCount = Convert.ToInt32(row["ProjectCount"]);

                    result.Add(technologyName, projectCount);
                }
            }

            return result;
        }

    }
}
