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
            string sqlStr = "EXEC CreateComment @commentId, @content, @createdAt, @createdBy, @taskId";

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

        #region CHECK INFORMATIONS

        public static bool CheckIsNotEmpty(string input, string fieldName)
        {
            string sqlStr = "SELECT IsValid FROM dbo.IsNotEmpty(@input, @fieldName)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@input", input),
                new SqlParameter("@fieldName", fieldName)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                return Convert.ToBoolean(dataTable.Rows[0]["IsValid"]);
            }

            return false;
        }

        #endregion
    }
}
