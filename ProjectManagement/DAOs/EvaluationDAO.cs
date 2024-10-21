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

            Evaluation evaluation = DBGetModel.GetModel(sqlStr, parameters, new EvaluationMapper());

            if (evaluation != null) return evaluation;
            return new Evaluation();
        }

        public static List<Evaluation> SelectListByUser(string studentId)
        {
            return DBGetModel.GetModelList(DBTableNames.Evaluation, "studentId", studentId, new EvaluationMapper());
        }

        #endregion

        #region EVALUATION DAO EXECUTION

        public static void InsertAssignStudent(string instructorId, string taskId, string studentId)
        {
            Evaluation evaluation = new Evaluation(string.Empty, 0.0D, 0.0D, false, DateTime.Now,
                instructorId, studentId, taskId);
            DBExecution.Insert(evaluation, DBTableNames.Evaluation);
        }
        public static void Update(Evaluation evaluation)
        {
            DBExecution.Update(evaluation, DBTableNames.Evaluation, "evaluationId", evaluation.EvaluationId);
        }

        #endregion

    }
}
