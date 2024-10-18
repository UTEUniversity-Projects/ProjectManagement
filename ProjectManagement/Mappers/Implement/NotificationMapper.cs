using System.Data;
using ProjectManagement.Enums;
using ProjectManagement.Models;
using ProjectManagement.Process;
using ProjectManagement.Utils;

namespace ProjectManagement.Mappers.Implement
{
    internal class NotificationMapper : IRowMapper<Notification>
    {
        public Notification MapRow(DataRow row)
        {
            string notificationId = row["notificationId"].ToString();
            string title = row["title"].ToString();
            string content = row["content"].ToString();
            ENotificationType type = EnumUtil.GetEnumFromDisplayName<ENotificationType>(row["type"].ToString());
            DateTime createdAt = DateTime.Parse(row["createdAt"].ToString());

            Notification notification = new Notification(notificationId, title, content, type, createdAt);

            return notification;
        }
    }
}
