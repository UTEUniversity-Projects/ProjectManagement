using ProjectManagement.Database;
using ProjectManagement.Mappers.Implement;
using ProjectManagement.Models;
using System.Data;
using System.Data.SqlClient;

namespace ProjectManagement.DAOs
{
    internal class CommentDAO : DBConnection
    {
        public static List<Comment> SelectList(string taskId)
        {
            string sqlStr = "SELECT * FROM FUNC_ViewComment(@taskId) ORDER BY createdAt ASC";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@taskId", taskId)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                List<Comment> commentList = new List<Comment>();
                CommentMapper commentMapper = new CommentMapper();

                foreach (DataRow row in dataTable.Rows)
                {
                    Comment comment = commentMapper.MapRow(row);
                    commentList.Add(comment);
                }

                return commentList;
            }

            return new List<Comment>();
        }
        public static void Insert(Comment comment)
        {
            string sqlStr = "EXEC PROC_CreateComment @commentId, @content, @createdAt, @createdBy, @taskId";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@commentId", comment.CommentId),
                new SqlParameter("@content", comment.Content),
                new SqlParameter("@createdAt", comment.CreatedAt),
                new SqlParameter("@createdBy", comment.CreatedBy),
                new SqlParameter("@taskId", comment.TaskId)
            };
            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, "Create Comment");
        }
    }
}
