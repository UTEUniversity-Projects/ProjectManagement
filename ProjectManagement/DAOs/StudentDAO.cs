using ProjectManagement.Database;
using ProjectManagement.Mappers.Implement;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.DAOs
{
    internal class StudentDAO : UserDAO
    {
        public static Student SelectOnlyByID(string studentId)
        {
            string sqlStr = string.Format("SELECT * FROM {0} JOIN {1} ON {0}.userId = {1}.studentId " +
                                            "WHERE studentId = @StudentId", DBTableNames.User, DBTableNames.Student);

            List<SqlParameter> parameters = new List<SqlParameter>{ new SqlParameter("@StudentId", studentId) };

            return DBGetModel.GetModel(sqlStr, parameters, new StudentMapper());
        }
    }
}
