using ProjectManagement.Models;
using System.Data;

namespace ProjectManagement.Mappers.Implement
{
    public class CommentMapper : IRowMapper<Comment>
    {
        public Comment MapRow(DataRow row)
        {
            string commentId = row["commentId"].ToString();
            string content = row["content"].ToString();
            DateTime createdAt = DateTime.Parse(row["createdAt"].ToString());
            string createdBy = row["createdBy"].ToString();
            string taskId = row["taskId"].ToString();

            Comment comment = new Comment(commentId, content, createdAt, createdBy, taskId);
            return comment;
        }
    }
}
