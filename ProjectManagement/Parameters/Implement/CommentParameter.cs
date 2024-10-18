using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Models;

namespace ProjectManagement.Parameters.Implement
{
    public class CommentParameter : ISqlParameter<Comment>
    {
        public List<SqlParameter> GetSqlParameters(Comment comment)
        {
            return new List<SqlParameter> 
            { 
                new SqlParameter()
            };
        }
    }
}
