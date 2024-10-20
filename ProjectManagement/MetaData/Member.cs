using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Enums;
using ProjectManagement.Models;

namespace ProjectManagement.MetaData
{
    public class Member
    {
        private Users user;
        private ETeamRole role;
        private DateTime joinAt;

        public Member()
        {
            this.user = new Users();
            this.role = default;
            this.joinAt = DateTime.MinValue;
        }
        public Member(Users user, ETeamRole role, DateTime joinAt)
        {
            this.user = user;
            this.role = role;
            this.joinAt = joinAt;
        }

        public Users User { get { return this.user; } }
        public ETeamRole Role { get { return this.role; } }
        public DateTime JoinAt { get { return this.joinAt; } }
    }
}
