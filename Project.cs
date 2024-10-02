using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementModel.Models
{
    internal class Project
    {
        #region PROJECT ATTRIBUTES

        private string projectId;
        private string instructorId;
        private string topic;
        private string description;
        private string feature;
        private string requirement;
        private int maxMember;
        private DateTime publicDate;
        private string status;
        private DateTime createdAt;
        private string fieldId;

        #endregion

        #region PROJECT CONTRUCTOR

        public Project() 
        {
            projectId = string.Empty;
            topic = string.Empty;
            description = string.Empty;
            feature = string.Empty;
            requirement = string.Empty;
            maxMember = 0;
            publicDate = DateTime.Now;
            status = string.Empty;
            createdAt = DateTime.Now;
            fieldId = string.Empty;
        }
        public Project(string projectId, string instructorId, string topic, string description, string feature, string requirement, int maxMember, DateTime publicDate, string status, DateTime createdAt, string fieldId)
        {
            this.projectId = projectId;
            this.instructorId = instructorId;
            this.topic = topic;
            this.description = description;
            this.feature = feature;
            this.requirement = requirement;
            this.maxMember = maxMember;
            this.publicDate = publicDate;
            this.status = status;
            this.createdAt = createdAt;
            this.fieldId = fieldId;
        }

        #endregion

        #region PROJECT PROPERTIES

        public string ProjectId
        {
            get { return projectId; }
            set { projectId = value; }
        }
        public string InstructorId
        {
            get { return instructorId; }
            set { instructorId = value; }
        }
        public string Topic
        {
            get { return topic; }
            set { topic = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string Feature
        {
            get { return feature; }
            set { feature = value; }
        }
        public string Requirement
        {
            get { return requirement; }
            set { requirement = value; }
        }
        public int MaxMember
        {
            get { return maxMember; }
            set { maxMember = value; }
        }
        public DateTime PublicDate
        {
            get { return publicDate; }
            set { publicDate = value; }
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
        public string FieldId
        {
            get { return fieldId; }
            set { fieldId = value; }
        }

        #endregion
    }
}
