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
