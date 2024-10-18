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

            DataTable dataTable = DBExecution.ExecuteQuery(sqlStr, parameters);

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
        public static List<Project> SelectListModeNotCompleted(string userId)
        {
            string sqlStr = string.Format("SELECT {0}.* FROM {0} INNER JOIN {1} ON {0}.projectId = {1}.projectId " +
                                           "WHERE {1}.teamId IN (SELECT teamId FROM {2} WHERE studentId = @UserId) " +
                                           "AND {0}.status = @GaveUp",
                                            DBTableNames.Project, DBTableNames.Team, DBTableNames.JoinTeam);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@GaveUp", EnumUtil.GetDisplayName(EProjectStatus.GAVEUP))
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

                DBExecution.ExecuteNonQuery(sqlStr, parameters);
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

            DBExecution.ExecuteNonQuery(sqlStr, parameters);
        }
        public static void UpdateFavorite(Project project) { }

        #endregion

    }
}
