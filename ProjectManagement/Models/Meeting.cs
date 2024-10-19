using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Process;
using ProjectManagement.Enums;
using ProjectManagement.Utils;

namespace ProjectManagement.Models
{
    public class Meeting
    {

        #region MEETING ATTRIBUTES

        private string meetingId;
        private string title;
        private string description;
        private DateTime startAt;
        private string location;
        private string link;
        private DateTime createdAt;
        private string createdBy;
        private string projectId;

        #endregion

        #region MEETING CONTRUCTOR

        public Meeting()
        {
            this.meetingId = string.Empty;
            this.title = string.Empty;
            this.description = string.Empty;
            this.startAt = DateTime.MinValue;
            this.location = string.Empty;
            this.link = string.Empty;
            this.createdAt = DateTime.MinValue;
            this.createdBy = string.Empty;
            this.projectId = string.Empty;
        }

        public Meeting(string meetingId, string title, string description, DateTime startAt, string location, string link, DateTime createdAt, string createdBy, string projectId)
        {
            this.meetingId = meetingId;
            this.title = title;
            this.description = description;
            this.startAt = startAt;
            this.location = location;
            this.link = link;
            this.createdAt = createdAt;
            this.createdBy = createdBy;
            this.projectId = projectId;
        }

        public Meeting(string title, string description, DateTime startAt, string location, string link, DateTime createdAt, string createdBy, string projectId)
        {
            this.meetingId = ModelUtil.GenerateModelId(EModelClassification.MEETING);
            this.title = title;
            this.description = description;
            this.startAt = startAt;
            this.location = location;
            this.link = link;
            this.createdAt = createdAt;
            this.createdBy = createdBy;
            this.projectId = projectId;
        }

        #endregion

        #region MEETING PROPERTIES

        public string MeetingId
        {
            get { return meetingId; }
            set { meetingId = value; }
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
        public DateTime StartAt
        {
            get { return startAt; }
            set { startAt = value; }
        }
        public string Location
        { 
            get { return this.location; }
            set { this.location = value; }
        }
        public string Link
        { 
            get { return this.link; } 
            set { this.link = value; }
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
        public string ProjectId
        {
            get { return projectId; }
            set { projectId = value; }
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
        public bool CheckStart()
        {
            return this.startAt >= DateTime.Now;
        }
        public bool CheckLocation()
        {
            return this.location != string.Empty;
        }

        #endregion

    }
}
