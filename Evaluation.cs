using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementModel.Models
{
    internal class Evaluation
    {
        #region EVALUATION ATTRIBUTES

        private string evaluationId;
        private string content;
        private double completionRate;
        private double score;
        private string status;
        private DateTime createdAt;
        private string createdBy;
        private string studentId;
        private string taskId;

        #endregion

        #region EVALUATION CONTRUCTOR

        public Evaluation()
        {

        }
        public Evaluation(string evaluationId, string content, double completionRate, double score, string status, DateTime createdAt, string createdBy, string studentId, string taskId)
        {
            this.evaluationId = evaluationId;
            this.content = content;
            this.completionRate = completionRate;
            this.score = score;
            this.status = status;
            this.createdAt = createdAt;
            this.createdBy = createdBy;
            this.studentId = studentId;
            this.taskId = taskId;
        }


        #endregion

        #region EVALUATION PROPERTIES

        public string EvaluationId
        {
            get { return evaluationId; }
            set { evaluationId = value; }
        }
        public string Content
        {
            get { return content; }
            set { content = value; }
        }
        public double CompletionRate
        {
            get { return completionRate; }
            set { completionRate = value; }
        }
        public double Score
        {
            get { return score; }
            set { score = value; }
        }
        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        public DateTime CreatedAt
        {
            get { return createdAt; }
            set { createdAt = value; }
        }
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public string StudentId
        {
            get { return studentId; }
            set { studentId = value; }
        }
        public string TaskId
        {
            get { return taskId; }
            set { taskId = value; }
        }

        #endregion
    }
}
