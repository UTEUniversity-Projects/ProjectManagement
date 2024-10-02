using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementModel.Models
{
    internal class Task
    {
        #region TASK ATTRIBUTES

        private string taskId;
        private DateTime startAt;
        private DateTime endAt;
        private string title;
        private string description;
        private double progress;
        private string priority;
        private DateTime createdAt;
        private string createBy;
        private string projectId;

        #endregion

        #region TASK CONTRUCTOR
        public Task()
        {
            this.taskId = string.Empty;
            this.startAt = DateTime.Now;
            this.endAt = DateTime.Now;
            this.title = string.Empty;
            this.description = string.Empty;
            this.progress = 0;
            this.priority = string.Empty;
            this.createdAt = DateTime.Now;
            this.createBy = string.Empty;
            this.projectId = string.Empty;
        }
        public Task(string taskId, DateTime startAt, DateTime endAt, string title, string description, double progress, string priority, DateTime createdAt, string createBy, string projectId)
        {
            this.taskId = taskId;
            this.startAt = startAt;
            this.endAt = endAt;
            this.title = title;
            this.description = description;
            this.progress = progress;
            this.priority = priority;
            this.createdAt = createdAt;
            this.createBy = createBy;
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
        public string Priority
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
            get { return createBy; }
            set { this.createBy = value; }
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

        #endregion
    }
}
