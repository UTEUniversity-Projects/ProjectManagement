using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementModel.Models
{
    #region NOTIFICATION ENUM

    public enum ENotificationType
    {
        //[Display(Name = "Thesis")]
        Thesis,
        //[Display(Name = "Task")]
        Task,
        //[Display(Name = "Evaluatiokn")]
        Evaluation,
        //[Display(Name = "Comment")]
        Comment,
        //[Display(Name = "Meeting")]
        Meeting,
        //[Display(Name = "Null")]
        Null,
    }

    #endregion
    internal class Notification
    {
        #region NOTIFICATION ATTRIBUTES

        private string notificationId;
        private string title;
        private string content;
        private ENotificationType type;
        private DateTime createdAt;

        #endregion

        #region NOTIFICATION CONTRUCTOR

        public Notification()
        {

        }
        public Notification(string notificationId, string title, string content, ENotificationType type, DateTime createdAt)
        {
            this.notificationId = notificationId;
            this.title = title;
            this.content = content;
            this.type = type;
            this.createdAt = createdAt;
        }

        #endregion

        #region NOTIFICATION PROPERTIES

        public string NotificationId 
        {
            get { return notificationId; } 
            set { notificationId = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string Content
        {
            get { return content; }
            set { content = value; }
        }
        public ENotificationType Type
        {
            get { return type; }
            set { type = value; }
        }
        public DateTime CreatedAt
        {
            get { return createdAt; }
            set { createdAt = value; }
        }

        #endregion
    }
}
