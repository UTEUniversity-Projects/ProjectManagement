using ProjectManagement.Process;
using ProjectManagement.Enums;
using ProjectManagement.Utils;

namespace ProjectManagement.Models
{
    public class Comment
    {

        #region COMMENT ATTRIBUTES

        private string commentId;
        private string content;
        private DateTime createdAt;
        private string createdBy;
        private string taskId;

        #endregion

        #region COMMENT CONSTRUCTORS

        public Comment()
        {
            commentId = string.Empty;
            content = string.Empty;
            createdAt = DateTime.MinValue;
            createdBy = string.Empty;
            taskId = string.Empty;
        }
        public Comment(string commentId, string content, DateTime createdAt, string createdBy, string taskId)
        {
            this.commentId = commentId;
            this.content = content;
            this.createdAt = createdAt;
            this.createdBy = createdBy;
            this.taskId = taskId;
        }
        public Comment(string content, DateTime createdAt, string createdBy, string taskId)
        {
            this.commentId = ModelUtil.GenerateModelId(EModelClassification.COMMENT);
            this.content = content;
            this.createdAt = createdAt;
            this.createdBy = createdBy;
            this.taskId = taskId;
        }

        #endregion

        #region COMMENT PROPERTIES

        public string CommentId
        {
            get { return commentId; }
            set { commentId = value; }
        }
        public string Content
        {
            get { return content; }
            set { content = value; }
        }
        public DateTime CreatedAt
        {
            get { return createdAt; }
            set { createdAt = value; }
        }
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public string TaskId
        {
            get { return taskId; }
            set { taskId = value; }
        }

        #endregion

        #region CHECK INFORMATIONS

        public bool CheckContent()
        {
            return this.content != string.Empty;
        }

        #endregion

    }
}
