using ProjectManagement.Database;
using ProjectManagement.Mappers.Implement;
using ProjectManagement.Models;
using System.Data.SqlClient;

namespace ProjectManagement.DAOs
{
    internal class CommentDAO : DBConnection
    {
        public static List<Comment> SelectList(string taskId)
        {
            return DBGetModel.GetModelList(DBTableNames.Comment, "taskId", taskId, new CommentMapper());
        }
        public static void Insert(Comment comment)
        {
            DBExecution.Insert(comment, DBTableNames.Comment);
        }
    }
}
