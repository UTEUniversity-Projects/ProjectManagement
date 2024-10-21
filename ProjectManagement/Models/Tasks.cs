using ProjectManagement.Enums;
using ProjectManagement.Utils;

namespace ProjectManagement.Models
{
    public class Tasks
    {

        #region TASK ATTRIBUTES

        private string taskId;
        private DateTime startAt;
        private DateTime endAt;
        private string title;
        private string description;
        private double progress;
        private ETaskPriority priority; 
        private DateTime createdAt;
        private string createdBy;
        private string projectId;

        #endregion

        #region TASK CONTRUCTOR

        public Tasks()
        {
            taskId = string.Empty;
            startAt = DateTime.MinValue;
            endAt = DateTime.MinValue;
            title = string.Empty;
            description = string.Empty;
            progress = 0;
            priority = default;
            createdAt = DateTime.MinValue;
            createdBy = string.Empty;
            projectId = string.Empty;
        }

        public Tasks(string taskId, DateTime startAt, DateTime endAt, string title, string description, double progress, ETaskPriority priority, DateTime createdAt, string createdBy, string projectId) 
        {
            this.taskId = taskId;
            this.startAt = startAt;
            this.endAt = endAt;
            this.title = title;
            this.description = description;
            this.progress = progress;
            this.priority = priority;
            this.createdAt = createdAt;
            this.createdBy = createdBy;
            this.projectId = projectId;
        }

        public Tasks(DateTime startAt, DateTime endAt, string title, string description, double progress, ETaskPriority priority, DateTime createdAt, string createdBy, string projectId)
        {
            this.taskId = ModelUtil.GenerateModelId(EModelClassification.TASK);
            this.startAt = startAt;
            this.endAt = endAt;
            this.title = title;
            this.description = description;
            this.progress = progress;
            this.priority = priority;
            this.createdAt = createdAt;
            this.createdBy = createdBy;
            this.projectId = projectId;
        }

        #endregion

        #region TASK PROPERTIES

        public string TaskId
        {
            get { return this.taskId; }
            set { this.taskId = value; }
        }
        public DateTime StartAt
        {
            get { return this.startAt; }
            set { this.startAt = value; }
        }
        public DateTime EndAt
        {
            get { return this.endAt; }
            set { this.endAt = value; }
        }
        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }
        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }
        public double Progress
        {
            get { return this.progress; }
            set { this.progress = value; }
        }
        public ETaskPriority Priority
        {
            get { return this.priority; }
            set { this.priority = value; }
        }
        public DateTime CreatedAt
        {
            get { return this.createdAt; }
            set { this.createdAt = value; }
        }
        public string CreatedBy
        {
            get { return this.createdBy; }
        }
        public string ProjectId
        {
            get { return projectId; }
            set { this.projectId = value; }
        }

        #endregion

        #region CHECK INFORMATIONS

        public bool CheckTitle()
        {
            return this.title != string.Empty;
        }
        public bool CheckDescription()
        {
            return this.description != string.Empty;
        }
        public bool CheckProgress()
        {
            return this.progress >= 0 && this.progress <= 100;
        }
        public bool CheckStart()
        {
            return this.startAt >= DateTime.Now;
        }
        public bool CheckEnd()
        {
            return this.startAt < this.endAt;
        }

        #endregion

    }
}
