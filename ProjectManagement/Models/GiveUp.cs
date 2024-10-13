using ProjectManagement.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Models
{
    public class GiveUp
    {

        #region GIVEUP ATTRIBUTES

        private string projectId;
        private string userId;
        private string reason;
        private DateTime createdAt;
        private EGiveUpStatus status;

        #endregion

        #region GIVEUP CONSTRUCTORS

        public GiveUp()
        {
            this.projectId = string.Empty;
            this.userId = string.Empty;
            this.reason = string.Empty;
            this.createdAt = DateTime.Now;
            this.status = default;
        }
        public GiveUp(string projectId, string userId, string reason, DateTime createdAt, EGiveUpStatus status)
        {
            this.projectId = projectId;
            this.userId = userId;
            this.reason = reason;
            this.createdAt = createdAt;
            this.status = status;
        }

        #endregion

        #region GIVEUP PROPERTIES

        public string ProjectId
        {
            get { return this.projectId; }
            set { this.projectId = value; }
        }
        public string UserId
        {
            get { return this.userId; }
            set { this.userId = value; }
        }
        public string Reason
        {
            get { return this.reason; }
            set { this.reason = value; }
        }
        public DateTime CreatedAt
        {
            get { return this.createdAt; }
            set { this.createdAt = value; }
        }
        public EGiveUpStatus Status
        {
            get { return this.status; }
            set { this.status = value; }
        }

        #endregion

        #region CHECK INFORMATIONS

        public bool CheckReason()
        {
            return this.reason != string.Empty;
        }

        #endregion

    }
}
