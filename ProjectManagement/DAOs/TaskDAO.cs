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

        #endregion

        #region TASK DAO EXECUTION

        public static void Insert(Tasks task)
        {
            DBExecution.Insert(task, DBTableNames.Task);
        }

        public static void Delete(string taskId)
        {
            DBExecution.Delete(DBTableNames.Evaluation, "taskId", taskId);
            DBExecution.Delete(DBTableNames.TaskStudent, "taskId", taskId);
            DBExecution.Delete(DBTableNames.Task, "taskId", taskId);
        }
        public static void DeleteFollowProject(string projectId)
        {
            string sqlStr = string.Format("SELECT taskId FROM {0} WHERE projectId = @ProjectId", DBTableNames.Task);

            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@ProjectId", projectId) };

            DataTable dataTable = DBExecution.ExecuteQuery(sqlStr, parameters);

            foreach (DataRow row in dataTable.Rows)
            {
                Delete(row["taskId"].ToString());
            }
        }

        public static void Update(Tasks task)
        {
            DBExecution.Update(task, DBTableNames.Task, "taskId", task.TaskId);
        }
        public static void UpdateIsFavorite(Tasks task)
        {
            // DBUtil.SQLExecuteByCommand(string.Format("UPDATE " + DBTableNames.Task + " SET isfavorite = {0} WHERE taskId = '{1}'", task.IsFavorite ? 1 : 0, task.TaskId));
        }

        #endregion

        #region SEARCH TASK

        public static List<Tasks> SearchTaskTitle(string projectId, string title)
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

        #endregion

    }
}
