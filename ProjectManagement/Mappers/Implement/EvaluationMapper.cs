using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Mappers.Implement
{
    internal class EvaluationMapper : IRowMapper<Evaluation>
    {
        public Evaluation MapRow(DataRow row)
        {
            string evaluationId = row["evaluationId"].ToString();
            string content = row["content"].ToString();
            double completionRate = double.Parse(row["completionRate"].ToString());
            double score = double.Parse(row["score"].ToString());
            bool evaluated = row["evaluated"].ToString() == "True" ? true : false;
            DateTime createdAt = DateTime.Parse(row["createdAt"].ToString());
            string createdBy = row["createdBy"].ToString();
            string studentId = row["studentId"].ToString();
            string taskId = row["taskId"].ToString();

            Evaluation evaluation = new Evaluation(evaluationId, content, completionRate, score,
                evaluated, createdAt, createdBy, studentId, taskId);

            return evaluation;
        }
    }
}
