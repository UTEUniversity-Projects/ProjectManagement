using ProjectManagement.Process;
using ProjectManagement.Enums;
using ProjectManagement.Utils;
using ProjectManagement.DAOs;

namespace ProjectManagement.Models
{
    public class Evaluation
    {

        #region EVALUATION ATTRIBUTES

        private string evaluationId;
        private string content;
        private double completionRate;
        private double score;
        private bool evaluated;
        private DateTime createdAt;
        private string createdBy;
        private string studentId;
        private string taskId;

        #endregion

        #region EVALUATION CONTRUCTORS

        public Evaluation()
        {
            evaluationId = string.Empty;
            content = string.Empty;
            completionRate = 0;
            score = 0;
            evaluated = false;
            createdAt = DateTime.MinValue;
            createdBy = string.Empty;
            studentId = string.Empty;
            taskId = string.Empty;
        }
        public Evaluation(string evaluationId, string content, double completionRate, double score, bool evaluated, DateTime createdAt, string createdBy, string studentId, string taskId)
        {
            this.evaluationId = evaluationId;
            this.content = content;
            this.completionRate = completionRate;
            this.score = score;
            this.evaluated = evaluated;
            this.createdAt = createdAt;
            this.createdBy = createdBy;
            this.studentId = studentId;
            this.taskId = taskId;
        }
        public Evaluation(string content, double completionRate, double score, bool evaluated, DateTime createdAt, string createdBy, string studentId, string taskId)
        {
            this.evaluationId = ModelUtil.GenerateModelId(EModelClassification.EVALUATION);
            this.content = content;
            this.completionRate = completionRate;
            this.score = score;
            this.evaluated = evaluated;
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
        public bool Evaluated
        {
            get { return evaluated; }
            set { evaluated = value; }
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

        #region CHECK INFORMATIONS

        public bool CheckContent()
        {
            return DAOUtils.CheckIsNotEmpty(this.content, "content");
        }
        public bool CheckCompletionRate()
        {
            return DAOUtils.CheckIsValidInRange(this.completionRate, 0.0D, 100.0D, "completion rate");
        }
        public bool CheckScore()
        {
            return DAOUtils.CheckIsValidInRange(this.score, 0.0D, 10.0D, "score");
        }

        #endregion

        #region FUNCTIONS

        public Color GetStatusColor()
        {
            if (this.Evaluated) return Color.FromArgb(45, 237, 55);
            else return Color.Gray;
        }

        #endregion

    }
}
