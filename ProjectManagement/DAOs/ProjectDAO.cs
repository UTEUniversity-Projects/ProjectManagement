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

namespace ProjectManagement.DAOs
{
    internal class ProjectDAO : DBConnection
    {       

        #region SELECT PROJECT

        public static Project SelectOnly(string projectId)
        {
            return DBGetModel.GetModel(DBTableNames.Project, "projectId", projectId, new ProjectMapper());
        }
        public static Project SelectFollowTeam(string teamId)
        {
            string sqlStr = $"SELECT projectId FROM {DBTableNames.Team} WHERE teamId = @TeamId";

            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@TeamId", teamId) };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            if (dataTable.Rows.Count > 0)
            {
                return SelectOnly(dataTable.Rows[0]["projectId"].ToString());
            }
            return new Project();
        }

        #endregion

        #region SELECT PROJECT FOLLOW ROLE

        public static List<Project> SelectListRoleLecture(string userId)
        {
            string sqlStr = string.Format("SELECT * FROM {0} WHERE instructorId = @UserId",
                DBTableNames.Project);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId)
            };

            return DBGetModel.GetModelList(sqlStr, parameters, new ProjectMapper());
        }
        public static List<Project> SelectListRoleStudent(string userId)
        {
            string sqlStr = string.Format("SELECT {0}.* FROM {0} WHERE status IN (@Published, @Registered) " +
                                           "AND NOT EXISTS(SELECT 1 FROM {1} WHERE {1}.projectId = {0}.projectId " +
                                           "AND teamId IN (SELECT teamId FROM {2} WHERE studentId = @UserId))",
                                           DBTableNames.Project, DBTableNames.Team, DBTableNames.JoinTeam);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@Published", EnumUtil.GetDisplayName(EProjectStatus.PUBLISHED)),
                new SqlParameter("@Registered", EnumUtil.GetDisplayName(EProjectStatus.REGISTERED))
            };

            return DBGetModel.GetModelList(sqlStr, parameters, new ProjectMapper());
        }
        public static List<Project> SelectListModeMyProjects(string userId)
        {
            string sqlStr = string.Format("SELECT {0}.* FROM {0} INNER JOIN {1} ON {0}.projectId = {1}.projectId " +
                                           "WHERE {1}.teamId IN (SELECT teamId FROM {2} WHERE studentId = @UserId)",
                                            DBTableNames.Project, DBTableNames.Team, DBTableNames.JoinTeam);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId)
            };

            return DBGetModel.GetModelList(sqlStr, parameters, new ProjectMapper());
        }
        public static List<Project> SelectListModeMyCompletedProjects(string userId)
        {
            string sqlStr = string.Format("SELECT {0}.* FROM {0} INNER JOIN {1} ON {0}.projectId = {1}.projectId " + 
                                            "WHERE {1}.teamId IN (SELECT teamId FROM {2} WHERE studentId = @UserId) " + 
                                            "AND {0}.status = @Completed",
                                            DBTableNames.Project, DBTableNames.Team, DBTableNames.JoinTeam); 

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@Completed", EnumUtil.GetDisplayName(EProjectStatus.COMPLETED))
            };

            return DBGetModel.GetModelList(sqlStr, parameters, new ProjectMapper());
        }
        public static List<string> GetFavoriteList(string userId)
        {
            string sqlStr = string.Format("SELECT * FROM {0} WHERE userId = @UserId", DBTableNames.FavoriteProject);
            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@UserId", userId) };
            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            List<string> favoriteProjects = new List<string>();
            foreach (DataRow row in dataTable.Rows)
            {
                favoriteProjects.Add(row["projectId"].ToString());
            }

            return favoriteProjects;
        }

        #endregion

        #region SEARCH PROJECT

        public static List<Project> SearchRoleLecture(string userId, string topic)
        {
            string sqlStr = string.Format("SELECT * FROM {0} WHERE instructorId = @UserId AND topic LIKE @TopicSyntax",
                                DBTableNames.Project);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@TopicSyntax", topic + "%")
            };

            return DBGetModel.GetModelList(sqlStr, parameters, new ProjectMapper());

        }
        public static List<Project> SearchRoleStudent(string topic)
        {
            string sqlStr = string.Format("SELECT * FROM {0} WHERE status IN (@Published, @Registered) AND topic LIKE @TopicSyntax",
                                    DBTableNames.Project);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@TopicSyntax", topic + "%"),
                new SqlParameter("@Published", EnumUtil.GetDisplayName(EProjectStatus.PUBLISHED)),
                new SqlParameter("@Registered", EnumUtil.GetDisplayName(EProjectStatus.REGISTERED))
            };

            return DBGetModel.GetModelList(sqlStr, parameters, new ProjectMapper());
        }

        #endregion

        #region PROJECT DAO EXECUTION

        public static void Insert(Project project, List<Technology> technologies)
        {
            DBExecution.Insert(project, DBTableNames.Project, "Create");
            InsertProjectTechnology(project.ProjectId, technologies);       
        }
        private static void InsertProjectTechnology(string projectId, List<Technology> technologies)
        {
            string sqlStr = string.Format("INSERT INTO {0} (projectId, technologyId) VALUES (@ProjectId, @TechnologyId)",
                DBTableNames.ProjectTechnology);

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

        public static void Delete(string projectId)
        {
            TeamDAO.DeleteFollowProject(projectId);
            TaskDAO.DeleteFollowProject(projectId);

            DBExecution.Delete(DBTableNames.Meeting, "projectId", projectId);
            DBExecution.Delete(DBTableNames.GiveUp, "projectId", projectId);
            DBExecution.Delete(DBTableNames.ProjectTechnology, "projectId", projectId);
            DBExecution.Delete(DBTableNames.FavoriteProject, "projectId", projectId);

            DBExecution.Delete(DBTableNames.Project, "projectId", projectId);
        }

        public static void Update(Project project, List<Technology> technologies)
        {
            DBExecution.Delete(DBTableNames.ProjectTechnology, "projectId", project.ProjectId);
            DBExecution.Update(project, DBTableNames.Project, "projectId", project.ProjectId);
            InsertProjectTechnology(project.ProjectId, technologies);
        }
        public static void UpdateStatus(Project project, EProjectStatus status)
        {
            string sqlStr = string.Format("UPDATE {0} SET status = @Status WHERE projectId = @ProjectId",
                                                DBTableNames.Project);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ProjectId", project.ProjectId),
                new SqlParameter("@Status", EnumUtil.GetDisplayName(status))
            };

            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }
        public static void UpdateFavorite(string userId, string projectId, bool isFavorite) 
        {
            string sqlStr = string.Empty;

            if (isFavorite == false)
            {
                sqlStr = string.Format("DELETE FROM {0} WHERE userId = @UserId AND projectId = @ProjectId", DBTableNames.FavoriteProject);
            }
            else
            {
                sqlStr = string.Format("INSERT INTO {0} (userId, projectId) VALUES (@UserId, @ProjectId)", DBTableNames.FavoriteProject);
            }

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@ProjectId", projectId)
            };

            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }

        #endregion

        #region CHECK INFORMATION

        public static bool CheckIsFavorite(string userId, string projectId)
        {
            string sqlStr = string.Format("SELECT 1 FROM {0} WHERE userId = @UserId AND projectId = @ProjectId", DBTableNames.FavoriteProject);
           
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@ProjectId", projectId)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            return dataTable.Rows.Count > 0;
        }

        #endregion

    }
}
