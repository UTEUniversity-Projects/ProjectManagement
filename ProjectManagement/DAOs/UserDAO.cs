using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.ApplicationServices;
using ProjectManagement.Database;
using ProjectManagement.Models;
using ProjectManagement.Enums;
using ProjectManagement.Mappers.Implement;
using System.Data.SqlClient;
using ProjectManagement.Utils;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ProjectManagement.DAOs
{
    internal class UserDAO : DBConnection
    {

        #region SELECT USER

        public static List<Users> SelectListByUserName(string userName, EUserRole role)
        {
            string sqlStr = "SELECT * FROM FUNC_SelectUsersByUserNameAndRole(@UserNameSyntax, @Role)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserNameSyntax", userName + "%"),
                new SqlParameter("@Role", EnumUtil.GetDisplayName(role))
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                List<Users> usersList = new List<Users>();
                UserMapper userMapper = new UserMapper();

                foreach (DataRow row in dataTable.Rows)
                {
                    Users user = userMapper.MapRow(row);

                    usersList.Add(user);
                }
                return usersList;
            }
            return new List<Users>();
        }
        public static Users SelectOnlyByID(string userId)
        {
            string sqlStr = "SELECT * FROM FUNC_SelectUserById(@UserId)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                UserMapper userMapper = new UserMapper();
                return userMapper.MapRow(dataTable.Rows[0]);
            }
            return null;
        }
        public static Users SelectOnlyByEmailAndPassword(string email, string password)
        {
            string sqlStr = "SELECT * FROM FUNC_SelectUserByEmailAndPassword(@Email, @Password)";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Email", email),
                new SqlParameter("@Password", password)
            };
            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                UserMapper userMapper = new UserMapper();
                return userMapper.MapRow(dataTable.Rows[0]);
            }
            return null;
        }        

        #endregion

        #region SELECT LIST ID USER

        public static List<string> SelectListId(EUserRole role)
        {
            string sqlStr = "SELECT userId FROM FUNC_SelectUserIdsByRole(@Role)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Role", EnumUtil.GetDisplayName(role))
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);
            List<string> list = new List<string>();

            foreach (DataRow row in dataTable.Rows)
            {
                list.Add(row["userId"].ToString());
            }

            return list;
        }

        #endregion

        #region USER DAO EXECUTION

        public static void Insert(Users user)
        {
            string sqlStr = "EXEC PROC_InsertUser @UserId, @UserName, @FullName, @Password, @Email, @PhoneNumber, " +
                                "@DateOfBirth, @CitizenCode, @University, @Faculty, @WorkCode, @Gender, @Avatar, @Role, @JoinAt";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", user.UserId),
                new SqlParameter("@UserName", user.UserName),
                new SqlParameter("@FullName", user.FullName),
                new SqlParameter("@Password", user.Password),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@PhoneNumber", user.PhoneNumber),
                new SqlParameter("@DateOfBirth", user.DateOfBirth),
                new SqlParameter("@CitizenCode", user.CitizenCode),
                new SqlParameter("@University", user.University),
                new SqlParameter("@Faculty", user.Faculty),
                new SqlParameter("@WorkCode", user.WorkCode),
                new SqlParameter("@Gender", EnumUtil.GetDisplayName(user.Gender)),
                new SqlParameter("@Avatar", user.Avatar),
                new SqlParameter("@Role", EnumUtil.GetDisplayName(user.Role)),
                new SqlParameter("@JoinAt", user.JoinAt)
            };
            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }
        public static void Update(Users user)
        {
            string sqlStr = "EXEC PROC_UpdateUser @UserId, @UserName, @FullName, @CitizenCode, " +
                                "@DateOfBirth, @PhoneNumber, @Email, @Gender";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", user.UserId),
                new SqlParameter("@UserName", user.UserName),
                new SqlParameter("@FullName", user.FullName),
                new SqlParameter("@CitizenCode", user.CitizenCode),
                new SqlParameter("@DateOfBirth", user.DateOfBirth),
                new SqlParameter("@PhoneNumber", user.PhoneNumber),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@Gender", EnumUtil.GetDisplayName(user.Gender))
            };
            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }


        #endregion
        
    }
}
