using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementModel.Models
{
    internal class Team
    {
        #region TEAM ATTRIBUTES

        private string teamId;
        private string teamName;
        private string avatar;
        private DateTime createdAt;
        private string createdBy;

        #endregion

        #region TEAM CONTRUCTOR

        public Team()
        {
            this.teamId = string.Empty;
            this.teamName = "Anonymous";
            this.avatar = "PicAvatarDemoUser";
            this.createdAt = DateTime.Now;
            this.createdBy = string.Empty;
        }
        public Team(string teamId, string teamName, string avatar, DateTime createdAt, string createdBy)
        {
            this.teamId = teamId;
            this.teamName = teamName;
            this.avatar = avatar;
            this.createdAt = createdAt;
            this.createdBy = createdBy;
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
            set { this.teamName = value; }
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

        #endregion

    }
}
