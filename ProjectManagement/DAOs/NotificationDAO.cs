using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Database;
using ProjectManagement.Models;
using ProjectManagement.Mappers.Implement;
using System.Data.SqlClient;
using ProjectManagement.Enums;
using ProjectManagement.Utils;

namespace ProjectManagement.DAOs
{
    internal class NotificationDAO : DBConnection
    {
        
        #region SELECT NOTIFICATION

        public static List<Notification> SelectList(Users user)
        {
            string sqlStr = string.Format("SELECT * FROM {0} AS N JOIN (SELECT * FROM {1} WHERE userId = @UserId) AS VN " +
                                            "ON N.notificationId = VN.notificationId " +
                                            "ORDER BY createdAt DESC",
                                            DBTableNames.Notification, DBTableNames.ViewNotification);

            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@UserId", user.UserId) };

            return DBGetModel.GetModelList(sqlStr, parameters, new NotificationMapper());
        }

        #endregion

        #region NOTIFICATION DAO EXECUTION

        public static void Insert(Notification notification)
        {
            DBExecution.Insert(notification, DBTableNames.Notification);
        }
        public static void InsertFollowListUser(string teamId, string content, ENotificationType type)
        {
            Notification notification = new Notification("Notification", content, type, DateTime.Now);
            DBExecution.Insert(notification, DBTableNames.Notification);

            string sqlStr = string.Format("INSERT INTO {0} VALUES (@UserId, )");

            List<Student> students = TeamDAO.GetMembersByTeamId(teamId);


            foreach (Student student in students)
            {

            }
        }
        public static void Delete(Notification notification)
        {
            DBExecution.Delete(DBTableNames.Notification, "notificationId", notification.NotificationId);
        }
        public static void UpdateIsSaw(string userId, string notificationId, bool flag)
        {
            string sqlStr = string.Format("UPDATE {0} SET seen = @Seen WHERE userId = @UserId AND notificationId = @NotificationId",
                                                DBTableNames.Notification);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Seen", flag ? 1 : 0),
                new SqlParameter("@UserId", userId),
                new SqlParameter("@NotificationId", notificationId)
            };

            DBExecution.ExecuteNonQuery(sqlStr, parameters);
        }
        public static void UpdateIsFavorite(string notificationId, bool flag) { }

        #endregion
            
    }
}
