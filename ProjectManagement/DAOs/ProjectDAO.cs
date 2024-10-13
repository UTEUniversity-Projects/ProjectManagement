﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
using Microsoft.VisualBasic.ApplicationServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            string sqlStr = $"SELECT * FROM {DBTableNames.Team} WHERE teamId = @TeamId";

            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@TeamId", teamId) };

            DataTable dt = DBExecution.ExecuteQuery(sqlStr, parameters);

            if (dt.Rows.Count > 0)
            {
                return SelectOnly(dt.Rows[0]["projectId"].ToString());
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
        public static List<Project> SelectListModeMyTheses(string userId)
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
        public static List<Project> SelectListModeMyCompletedTheses(string userId)
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

        public static void Insert(Project project)
        {
            DBExecution.Insert(project, DBTableNames.Project);
        }

        public static void Delete(Project project)
        {
            DBExecution.Delete(DBTableNames.Project, "projectId", project.ProjectId);
        }

        public static void Update(Project project)
        {
            DBExecution.Update(project, DBTableNames.Project, "projectId", project.ProjectId);
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
