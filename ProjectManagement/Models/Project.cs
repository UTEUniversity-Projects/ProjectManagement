using Guna.UI2.WinForms.Suite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Process;
using ProjectManagement.Enums;
using ProjectManagement.Utils;

namespace ProjectManagement.Models
{
    public class Project
    {

        #region PROJECT ATTRIBUTES

        private string projectId;
        private string instructorId;
        private string topic;
        private string description;
        private string feature;
        private string requirement;
        private int maxMember;
        private EProjectStatus status;
        private DateTime createdAt;
        private string createdBy;
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
            status = default;
            createdAt = DateTime.MinValue;
            createdBy = string.Empty;
            fieldId = string.Empty;
        }

        public Project(string projectId, string instructorId, string topic, string description, string feature, string requirement, 
            int maxMember, EProjectStatus status, DateTime createdAt, string createdBy, string fieldId)
        {
            this.projectId = projectId;
            this.instructorId = instructorId;
            this.topic = topic;
            this.description = description;
            this.feature = feature;
            this.requirement = requirement;
            this.maxMember = maxMember;
            this.status = status;
            this.createdAt = createdAt;
            this.createdBy = createdBy;
            this.fieldId = fieldId;
        }

        public Project(string instructorId, string topic, string description, string feature, string requirement, 
            int maxMember, EProjectStatus status, DateTime createdAt, string createdBy, string fieldId)
        {
            this.projectId = ModelUtil.GenerateModelId(EModelClassification.PROJECT);
            this.instructorId = instructorId;
            this.topic = topic;
            this.description = description;
            this.feature = feature;
            this.requirement = requirement;
            this.maxMember = maxMember;
            this.status = status;
            this.createdAt = createdAt;
            this.createdBy = createdBy;
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
        public EProjectStatus Status
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
        public string FieldId
        {
            get { return fieldId; }
            set { fieldId = value; }
        }

        #endregion

        #region CHECK INFORMATIONS

        public bool CheckTopic()
        {
            return this.topic != string.Empty;
        }
        public bool CheckDescription()
        {
            return this.description != string.Empty;
        }
        public bool CheckFeature()
        {
            return this.feature != string.Empty;
        }
        public bool CheckRequirement()
        {
            return this.requirement != string.Empty;
        }
        public bool CheckInstructorId()
        {
            return this.instructorId != string.Empty;
        }

        #endregion

        #region FUNCTIONS

        public Color GetStatusColor()
        {
            switch (this.status)
            {
                case EProjectStatus.REGISTERED:
                    return Color.FromArgb(255, 87, 87);
                case EProjectStatus.PROCESSING:
                    return Color.FromArgb(94, 148, 255);
                case EProjectStatus.COMPLETED:
                    return Color.FromArgb(45, 237, 55);
                case EProjectStatus.GAVEUP:
                    return Color.FromArgb(252, 182, 3);
                case EProjectStatus.WAITING:
                    return Color.FromArgb(237, 62, 247);
                default:
                    return Color.Gray;
            }
        }
        public int GetPriority()
        {
            switch (this.status)
            {
                case EProjectStatus.REGISTERED:
                    return 0;
                case EProjectStatus.PROCESSING:
                    return 1;
                case EProjectStatus.PUBLISHED:
                    return 2;
                case EProjectStatus.COMPLETED:
                    return 3;
                default:
                    return int.MaxValue;
            }
        }

        #endregion

    }
}
