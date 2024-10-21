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
            string sqlStr = string.Format("SELECT * FROM {0} WHERE userName LIKE @UserNameSyntax AND role = @Role",
                                DBTableNames.User);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserNameSyntax", userName + "%"),
                new SqlParameter("@Role", EnumUtil.GetDisplayName(role))
            };

            return DBGetModel.GetModelList(sqlStr, parameters, new UserMapper());
        }
        public static Users SelectOnlyByID(string userId)
        {
            string sqlStr = string.Format("SELECT * FROM {0} WHERE userId = @UserId", DBTableNames.User);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId)
            };

            return DBGetModel.GetModel(sqlStr, parameters, new UserMapper());
        }
        public static Users SelectOnlyByEmailAndPassword(string email, string password)
        {
            string sqlStr = string.Format("SELECT * FROM {0} WHERE email = @Email AND password = @Password",
                                        DBTableNames.User);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Email", email),
                new SqlParameter("@Password", password)
            };

            return DBGetModel.GetModel(sqlStr, parameters, new UserMapper());
        }        

        #endregion

        #region SELECT LIST ID USER

        public static List<string> SelectListId(EUserRole role)
        {
            string sqlStr = $"SELECT userId FROM {DBTableNames.User} WHERE role = @Role";

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
            DBExecution.Insert(user, DBTableNames.User);
        }
        public static void Update(Users user)
        {
            DBExecution.Update(user, DBTableNames.User, "userId", user.UserId);
        }
        public static bool CheckNonExist(string tableName, string field, string information)
        {
            string sqlStr = string.Format("SELECT 1 FROM {0} WHERE {1} = @Information", tableName, field);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Information", information)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            return dataTable.Rows.Count == 0;
        }

        #endregion

    }
}
