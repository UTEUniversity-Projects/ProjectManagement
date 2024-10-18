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
using Microsoft.VisualBasic.ApplicationServices;
using ProjectManagement.MetaData;
using ProjectManagement.Mappers;

namespace ProjectManagement.DAOs
{
    internal class NotificationDAO : DBConnection
    {
        
        #region SELECT NOTIFICATION

        public static List<NotificationMeta> SelectList(string userId)
        {
            string sqlStr = string.Format("SELECT * FROM {0} AS N JOIN (SELECT * FROM {1} WHERE userId = @UserId) AS VN " +
                                            "ON N.notificationId = VN.notificationId " +
                                            "ORDER BY createdAt DESC",
                                            DBTableNames.Notification, DBTableNames.ViewNotification);

            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@UserId", userId) };
            DataTable dataTable = DBExecution.ExecuteQuery(sqlStr, parameters);

            List<NotificationMeta> list = new List<NotificationMeta>();
            NotificationMapper notificationMapper = new NotificationMapper();

            foreach (DataRow row in dataTable.Rows)
            {
                Notification notification = notificationMapper.MapRow(row);
                bool isSaw = row["seen"].ToString() == "True" ? true : false;

                list.Add(new NotificationMeta(notification, isSaw));
            }

            return list;
        }

        #endregion

        #region NOTIFICATION DAO EXECUTION

        public static void Insert(Notification notification, string userId)
        {
            DBExecution.Insert(notification, DBTableNames.Notification);
            InsertViewNotification(userId, notification.NotificationId, false);
        }
        public static void InsertFollowTeam(string teamId, string content, ENotificationType type)
        {
            Notification notification = new Notification("Notification", content, type, DateTime.Now);
            DBExecution.Insert(notification, DBTableNames.Notification);

            List<Member> members = TeamDAO.GetMembersByTeamId(teamId);

            foreach (Member member in members)
            {
                InsertViewNotification(member.User.UserId, notification.NotificationId, false);
            }
        }
        private static void InsertViewNotification(string userId, string notificationId, bool seen)
        {
            string sqlStr = string.Format("INSERT INTO {0} (userId, notificationId, seen) " +
                "VALUES (@UserId, @NotificationId, @Seen)", DBTableNames.ViewNotification);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("NotificationId", notificationId),
                new SqlParameter("@Seen", seen == true ? 1 : 0)
            };

            DBExecution.ExecuteNonQuery(sqlStr, parameters);
        }

        public static void Delete(string userId, string notificationId)
        {
            string sqlStr = string.Format("DELETE FROM {0} WHERE userId = @UserId AND notificationId = @NotificationId",
                                                DBTableNames.ViewNotification);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@NotificationId", notificationId)
            };

            DBExecution.ExecuteNonQuery(sqlStr, parameters);

            DBExecution.Delete(DBTableNames.Notification, "notificationId", notificationId);
        }
        public static void UpdateIsSaw(string userId, string notificationId, bool flag)
        {
            string sqlStr = string.Format("UPDATE {0} SET seen = @Seen WHERE userId = @UserId AND notificationId = @NotificationId",
                                                DBTableNames.ViewNotification);

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
