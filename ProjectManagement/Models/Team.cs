using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Process;

namespace ProjectManagement.Models
{
    public class Team
    {
        private MyProcess myProcess = new MyProcess();

        #region TEAM ATTRIBUTES

        private string idteam;
        private string teamName;
        private string avatarName;
        private DateTime created;
        private List<User> members;

        #endregion

        #region TEAM CONTRUCTOR

        public Team()
        {
            this.idteam = string.Empty;
            this.teamName = "Anonymous";
            this.avatarName = "PicAvatarDemoUser";
            this.created = DateTime.Now;
            this.members = new List<User>();
        }
        public Team(List<User> members)
        {
            this.idteam = myProcess.GenIDClassify(EClassify.Team);
            this.teamName = "Anonymous";
            this.avatarName = "PicAvatarDemoUser";
            this.created = DateTime.Now;
            this.members = members;
        }
        public Team(string teamName, string avatarName, List<User> members)
        {
            this.idteam = myProcess.GenIDClassify(EClassify.Team);
            this.teamName = teamName;
            this.avatarName = avatarName;
            this.created = DateTime.Now;
            this.members = members;
        }
        public Team(string teamName, string avatarName, DateTime created, List<User> members)
        {
            this.idteam = myProcess.GenIDClassify(EClassify.Team);
            this.teamName = teamName;
            this.avatarName = avatarName;
            this.created = created;
            this.members = members;
        }
        public Team(string idteam, string teamName, string avatarName, DateTime created, List<User> members)
        {
            this.idteam = idteam;
            this.teamName = teamName;
            this.avatarName = avatarName;
            this.created = created;
            this.members = members;
        }

        #endregion

        #region TEAM PROPERTIES

        public string IdTeam
        {
            get { return this.idteam; }
        }
        public string TeamName
        {
            get { return this.teamName; }
        }
        public string AvatarName
        {
            get { return this.avatarName; }
        }
        public DateTime Created
        {
            get { return this.created; }
        }
        public List<User> Members
        {
            get { return this.members; }
        }

        #endregion

    }
}
