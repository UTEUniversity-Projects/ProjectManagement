using System.Data;
using ProjectManagement.Models;
using ProjectManagement.Process;
using ProjectManagement.Enums;
using ProjectManagement.Utils;

namespace ProjectManagement.Mappers.Implement
{
    internal class UserMapper : IRowMapper<Users>
    {
        public Users MapRow(DataRow row)
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

            return user;
        }
    }
}
