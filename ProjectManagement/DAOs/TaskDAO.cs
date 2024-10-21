using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Database;
using ProjectManagement.Models;
using ProjectManagement.Mappers.Implement;
using System.Data.SqlClient;
using ProjectManagement.Enums;
using ProjectManagement.Utils;
using ProjectManagement.MetaData;
using Microsoft.VisualBasic.ApplicationServices;

namespace ProjectManagement.DAOs
{
    internal class TaskDAO : DBConnection
    {

        #region SELECT TASKS

        public static Tasks SelectOnly(string taskId)
        {
            string sqlStr = string.Format("SELECT * FROM {0} WHERE taskId = @TaskId", DBTableNames.Task);


            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@TaskId", taskId)
            };

            return DBGetModel.GetModel(sqlStr, parameters, new TaskMapper());
        }
        public static List<Tasks> SelectListByTeam(string teamId)
        {
            string sqlStr = string.Format("SELECT {0}.* FROM {0} INNER JOIN {1} ON {0}.projectId = {1}.projectId " +
                                            "WHERE teamId = @TeamId AND status = @Accepted " + "" +
                                            "ORDER BY {0}.createdAt DESC",
                                            DBTableNames.Task, DBTableNames.Team);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@TeamId", teamId),
                new SqlParameter("@Accepted", EnumUtil.GetDisplayName(ETeamStatus.ACCEPTED))
            };

            return DBGetModel.GetModelList(sqlStr, parameters, new TaskMapper());
        }
        public static List<TaskMeta> SelectListTaskMeta(string userId, string teamId, string projectId)
        {
            List<Tasks> tasks = SelectListByTeam(teamId);
            List<string> favoriteTasks = SelectFavoriteList(userId, projectId);

            List<TaskMeta> taskMetas = new List<TaskMeta>();
            foreach (Tasks task in tasks)
            {
                taskMetas.Add(new TaskMeta(task, favoriteTasks.Contains(task.TaskId)));
            }

            return taskMetas;
        }
        public static List<Tasks> SelectListTaskByStudent(string studentId)
        {
            string sqlStr = string.Format("SELECT {0}.* FROM {0} INNER JOIN {1} ON {0}.taskId = {1}.taskId " +
                                            "WHERE projectId = @projectId AND studentId = @studentId ORDER BY {0}.createdAt DESC",
                                            DBTableNames.Task, DBTableNames.TaskStudent);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@studentId", studentId)
            };

            return DBGetModel.GetModelList(sqlStr, parameters, new TaskMapper());
        }
        private static List<string> SelectFavoriteList(string userId, string projectId)
        {
            string sqlStr = string.Format("SELECT FT.taskId FROM {0} AS FT " +
                "JOIN (SELECT taskId FROM {1} WHERE projectId = @ProjectId) AS T " +
                "ON FT.taskId = T.taskId " +
                "WHERE userId = @UserId", DBTableNames.FavoriteTask, DBTableNames.Task);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@UserId", userId)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            List<string> list = new List<string>();
            foreach (DataRow row in  dataTable.Rows)
            {
                list.Add(row["taskId"].ToString());
            }

            return list;
        }

        #endregion

        #region TASK DAO EXECUTION

        public static void Insert(Tasks task)
        {
            DBExecution.Insert(task, DBTableNames.Task, "Create task");
        }
        public static void InsertAssignStudent(string taskId, string studentId)
        {
            string sqlStr = string.Format("INSERT INTO {0} (taskId, studentId) VALUES (@TaskId, @StudentId)", DBTableNames.TaskStudent);
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@TaskId", taskId),
                new SqlParameter("StudentId", studentId)
            };
            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }

        public static void Delete(string taskId)
        {
            DBExecution.Delete(DBTableNames.Evaluation, "taskId", taskId);
            DBExecution.Delete(DBTableNames.Comment, "taskId", taskId);

            DBExecution.Delete(DBTableNames.TaskStudent, "taskId", taskId);
            DBExecution.Delete(DBTableNames.FavoriteTask, "taskId", taskId);

            DBExecution.Delete(DBTableNames.Task, "taskId", taskId);
        }
        public static void DeleteFollowProject(string projectId)
        {
            string sqlStr = string.Format("SELECT taskId FROM {0} WHERE projectId = @ProjectId", DBTableNames.Task);

            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@ProjectId", projectId) };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            foreach (DataRow row in dataTable.Rows)
            {
                Delete(row["taskId"].ToString());
            }
        }

        public static void Update(Tasks task)
        {
            DBExecution.Update(task, DBTableNames.Task, "taskId", task.TaskId);
        }
        public static void UpdateFavorite(string userId, string taskId, bool isFavorite)
        {
            string sqlStr = string.Empty;

            if (isFavorite == false)
            {
                sqlStr = string.Format("DELETE FROM {0} WHERE userId = @UserId AND taskId = @TaskId", DBTableNames.FavoriteTask);
            }
            else
            {
                sqlStr = string.Format("INSERT INTO {0} (userId, taskId) VALUES (@UserId, @TaskId)", DBTableNames.FavoriteTask);
            }

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@TaskId", taskId)
            };

            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }

        #endregion

        #region SEARCH TASK

        private static List<Tasks> SearchTaskTitle(string projectId, string title)
        {
            string sqlStr = string.Format("SELECT * FROM {0} WHERE projectId = @ProjectId AND title LIKE @TitleSyntax ORDER BY createdAt DESC",
                                DBTableNames.Task);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@TitleSyntax", title + "%")
            };

            return DBGetModel.GetModelList(sqlStr, parameters, new TaskMapper());
        }
        public static List<TaskMeta> SearchTaskMetaTitle(string userId, string projectId, string title)
        {
            List<Tasks> tasks = SearchTaskTitle(projectId, title);
            List<string> favoriteTasks = SelectFavoriteList(userId, projectId);

            List<TaskMeta> taskMetas = new List<TaskMeta>();
            foreach (Tasks task in tasks)
            {
                taskMetas.Add(new TaskMeta(task, favoriteTasks.Contains(task.TaskId)));
            }

            return taskMetas;
        }

        #endregion

        #region CHECK INFORMATION

        public static bool CheckIsFavorite(string userId, string taskId)
        {
            string sqlStr = string.Format("SELECT 1 FROM {0} WHERE userId = @UserId AND taskId = @TaskId", DBTableNames.FavoriteTask);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@TaskId", taskId)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            return dataTable.Rows.Count > 0;
        }

        #endregion

    }
}
