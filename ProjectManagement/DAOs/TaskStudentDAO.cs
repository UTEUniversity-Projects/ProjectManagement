using Microsoft.VisualBasic.ApplicationServices;
using ProjectManagement.Database;
using ProjectManagement.Enums;
using ProjectManagement.MetaData;
using ProjectManagement.Models;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Mappers.Implement;
using System.Data.SqlClient;
using System.Threading.Tasks;
namespace ProjectManagement.DAOs
{
    public class TaskStudent
    {
        protected string idTask;
        protected string idStudent;
        public TaskStudent(string idTask, string idStudent)
        {
            this.idTask = idTask;
            this.idStudent = idStudent;
        }
        public string TaskId
        {
            get { return idTask; }
            set { idTask = value; }
        }
        public string StudentId
        {
            get { return idStudent; }
            set { idStudent = value; }
        }
    }

    internal class TaskStudentDAO : DBConnection
    {
        public static void Insert(TaskStudent taskStudent)
        {
            DBExecution.Insert(taskStudent, DBTableNames.TaskStudent);
        }
        public static List<Member> GetMembersByTaskId(string taskId)
        {
            string sqlStr = $"SELECT studentId FROM {DBTableNames.TaskStudent} WHERE taskId = @TaskId";
            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@TaskId", taskId) };

            DataTable dataTable = DBExecution.ExecuteQuery(sqlStr, parameters);

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
        public static List<Tasks> SelectListTaskByStudentId(string studentId)
        {
            string sqlStr = string.Format("SELECT {0}.* FROM {0} INNER JOIN {1} ON {0}.taskId = {1}.taskId " +
                                            "WHERE studentId = @studentId ORDER BY {0}.createdAt DESC",
                                            DBTableNames.Task, DBTableNames.TaskStudent);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@studentId", studentId)
            };

            return DBGetModel.GetModelList(sqlStr, parameters, new TaskMapper());
        }
    }
    
}
