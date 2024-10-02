using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementModel.Models
{
    internal class Meeting
    {
        #region MEETING ATTRIBUTES

        private string meetingId;
        private string title;
        private string description;
        private DateTime startAt;
        private string location;
        private string link;
        private DateTime createdAt;
        private string createBy;
        private string projectId;

        #endregion

        #region MEETING CONTRUCTOR

        public Meeting()
        {

        }
        public Meeting(string meetingId, string title, string description, DateTime startAt, string location, string link, DateTime createdAt, string createBy, string projectId)
        {
            this.meetingId = meetingId;
            this.title = title;
            this.description = description;
            this.startAt = startAt;
            this.location = location;
            this.link = link;
            this.createdAt = createdAt;
            this.createBy = createBy;
            this.projectId = projectId;
        }

        #endregion

        #region MEETING PROPERTIES

        public string MeetingId { 
            get {  return meetingId; }
            set { meetingId = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public DateTime StartAt
        {
            get { return startAt; }
            set { startAt = value; }
        }
        public string Location
        {
            get { return location; }
            set { location = value; }
        }
        public string Link
        {
            get { return link; }
            set { link = value; }
        }
        public DateTime CreatedAt
        {
            get { return createdAt; }
            set { createdAt = value; }
        }
        public string CreateBy
        {
            get { return createBy; }
            set { createBy = value; }
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
        public bool CheckStartAt()
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
