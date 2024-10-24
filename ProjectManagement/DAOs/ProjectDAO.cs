using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using ProjectManagement.Database;
using ProjectManagement.Models;
using ProjectManagement.Enums;
using ProjectManagement.Utils;
using ProjectManagement.Mappers.Implement;
using System.Data.SqlClient;
using System.Web;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.VisualBasic.ApplicationServices;
using System.Globalization;

namespace ProjectManagement.DAOs
{
    internal class ProjectDAO : DBConnection
    {

        #region SELECT PROJECT

        public static Project SelectOnly(string projectId)
        {
            string sqlStr = $"SELECT * FROM FUNC_GetProjectById(@projectId)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@projectId", projectId)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                ProjectMapper projectMapper = new ProjectMapper();
                return projectMapper.MapRow(dataTable.Rows[0]);
            }

            return null;
        }
        public static Project SelectFollowTeam(string teamId)
        {
            string sqlStr = $"SELECT projectId FROM FUNC_GetProjectIdByTeamId(@TeamId)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@TeamId", teamId)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                return SelectOnly(dataTable.Rows[0]["projectId"].ToString());
            }

            return new Project();
        }

        #endregion

        #region SELECT PROJECT FOLLOW ROLE

        public static List<Project> SelectListRoleLecture(string userId)
        {
            string sqlStr = "SELECT * FROM FUNC_GetProjectsByInstructorId(@UserId)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            List<Project> projectList = new List<Project>();

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                ProjectMapper projectMapper = new ProjectMapper();

                foreach (DataRow row in dataTable.Rows)
                {
                    Project project = projectMapper.MapRow(row);
                    projectList.Add(project);
                }
            }

            return projectList;
        }
        public static List<Project> SelectByLectureAndYear(string userId, int year)
        {
            string sqlStr = "SELECT * FROM FUNC_GetProjectByLectureAndYear(@UserId, @YearSelected)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@YearSelected", year)
            };
            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            List<Project> listProjects = new List<Project>();

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                ProjectMapper projectMapper = new ProjectMapper();
                foreach (DataRow row in dataTable.Rows)
                {
                    Project project = projectMapper.MapRow(row);
                    listProjects.Add(project);
                }
            }

            return listProjects;
        }
        public static List<Project> SelectListRoleStudent(string userId)
        {
            string sqlStr = "SELECT * FROM FUNC_GetProjectsForStudent(@UserId, @Published, @Registered)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@Published", EnumUtil.GetDisplayName(EProjectStatus.PUBLISHED)),
                new SqlParameter("@Registered", EnumUtil.GetDisplayName(EProjectStatus.REGISTERED))
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            List<Project> projectList = new List<Project>();

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                ProjectMapper projectMapper = new ProjectMapper();

                foreach (DataRow row in dataTable.Rows)
                {
                    Project project = projectMapper.MapRow(row);
                    projectList.Add(project);
                }
            }

            return projectList;
        }
        public static List<Project> SelectListModeMyProjects(string userId)
        {
            string sqlStr = "SELECT * FROM FUNC_GetMyProjects(@UserId)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            List<Project> projectList = new List<Project>();

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                ProjectMapper projectMapper = new ProjectMapper();

                foreach (DataRow row in dataTable.Rows)
                {
                    Project project = projectMapper.MapRow(row);
                    projectList.Add(project);
                }
            }

            return projectList;
        }
        public static List<Project> SelectListModeMyCompletedProjects(string userId)
        {
            string sqlStr = "SELECT * FROM FUNC_GetMyCompletedProjects(@UserId, @Completed)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@Completed", EnumUtil.GetDisplayName(EProjectStatus.COMPLETED))
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            List<Project> projectList = new List<Project>();

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                ProjectMapper projectMapper = new ProjectMapper();

                foreach (DataRow row in dataTable.Rows)
                {
                    Project project = projectMapper.MapRow(row);
                    projectList.Add(project);
                }
            }

            return projectList;
        }

        #endregion

        #region SEARCH PROJECT

        public static List<Project> SearchRoleLecture(string userId, string topic)
        {
            string sqlStr = "SELECT * FROM FUNC_SearchRoleLecture(@UserId, @TopicSyntax)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@TopicSyntax", topic + "%")
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            List<Project> projectList = new List<Project>();

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                ProjectMapper projectMapper = new ProjectMapper();

                foreach (DataRow row in dataTable.Rows)
                {
                    Project project = projectMapper.MapRow(row);
                    projectList.Add(project);
                }
            }

            return projectList;
        }
        public static List<Project> SearchRoleStudent(string topic)
        {
            string sqlStr = "SELECT * FROM FUNC_SearchRoleStudent(@TopicSyntax, @Published, @Registered)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@TopicSyntax", topic + "%"),
                new SqlParameter("@Published", EnumUtil.GetDisplayName(EProjectStatus.PUBLISHED)),
                new SqlParameter("@Registered", EnumUtil.GetDisplayName(EProjectStatus.REGISTERED))
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            List<Project> projectList = new List<Project>();

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                ProjectMapper projectMapper = new ProjectMapper();

                foreach (DataRow row in dataTable.Rows)
                {
                    Project project = projectMapper.MapRow(row);
                    projectList.Add(project);
                }
            }

            return projectList;
        }

        #endregion

        #region PROJECT DAO CRUD

        public static void Insert(Project project, List<Technology> technologies)
        {
            string sqlStr = "EXEC PROC_InsertProject @ProjectId, @InstructorId, @Topic, @Description, @Feature, @Requirement," +
                " @MaxMember, @Status, @CreatedAt, @CreatedBy, @FieldId";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ProjectId", project.ProjectId),
                new SqlParameter("@InstructorId", project.InstructorId),
                new SqlParameter("@Topic", project.Topic),
                new SqlParameter("@Description", project.Description),
                new SqlParameter("@Feature", project.Feature),
                new SqlParameter("@Requirement", project.Requirement),
                new SqlParameter("@MaxMember", project.MaxMember),
                new SqlParameter("@Status", EnumUtil.GetDisplayName(project.Status)),
                new SqlParameter("@CreatedAt", project.CreatedAt),
                new SqlParameter("@CreatedBy", project.CreatedBy),
                new SqlParameter("@FieldId", project.FieldId)
            };

            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, "Insert Project");

            InsertProjectTechnology(project.ProjectId, technologies);
        }
        public static void InsertProjectTechnology(string projectId, List<Technology> technologies)
        {
            string sqlStr = "EXEC PROC_InsertProjectTechnology @ProjectId, @TechnologyId";

            foreach (Technology technology in technologies)
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@ProjectId", projectId),
                    new SqlParameter("@TechnologyId", technology.TechnologyId)
                };

                DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
            }
        }
        
        public static void Update(Project project, List<Technology> technologies)
        {
            string sqlStr = "EXEC PROC_UpdateProject @ProjectId, @InstructorId, @Topic, @Description, @Feature, @Requirement, @MaxMember, @Status, @CreatedAt, @CreatedBy, @FieldId";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ProjectId", project.ProjectId),
                new SqlParameter("@InstructorId", project.InstructorId),
                new SqlParameter("@Topic", project.Topic),
                new SqlParameter("@Description", project.Description),
                new SqlParameter("@Feature", project.Feature),
                new SqlParameter("@Requirement", project.Requirement),
                new SqlParameter("@MaxMember", project.MaxMember),
                new SqlParameter("@Status", EnumUtil.GetDisplayName(project.Status)),
                new SqlParameter("@CreatedAt", project.CreatedAt),
                new SqlParameter("@CreatedBy", project.CreatedBy),
                new SqlParameter("@FieldId", project.FieldId)
            };

            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, "Update Project");

            InsertProjectTechnology(project.ProjectId, technologies);
        }
        public static void UpdateStatus(Project project, EProjectStatus status)
        {
            string sqlStr = "EXEC PROC_UpdateProjectStatus @ProjectId, @Status";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ProjectId", project.ProjectId),
                new SqlParameter("@Status", EnumUtil.GetDisplayName(status))
            };

            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }
        public static void UpdateFavorite(string userId, string projectId, bool isFavorite)
        {
            string sqlStr = "EXEC PROC_UpdateFavoriteProject @UserId, @ProjectId, @IsFavorite";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@IsFavorite", isFavorite)
            };

            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }

        public static void Delete(string projectId)
        {
            TaskDAO.DeleteFollowProject(projectId);
            TeamDAO.DeleteFollowProject(projectId);

            string sqlStr = "EXEC PROC_DeleteProject @ProjectId";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ProjectId", projectId)
            };

            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }

        #endregion

        #region PROJECT DAO UTIL

        public static List<string> SelectFavoriteList(string userId)
        {
            string sqlStr = "SELECT * FROM FUNC_GetFavoriteProjects(@UserId)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            List<string> favoriteProjects = new List<string>();

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    favoriteProjects.Add(row["projectId"].ToString());
                }
            }

            return favoriteProjects;
        }

        #endregion

        #region CHECK INFORMATION

        public static bool CheckIsFavoriteProject(string userId, string projectId)
        {
            string sqlStr = "SELECT dbo.FUNC_CheckIsFavoriteProject(@UserId, @ProjectId)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@ProjectId", projectId)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            return dataTable.Rows.Count > 0 && Convert.ToBoolean(dataTable.Rows[0][0]);
        }

        #endregion

        #region STATISTICAL

        public static DataTable CreateProjectTable(List<Project> listProjects)
        {
            DataTable projectTable = new DataTable();
            projectTable.Columns.Add("projectId", typeof(string));
            projectTable.Columns.Add("instructorId", typeof(string));
            projectTable.Columns.Add("topic", typeof(string));
            projectTable.Columns.Add("description", typeof(string));
            projectTable.Columns.Add("feature", typeof(string));
            projectTable.Columns.Add("requirement", typeof(string));
            projectTable.Columns.Add("maxMember", typeof(int));
            projectTable.Columns.Add("status", typeof(string));
            projectTable.Columns.Add("createdAt", typeof(DateTime));
            projectTable.Columns.Add("createdBy", typeof(string));
            projectTable.Columns.Add("fieldId", typeof(string));

            foreach (var project in listProjects)
            {
                projectTable.Rows.Add(
                    project.ProjectId, project.InstructorId, project.Topic, project.Description,
                    project.Feature, project.Requirement, project.MaxMember, EnumUtil.GetDisplayName(project.Status),
                    project.CreatedAt, project.FieldId);
            }
            return projectTable;
        }
        public static Dictionary<string, int> GroupedByMonth(List<Project> listProjects)
        {
            DataTable projectTable = CreateProjectTable(listProjects);
            string sqlStr = "SELECT MonthName, ProjectCount FROM dbo.FUNC_GetProjectsGroupedByMonth(@ProjectList)" +
                            "ORDER BY MonthNumber;";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                 new SqlParameter
                 {
                    ParameterName = "@ProjectList",
                    SqlDbType = SqlDbType.Structured, 
                    TypeName = "ProjectTableType", 
                    Value = projectTable
                 }
            };
            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            Dictionary<string, int> result = new Dictionary<string, int>();
            if (dataTable.Rows.Count > 0)
            {
                foreach(DataRow row in dataTable.Rows)
                {
                    string monthName = row["MonthName"].ToString();
                    int projectCount = Convert.ToInt32(row["ProjectCount"]);

                    result.Add(monthName, projectCount);
                }
            }
            return result;
        }
        public static Dictionary<EProjectStatus, int> GroupedByStatus(List<Project> listProjects)
        {
            DataTable projectTable = CreateProjectTable(listProjects);
            string sqlStr = "SELECT * FROM dbo.FUNC_GetProjectsGroupedByStatus(@ProjectList)";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                 new SqlParameter
                 {
                    ParameterName = "@ProjectList",
                    SqlDbType = SqlDbType.Structured,
                    TypeName = "ProjectTableType",
                    Value = projectTable
                 }
            };
            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            Dictionary<EProjectStatus, int> result = new Dictionary<EProjectStatus, int>();

            foreach (DataRow row in dataTable.Rows)
            {
                EProjectStatus status = EnumUtil.GetEnumFromDisplayName<EProjectStatus>(row["ProjectStatus"].ToString());
                int projectCount = Convert.ToInt32(row["ProjectCount"]);

                result.Add(status, projectCount);
            }
            return result;
        }
        public static Dictionary<string, int> GetTopField(List<Project> listProjects)
        {
            DataTable projectTable = CreateProjectTable(listProjects);
            string sqlStr = "SELECT * FROM FUNC_GetTopFieldsByProjects(@ProjectList) ORDER BY ProjectCount DESC";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                 new SqlParameter
                 {
                    ParameterName = "@ProjectList",
                    SqlDbType = SqlDbType.Structured,
                    TypeName = "ProjectTableType",
                    Value = projectTable
                 }
            };
            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            MessageBox.Show(dataTable.Rows.Count.ToString());

            Dictionary<string, int> result = new Dictionary<string, int>();

            foreach (DataRow row in dataTable.Rows)
            {
                string fieldName = row["FieldName"].ToString();
                int projectCount = Convert.ToInt32(row["ProjectCount"]);
                MessageBox.Show($"FieldName: {row["FieldName"]}, ProjectCount: {row["ProjectCount"]}");
                result.Add(fieldName, projectCount);
            }
            return result;
        }
        #endregion

    }
}
