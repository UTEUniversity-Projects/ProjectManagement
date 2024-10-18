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
    public class Team
    {

        #region TEAM ATTRIBUTES

        private string teamId;
        private string teamName;
        private string avatar;
        private DateTime createdAt;
        private string createdBy;
        private string projectId;
        private ETeamStatus status;

        #endregion

        #region TEAM CONTRUCTORS

        public Team()
        {
            teamId = string.Empty;
            teamName = "Anonymous";
            avatar = "PicAvatarDemoUser";
            createdAt = DateTime.Now;
            createdBy = string.Empty;
            projectId = string.Empty;
            status = default;
        }
        public Team(string teamId, string teamName, string avatar, DateTime createdAt, string createdBy, string projectId, ETeamStatus status)
        {
            this.teamId = teamId;
            this.teamName = teamName;
            this.avatar = avatar;
            this.createdAt = createdAt;
            this.createdBy = createdBy;
            this.projectId = projectId;
            this.status = status;
        }
        public Team(string teamName, string avatar, DateTime createdAt, string createdBy, string projectId, ETeamStatus status)
        {
            this.teamId = ModelUtil.GenerateModelId(EModelClassification.TEAM);
            this.teamName = teamName;
            this.avatar = avatar;
            this.createdAt = createdAt;
            this.createdBy = createdBy;
            this.projectId = projectId;
            this.status = status;
        }

        #endregion

        #region TEAM PROPERTIES

        public string TeamId
        {
            get { return this.teamId; }
            set { this.teamId = value; }
        }
        public string TeamName
        {
            get { return this.teamName; }
        }
        public string Avatar
        {
            get { return this.avatar; }
            set { this.avatar = value; }
        }
        public DateTime CreatedAt
        {
            get { return this.createdAt; }
            set { this.createdAt = value; }
        }
        public string CreatedBy
        {
            get { return this.createdBy; }
            set { this.createdBy = value; }
        }
        public string ProjectId
        {
            get { return this.projectId; }
            set { this.projectId = value; }
        }
        public ETeamStatus Status
        {
            get { return this.status; }
            set { this.status = value; }
        }

        #endregion

    }
}
