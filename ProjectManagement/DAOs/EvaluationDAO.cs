using ProjectManagement.Database;
using ProjectManagement.Mappers.Implement;
using ProjectManagement.Models;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ProjectManagement.DAOs
{
    internal class EvaluationDAO : DBConnection
    {

        #region SELECT EVALUATION

        // 1.
        public static Evaluation SelectOnly(string taskId, string studentId)
        {
            string sqlStr = string.Format("SELECT * FROM FUNC_GetEvaluation(@TaskId, @StudentId)");
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@TaskId", taskId),
                new SqlParameter("@StudentId", studentId)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                EvaluationMapper evaluationMapper = new EvaluationMapper();
                return evaluationMapper.MapRow(dataTable.Rows[0]);
            }

            return null;
        }

        // 2.
        public static List<Evaluation> SelectListByUser(string studentId)
        {
            string sqlStr = string.Format("SELECT * FROM dbo.FUNC_GetEvaluationByStudentId(@StudentId)");
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@StudentId", studentId)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            List<Evaluation> listEvaluations = new List<Evaluation>(); 

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                EvaluationMapper evaluationMapper = new EvaluationMapper();
                foreach (DataRow row in dataTable.Rows)
                {
                    Evaluation evaluation = evaluationMapper.MapRow(row);
                    listEvaluations.Add(evaluation);
                }
            }

            return listEvaluations;
        }

        // 3.
        public static List<Evaluation> SelectListByUserAndYear(string studentId, int year)
        {
            string sqlStr = string.Format("SELECT * FROM dbo.FUNC_GetEvaluationByStudentIdAndYear(@StudentId, @YearSelected)");
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@StudentId", studentId),
                new SqlParameter("@YearSelected", year)

            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            List<Evaluation> listEvaluations = new List<Evaluation>();

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                EvaluationMapper evaluationMapper = new EvaluationMapper();
                foreach (DataRow row in dataTable.Rows)
                {
                    Evaluation evaluation = evaluationMapper.MapRow(row);
                    listEvaluations.Add(evaluation);
                }
            }

            return listEvaluations;
        }
        #endregion

        #region EVALUATION DAO EXECUTION

        // 4.
        public static void InsertAssignStudent(string instructorId, string taskId, string studentId)
        {
            Evaluation evaluation = new Evaluation(string.Empty, 0.0D, 0.0D, false, DateTime.Now,
                instructorId, studentId, taskId);
            string sqlStr = "EXEC dbo.PROC_AddEvaluation " +
                            "@EvaluationId, @Content, @CompletionRate, " +
                            "@Score, @Evaluated, @CreatedAt, @CreatedBy, @StudentId, @TaskId";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@EvaluationId", evaluation.EvaluationId),
                new SqlParameter("@Content", evaluation.Content),
                new SqlParameter("@CompletionRate", evaluation.CompletionRate),
                new SqlParameter("@Score", evaluation.Score),
                new SqlParameter("@Evaluated", evaluation.Evaluated),
                new SqlParameter("@CreatedAt", evaluation.CreatedAt),
                new SqlParameter("@CreatedBy", evaluation.CreatedBy),
                new SqlParameter("@StudentId", evaluation.StudentId),
                new SqlParameter("@TaskId", evaluation.TaskId)
            };
            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }

        // 5.
        public static void Update(Evaluation evaluation)
        {
            string sqlStr = "EXEC dbo.PROC_UpdateEvaluation " +
                            "@EvaluationId, @Content, @CompletionRate, " +
                            "@Score, @Evaluated, @CreatedAt, @CreatedBy, @StudentId, @TaskId";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@EvaluationId", evaluation.EvaluationId),
                new SqlParameter("@Content", evaluation.Content),
                new SqlParameter("@CompletionRate", evaluation.CompletionRate),
                new SqlParameter("@Score", evaluation.Score),
                new SqlParameter("@Evaluated", evaluation.Evaluated),
                new SqlParameter("@CreatedAt", evaluation.CreatedAt),
                new SqlParameter("@CreatedBy", evaluation.CreatedBy),
                new SqlParameter("@StudentId", evaluation.StudentId),
                new SqlParameter("@TaskId", evaluation.TaskId)
            };

            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }

        // 6.
        public static void DeleteByTaskId(string taskId)
        {
            string sqlStr = "EXEC dbo.PROC_DeleteEvaluationByTaskId @TaskId";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@TaskId", taskId)
            };
            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }
        #endregion

        #region STATISTICAL

        public static DataTable CreateEvaluationTable(List<Evaluation> listEvaluations)
        {
            DataTable evaluationTable = new DataTable();

            evaluationTable.Columns.Add("evaluationId", typeof(string));
            evaluationTable.Columns.Add("content", typeof(string));
            evaluationTable.Columns.Add("completionRate", typeof(float));
            evaluationTable.Columns.Add("score", typeof(float));
            evaluationTable.Columns.Add("evaluated", typeof(bool));
            evaluationTable.Columns.Add("createdAt", typeof(DateTime));
            evaluationTable.Columns.Add("createdBy", typeof(string));
            evaluationTable.Columns.Add("studentId", typeof(string));
            evaluationTable.Columns.Add("taskId", typeof(string));

            foreach (var evaluation in listEvaluations)
            {
                evaluationTable.Rows.Add(
                    evaluation.EvaluationId,
                    evaluation.Content,
                    evaluation.CompletionRate,
                    evaluation.Score,
                    evaluation.Evaluated,
                    evaluation.CreatedAt,
                    evaluation.CreatedBy,
                    evaluation.StudentId,
                    evaluation.TaskId
                );
            }

            return evaluationTable;
        }


        public static Dictionary<string, int> GroupByMonth(List<Evaluation> listEvaluations)
        {
            DataTable evaluationTable = CreateEvaluationTable(listEvaluations);

            string sqlStr = "SELECT MonthName, EvaluationCount FROM FUNC_GetEvaluationsGroupedByMonth(@EvaluationList) " +
                            "ORDER BY MonthNumber;";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter
                {
                    ParameterName = "@EvaluationList",
                    SqlDbType = SqlDbType.Structured,
                    TypeName = "EvaluationTableType",
                    Value = evaluationTable
                }
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);
            Dictionary<string, int> result = new Dictionary<string, int>();

            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    string monthName = row["MonthName"].ToString();
                    int evaluationCount = Convert.ToInt32(row["EvaluationCount"]);

                    result.Add(monthName, evaluationCount);
                }
            }

            return result;
        }


        #endregion

    }
}
