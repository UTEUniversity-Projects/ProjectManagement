using ProjectManagement.DAOs;
using ProjectManagement.Enums;
using ProjectManagement.Models;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Mappers.Implement
{
    internal class StudentMapper : IRowMapper<Student>
    {
        public Student MapRow(DataRow row)
        {
            string userId = row["userId"].ToString();
            string userName = row["userName"].ToString();
            string fullName = row["fullName"].ToString();
            string password = row["password"].ToString();
            string email = row["email"].ToString();
            string phoneNumber = row["phoneNumber"].ToString();
            DateTime dateOfBirth = DateTime.Parse(row["dateOfBirth"].ToString());
            string citizenCode = row["citizenCode"].ToString();
            string university = row["university"].ToString();
            string faculty = row["faculty"].ToString();
            string workCode = row["workCode"].ToString();
            EUserGender gender = EnumUtil.GetEnumFromDisplayName<EUserGender>(row["gender"].ToString());
            string avatar = row["avatar"].ToString();
            EUserRole role = EnumUtil.GetEnumFromDisplayName<EUserRole>(row["role"].ToString());
            DateTime joinAt = DateTime.Parse(row["joinAt"].ToString());

            Users user = new Users(userId, userName, fullName, password, email, phoneNumber, dateOfBirth, citizenCode,
                university, faculty, workCode, gender, avatar, role, joinAt);

            Student student = new Student(user);

            return student;
        }
    }
}
