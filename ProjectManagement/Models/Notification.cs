using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ProjectManagement.Process;
using ProjectManagement.Enums;
using ProjectManagement.Utils;

namespace ProjectManagement.Models
{
    public class Notification
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
            notificationId = string.Empty;
            title = string.Empty;
            content = string.Empty;
            type = default;
            createdAt = DateTime.MinValue;
        }
        public Notification(string notificationId, string title, string content, ENotificationType type, DateTime createdAt)
        {
            this.notificationId = notificationId;
            this.title = title;
            this.content = content;
            this.type = type;
            this.createdAt = createdAt;
        }
        public Notification(string title, string content, ENotificationType type, DateTime createdAt)
        {
            this.notificationId = ModelUtil.GenerateModelId(EModelClassification.NOTIFICATION);
            this.title = title;
            this.content = content;
            this.type = type;
            this.createdAt = createdAt;
        }

        #endregion

        #region PROPERTIES

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
            get { return this.content; }
        }
        public ENotificationType Type
        {
            get { return this.type; }
        }
        public DateTime CreatedAt
        {
            get { return createdAt; }
            set { createdAt = value; }
        }

        #endregion

        #region FUNCTIONS 

        public static ENotificationType GetNotificationType(string objectId)
        {
            if (objectId.Length < 4) return default;

            string pattern = objectId.Substring(2, 2);
            if (pattern == ConvertEClassifyToString(EModelClassification.PROJECT)) return ENotificationType.PROJECT;
            if (pattern == ConvertEClassifyToString(EModelClassification.TASK)) return ENotificationType.TASK;
            if (pattern == ConvertEClassifyToString(EModelClassification.EVALUATION)) return ENotificationType.EVALUATION;
            if (pattern == ConvertEClassifyToString(EModelClassification.COMMENT)) return ENotificationType.COMMENT;
            if (pattern == ConvertEClassifyToString(EModelClassification.MEETING)) return ENotificationType.MEETING;
            return default;
        }
        private static string ConvertEClassifyToString(EModelClassification eClassify)
        {
            return ((int)eClassify).ToString().PadLeft(2, '0');
        }
        public Color GetTypeColor()
        {
            switch (this.type)
            {
                case ENotificationType.PROJECT:
                    return Color.FromArgb(94, 148, 255);
                case ENotificationType.TASK:
                    return Color.FromArgb(45, 237, 55);
                case ENotificationType.EVALUATION:
                    return Color.FromArgb(237, 62, 247);
                case ENotificationType.MEETING:
                    return Color.FromArgb(255, 87, 87);
                case ENotificationType.GIVEUP:
                    return Color.FromArgb(252, 182, 3);
                default:
                    return Color.Gray;
            }
        }
        public static string GetContentTypeProject(string senderName, string projectTopic)
        {
            return string.Format("{0} has suggested the [{1}] project to you", senderName, projectTopic);
        }
        public static string GetContentTypeRegistered(string teamName, string projectTopic)
        {
            return string.Format("{0} team has REGISTERED for the [{1}] project", teamName, projectTopic);
        }
        public static string GetContentRegisteredMembers(string senderName, string teamName, string projectTopic)
        {
            return string.Format("{0} has REGISTERED team [{1}] with you for the project [{2}]", senderName, teamName, projectTopic);
        }
        public static string GetContentTypeAccepted(string senderName, string projectTopic)
        {
            return string.Format("{0} has AGREED with your team for the project [{1}]", senderName, projectTopic);
        }
        public static string GetContentTypeRejected(string senderName, string projectTopic)
        {
            return string.Format("Ohhh, {0} has REJECTED with your team for the project [{1}]", senderName, projectTopic);
        }
        public static string GetContentTypeTask(string senderName, string taskTitle, string projectTopic)
        {
            return string.Format("{0} has createdAt a [{1}] task in the [{2}] project", senderName, taskTitle, projectTopic);
        }
        public static string GetContentTypeEvaluation(string senderName, string taskTitle)
        {
            return string.Format("{0} evaluated you in [{1}] task", senderName, taskTitle);
        }
        public static string GetContentTypeComment(string senderName, string comment, string taskTitle)
        {
            return string.Format("{0} commented [{1}] in [{2}] task", senderName, comment, taskTitle);
        }
        public static string GetContentTypeMeeting(string meetingTitle, string creator)
        {
            return string.Format("[{0}] was createdAt by [{1}]", meetingTitle, creator);
        }
        public static string GetContentTypeMeetingUpdated(string meetingTitle)
        {
            return string.Format("[{0}] meeting has content edited", meetingTitle);
        }
        public static string GetContentTypeGiveUp(string teamName, string projectTopic)
        {
            return string.Format("The [{0}] team abandoned the the [{1}] project", teamName, projectTopic);
        }
        public static string GetContentTypeGiveUpAccepted(string teamName, string projectTopic)
        {
            return string.Format("Your request has been ACCEPTED for [{0}] team abandoned the the [{1}] project", teamName, projectTopic);
        }
        public static string GetContentTypeGiveUpRejected(string teamName, string projectTopic)
        {
            return string.Format("Your request has been REJECTED for [{0}] team abandoned the the [{1}] project", teamName, projectTopic);
        }
        public static string GetContentCompletedProject(string projectTopic)
        {
            return string.Format("COMPLETED the [{0}] project", projectTopic);
        }

        #endregion

    }
}
