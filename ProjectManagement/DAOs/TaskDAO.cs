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
using static TheArtOfDevHtmlRenderer.Adapters.RGraphicsPath;
using System.Windows.Forms;

namespace ProjectManagement.DAOs
{
    internal class TaskDAO : DBConnection
    {
        #region CHECK INFORMATIONS
        public static bool CheckIsNotEmpty(string input, string fieldName)
        {
            string sqlStr = "SELECT * FROM dbo.FUNC_IsNotEmpty(@Input, @FieldName)";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
            new SqlParameter("@Input", input),
            new SqlParameter("@FieldName", fieldName)
            };
            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);
            return dataTable.Rows.Count > 0 && Convert.ToBoolean(dataTable.Rows[0]["IsValid"]);
        }
        public static bool CheckIsValidInRange(double value, double minValue, double maxValue, string fieldName)
        {
            string sqlStr = "SELECT IsValid, Message FROM FUNC_IsValidInRange(@Value, @MinValue, @MaxValue, @FieldName)";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Value", value),
                new SqlParameter("@MinValue", minValue),
                new SqlParameter("@MaxValue", maxValue),
                new SqlParameter("@FieldName", fieldName)
            };
            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);
            return dataTable.Rows.Count > 0 && Convert.ToBoolean(dataTable.Rows[0]["IsValid"]);
        }
        public static bool CheckStartDate(DateTime startDate, string fieldName)
        {
            string sqlStr = "SELECT IsValid, Message FROM FUNC_CheckStartDate(@StartAt, @FieldName)";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@StartAt", startDate),
                new SqlParameter("@FieldName", fieldName)
            };
            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);
            return dataTable.Rows.Count > 0 && Convert.ToBoolean(dataTable.Rows[0]["IsValid"]);
        }
        public static bool CheckEndDate(DateTime startDate, DateTime endDate, string fieldName)
        {
            string sqlStr = "SELECT IsValid, Message FROM FUNC_CheckEndDate(@StartAt, @EndAt, @FieldName)";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@StartAt", startDate),
                new SqlParameter("@EndAt", endDate),
                new SqlParameter("@FieldName", fieldName)
            };
            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);
            return dataTable.Rows.Count > 0 && Convert.ToBoolean(dataTable.Rows[0]["IsValid"]);
        }
        #endregion

        #region SELECT TASKS
        public static Tasks SelectOnly(string taskId)
        {
            string sqlStr = string.Format("Select * From FUNC_GetTaskById(@taskId)");


            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@TaskId", taskId)
            };
            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                TaskMapper taskMapper = new TaskMapper();
                return taskMapper.MapRow(dataTable.Rows[0]);
            }

            return null;
        }
        public static List<Tasks> SelectListByTeam(string teamId)
        {
            string sqlStr = string.Format("SELECT * FROM FUNC_GetTasksByTeamId(@TeamId, @Accepted) ORDER BY createdAt DESC");

            List <SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@TeamId", teamId),
                new SqlParameter("@Accepted", EnumUtil.GetDisplayName(ETeamStatus.ACCEPTED))
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);
            if (dataTable != null && dataTable.Rows.Count > 0) {
                List<Tasks> tasksList = new List<Tasks>();
                TaskMapper taskMapper = new TaskMapper();

                foreach (DataRow row in dataTable.Rows)
                {
                    Tasks task = taskMapper.MapRow(row);

                    tasksList.Add(task);
                }
                return tasksList;
            }
            return new List<Tasks>();
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
        public static List<Tasks> SelectListTaskByStudent(string projectId, string studentId)
        {
            string sqlStr = string.Format("SELECT * FROM FUNC_GetTasksByProjectAndStudent(@ProjectId, @StudentId)");

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@StudentId", studentId)
            };
            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            List<Tasks> tasksList = new List<Tasks>();
            TaskMapper taskMapper = new TaskMapper();

            foreach (DataRow row in dataTable.Rows)
            {
                Tasks task = taskMapper.MapRow(row);
                tasksList.Add(task);
            }

            return tasksList;
        }
        public static List<Tasks> SelectListTaskByProject(string projectId)
        {
            string sqlStr = string.Format("SELECT * FROM FUNC_GetTaskIdsByProjectId(@projectId) ORDER BY createdAt DESC");

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@projectId", projectId)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                List<Tasks> tasksList = new List<Tasks>();
                TaskMapper taskMapper = new TaskMapper();

                foreach (DataRow row in dataTable.Rows)
                {
                    Tasks task = taskMapper.MapRow(row);
                    tasksList.Add(task);
                }
                return tasksList;
            }
            return new List<Tasks>();
        }
        private static List<string> SelectFavoriteList(string userId, string projectId)
        {
            string sqlStr = "SELECT * FROM FUNC_GetFavoriteTasksByProjectIdAndUserId(@ProjectId, @UserId)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@UserId", userId)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                List<string> list = new List<string>();

                foreach (DataRow row in dataTable.Rows)
                {
                    string taskId = row["taskId"].ToString();
                    list.Add(taskId);
                }
                return list;
            }

            return new List<string>();
        }


        #endregion

        #region TASK DAO EXECUTION

        public static void Insert(Tasks task)
        {
            string sqlStr = "EXEC PROC_AddTask @taskId, @startAt, @endAt, @title, @description, @progress, @priority, @createdBy, @projectId";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@taskId", task.TaskId),
                new SqlParameter("@startAt", task.StartAt),
                new SqlParameter("@endAt", task.EndAt),
                new SqlParameter("@title", task.Title),
                new SqlParameter("@description", task.Description),
                new SqlParameter("@progress", task.Progress),
                new SqlParameter("@priority", EnumUtil.GetDisplayName(task.Priority)),
                new SqlParameter("@createdBy", task.CreatedBy),
                new SqlParameter("@projectId", task.ProjectId)
            };

            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }
        public static void InsertAssignStudent(string taskId, string studentId)
        {
            string sqlStr = "EXEC PROC_InsertAssignStudent @taskId, @studentId";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@taskId", taskId),
                new SqlParameter("@studentId", studentId)
            };

            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }

        public static void DeleteTaskStudentByTaskId(string taskId)
        {
            string sqlStr = "EXEC PROC_DeleteTaskStudentByTaskId @taskId";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@taskId", taskId)
            };

            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }
        public static void DeleteFavoriteTaskByTaskId(string taskId)
        {
            string sqlStr = "EXEC PROC_DeleteFavoriteTaskByTaskId @taskId";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@taskId", taskId)
            };

            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }
        public static void Delete(string taskId)
        {
            DBExecution.Delete(DBTableNames.Evaluation, "taskId", taskId);
            DBExecution.Delete(DBTableNames.Comment, "taskId", taskId);

            DeleteTaskStudentByTaskId(taskId);
            DeleteFavoriteTaskByTaskId(taskId);

            string sqlStr = "EXEC PROC_DeleteTaskByTaskId @taskId";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@taskId", taskId)
            };

            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }
        
        public static void DeleteFollowProject(string projectId)
        {
            List<Tasks> listTasks = SelectListTaskByProject(projectId);

            foreach (Tasks task in listTasks)
            {
                Delete(task.TaskId);
            }
        }

        public static void Update(Tasks task)
        {
            string sqlStr = "EXEC PROC_UpdateTask @TaskId, @Title, @Description, @StartAt, @EndAt, @Progress, @Priority";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@TaskId", task.TaskId),
                new SqlParameter("@Title", task.Title),
                new SqlParameter("@Description", task.Description),
                new SqlParameter("@StartAt", task.StartAt),
                new SqlParameter("@EndAt", task.EndAt),
                new SqlParameter("@Progress", task.Progress),
                new SqlParameter("@priority", EnumUtil.GetDisplayName(task.Priority))
            };

            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, "Update Task");
        }
       
        public static void DeleteFavoritesTask(string userId, string taskId)
        {
            string sqlStr = "EXEC PROC_DeleteFavoriteTask @userId, @taskId";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@userId", userId),
                new SqlParameter("@taskId", taskId),
            };

            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, "Update Task");
        }
        public static void InsertFavoritesTask(string userId, string taskId)
        {
            string sqlStr = "EXEC PROC_InsertFavoriteTask @userId, @taskId";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@userId", userId),
                new SqlParameter("@taskId", taskId),
            };

            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, "Update Task");
        }
        public static void UpdateFavorite(string userId, string taskId, bool isFavorite)
        {
            string sqlStr = string.Empty;

            if (isFavorite == false)
            {
                DeleteFavoritesTask(userId, taskId);
            }
            else
            {
                InsertFavoritesTask(userId, taskId);
            }
        }

        #endregion

        #region SEARCH TASK

        private static List<Tasks> SearchTaskTitle(string projectId, string title)
        {
            string sqlStr = "EXEC PROC_SearchTaskByTitle @ProjectId, @TitleSyntax";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@TitleSyntax", title + "%") // Tìm các task bắt đầu với 'title'
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                List<Tasks> tasksList = new List<Tasks>();
                TaskMapper taskMapper = new TaskMapper();

                foreach (DataRow row in dataTable.Rows)
                {
                    Tasks task = taskMapper.MapRow(row);

                    tasksList.Add(task);
                }
                return tasksList;
            }
            return new List<Tasks>();
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
            string sqlStr = "SELECT IsFavorite FROM FUNC_CheckIsFavorite(@UserId, @TaskId)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@TaskId", taskId)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);
            return dataTable != null && dataTable.Rows.Count > 0;
        }

        #endregion

        public static List<Member> GetMembersByTaskId(string taskId)
        {
            string sqlStr = $"SELECT studentId FROM {DBTableNames.TaskStudent} WHERE taskId = @TaskId";
            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@TaskId", taskId) };
            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            List<Member> list = new List<Member>();

            foreach (DataRow row in dataTable.Rows)
            {
                Users student = UserDAO.SelectOnlyByID(row["studentId"].ToString());
                ETeamRole role = default;
                DateTime joinAt = DateTime.Now;

                list.Add(new Member(student, role, joinAt));
            }

            return list.OrderBy(m => m.Role).ToList();
        }

    }
}
