using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Process;

namespace ProjectManagement.Models
{
    public class Student : Users
    {
        public Student() : base() { }

        public Student(Users user)
        : base(user.UserId, user.UserName, user.FullName, user.Password, user.Email, user.PhoneNumber, user.DateOfBirth,
               user.CitizenCode, user.University, user.Faculty, user.Faculty, user.Gender, user.Avatar, user.Role, user.JoinAt)
        { }
    }
}
