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
            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            List<NotificationMeta> list = new List<NotificationMeta>();
            NotificationMapper notificationMapper = new NotificationMapper();
            List<string> favoriteNotifications = GetFavoriteList(userId);

            foreach (DataRow row in dataTable.Rows)
            {
                Notification notification = notificationMapper.MapRow(row);
                bool isSaw = row["seen"].ToString() == "True" ? true : false;

                list.Add(new NotificationMeta(notification, isSaw, favoriteNotifications.Contains(notification.NotificationId)));
            }

            return list;
        }
        private static List<string> GetFavoriteList(string userId)
        {
            string sqlStr = string.Format("SELECT * FROM {0} WHERE userId = @UserId", DBTableNames.FavoriteNotification);
            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@UserId", userId) };
            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            List<string> favoriteProjects = new List<string>();
            foreach (DataRow row in dataTable.Rows)
            {
                favoriteProjects.Add(row["notificationId"].ToString());
            }

            return favoriteProjects;
        }

        #endregion

        #region NOTIFICATION DAO EXECUTION

        public static void InsertOnly(Notification notification)
        {
            DBExecution.Insert(notification, DBTableNames.Notification);
        }
        public static void Insert(Notification notification, string userId)
        {
            InsertOnly(notification);
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
        public static void InsertFollowPeoples(List<Users> peoples, string content, ENotificationType type)
        {
            Notification notification = new Notification("Notification", content, type, DateTime.Now);
            DBExecution.Insert(notification, DBTableNames.Notification);

            foreach (Users people in peoples)
            {
                InsertViewNotification(people.UserId, notification.NotificationId, false);
            }
        }
        public static void InsertViewNotification(string userId, string notificationId, bool seen)
        {
            string sqlStr = string.Format("INSERT INTO {0} (userId, notificationId, seen) " +
                "VALUES (@UserId, @NotificationId, @Seen)", DBTableNames.ViewNotification);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@NotificationId", notificationId),
                new SqlParameter("@Seen", seen == true ? 1 : 0)
            };

            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
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

            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);

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

            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }
        public static void UpdateFavorite(string userId, string notificationId, bool isFavorite)
        {
            string sqlStr = string.Empty;

            if (isFavorite == false)
            {
                sqlStr = string.Format("DELETE FROM {0} WHERE userId = @UserId AND notificationId = @NotificationId", DBTableNames.FavoriteNotification);
            }
            else
            {
                sqlStr = string.Format("INSERT INTO {0} (userId, notificationId) VALUES (@UserId, @NotificationId)", DBTableNames.FavoriteNotification);
            }

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@NotificationId", notificationId)
            };

            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }

        #endregion

    }
}
