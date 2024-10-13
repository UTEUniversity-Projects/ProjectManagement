using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Process;

namespace ProjectManagement.Models
{
    public class Lecture : Users
    {
        public Lecture() : base() { }

        public Lecture(Users user)
        : base(user.UserId, user.UserName, user.FullName, user.Password, user.Email, user.PhoneNumber, user.DateOfBirth,
               user.CitizenCode, user.University, user.Faculty, user.WorkCode, user.Gender, user.Avatar, user.Role, user.JoinAt)
        { }
    }
}
