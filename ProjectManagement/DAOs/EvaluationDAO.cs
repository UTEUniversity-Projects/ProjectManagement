using ProjectManagement.Database;
using ProjectManagement.Mappers.Implement;
using ProjectManagement.Models;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ProjectManagement.DAOs
{
    internal class EvaluationDAO : DBConnection
    {

        #region SELECT EVALUATION

        public static Evaluation SelectOnly(string taskId, string studentId)
        {
            string sqlStr = string.Format("SELECT * FROM {0} WHERE taskId = @TaskId AND studentId = @StudentId",
                DBTableNames.Evaluation);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@TaskId", taskId),
                new SqlParameter("@StudentId", studentId)
            };

            return DBGetModel.GetModel(sqlStr, parameters, new EvaluationMapper());
        }

        public static List<Evaluation> SelectListByTask(string taskId)
        {
            return DBGetModel.GetModelList(DBTableNames.Evaluation, "taskId", taskId, new EvaluationMapper());
        }

        public static List<Evaluation> SelectListByUser(string studentId)
        {
            return DBGetModel.GetModelList(DBTableNames.Evaluation, "studentId", studentId, new EvaluationMapper());
        }

        #endregion

        #region EVALUATION DAO EXECUTION

        public static void InsertFollowTeam(string instructorId, string taskId, string teamId)
        {
            string sqlStr = string.Format("SELECT studentId FROM {0} WHERE teamId = @TeamId", DBTableNames.JoinTeam);

            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@TeamId", teamId) };

            DataTable dataTable = DBExecution.ExecuteQuery(sqlStr, parameters);

            foreach (DataRow row in dataTable.Rows)
            {
                Evaluation evaluation = new Evaluation(string.Empty, 0.0D, 0.0D, false, DateTime.Now, 
                    instructorId, row["studentId"].ToString(), taskId);
                DBExecution.Insert(evaluation, DBTableNames.Evaluation);
            }
        }

        public static void Update(Evaluation evaluation)
        {
            DBExecution.Update(evaluation, DBTableNames.Evaluation, "evaluationId", evaluation.EvaluationId);
        }

        public static void DeleteFollowTask(Tasks task)
        {
            string sqlStr = string.Format("DELETE FROM {0} WHERE taskId = @TaskId", DBTableNames.Evaluation);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@TaskId", task.TaskId)
            };

            DBExecution.ExecuteNonQuery(sqlStr, parameters);
        }

        #endregion

    }
}
