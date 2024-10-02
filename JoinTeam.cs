using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementModel.Models
{
    public enum ERoleTeam
    {
        //[Display(Name = "Member")]
        Member,
        //[Display(Name = "Leader")]
        Leader
    }
    internal class JoinTeam
    {
        #region JOINTEAM ATTRIBUTE

        private string teamId;
        private string studentId;
        private ERoleTeam role;
        private DateTime joinAt;
        
        #endregion

        #region JOINTEAM CONTRCUTOR

        public JoinTeam()
        {

        }
        public JoinTeam(string teamId, string studentId, ERoleTeam role, DateTime joinAt)
        {
            this.teamId = teamId;
            this.studentId = studentId;
            this.role = role;
            JoinAt = joinAt;
        }

        #endregion

        #region JOINTEAM PROPERTIES

        public string TeamId 
        { 
            get { return teamId; }
            set { teamId = value; }
        }
        public string StudentId
        {
            get { return studentId; }
            set { studentId = value; }
        }
        public ERoleTeam Role
        {
            get { return role; }
            set { role = value; }
        }
        public DateTime JoinAt
        {
            get { return joinAt; }
            set { joinAt = value; }
        }

        #endregion
    }
}
