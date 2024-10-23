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
using Guna.UI2.AnimatorNS;

namespace ProjectManagement.DAOs
{
    internal class NotificationDAO : DBConnection
    {
        
        #region SELECT NOTIFICATION

        // 1.
        public static List<NotificationMeta> SelectList(string userId)
        {
            string sqlStr = string.Format("SELECT * FROM dbo.FUNC_GetNotificationsByUserId(@UserId)");

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
        // 2.
        private static List<string> GetFavoriteList(string userId)
        {
            string sqlStr = string.Format("SELECT * FROM dbo.FUNC_GetFavoriteNotifications(@UserId)");
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

        // 3.
        public static void InsertOnly(Notification notification)
        {
            string sqlStr = "EXEC dbo.PROC_AddNotification " +
                            "@NotificationId, @Title, @Content, @Type, @CreatedAt";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@NotificationId", notification.NotificationId),
                new SqlParameter("@Title", notification.Title),
                new SqlParameter("@Content", notification.Content),
                new SqlParameter("@Type", notification.Type),
                new SqlParameter("@CreatedAt", notification.CreatedAt)
            };
            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }

        public static void Insert(Notification notification, string userId)
        {
            InsertOnly(notification);
            InsertViewNotification(userId, notification.NotificationId, false);
        }
        public static void InsertFollowTeam(string teamId, string content, ENotificationType type)
        {
            Notification notification = new Notification("Notification", content, type, DateTime.Now);
            InsertOnly(notification);

            List<Member> members = TeamDAO.GetMembersByTeamId(teamId);

            foreach (Member member in members)
            {
                InsertViewNotification(member.User.UserId, notification.NotificationId, false);
            }
        }
        public static void InsertFollowPeoples(List<Users> peoples, string content, ENotificationType type)
        {
            Notification notification = new Notification("Notification", content, type, DateTime.Now);
            InsertOnly(notification);

            foreach (Users people in peoples)
            {
                InsertViewNotification(people.UserId, notification.NotificationId, false);
            }
        }

        // 4.
        public static void InsertViewNotification(string userId, string notificationId, bool seen)
        {
            string sqlStr = "EXEC dbo.PROC_AddViewNotification @UserId, @NotificationId, @Seen";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@NotificationId", notificationId),
                new SqlParameter("@Seen", seen ? 1 : 0)
            };

            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }

        // 5.
        public static void Delete(string userId, string notificationId)
        {
            string sqlStr = string.Format("EXEC dbo.PROC_DeleteNotification @UserId, @NotificationId");

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@NotificationId", notificationId)
            };

            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }

        // 6.
        public static void UpdateIsSaw(string userId, string notificationId, bool flag)
        {
            string sqlStr = string.Format("EXEC dbo.PROC_UpdateViewNotification @Seen, @UserId, @NotificationId");

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Seen", flag ? 1 : 0),
                new SqlParameter("@UserId", userId),
                new SqlParameter("@NotificationId", notificationId)
            };

            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }

        // 7.
        public static void UpdateFavorite(string userId, string notificationId, bool isFavorite)
        {
            string sqlStr = string.Format("EXEC dbo.PROC_UpdateFavoriteNotification @IsFavorite, @UserId, @NotificationId");

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@IsFavorite", isFavorite ? 1 : 0),
                new SqlParameter("@UserId", userId),
                new SqlParameter("@NotificationId", notificationId)
            };

            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }

        #endregion

    }
}
