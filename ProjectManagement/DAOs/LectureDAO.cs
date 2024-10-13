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
    internal class LectureDAO : UserDAO
    {
        public static Lecture SelectOnlyByID(string lectureId)
        {
            string sqlStr = string.Format("SELECT * FROM {0} JOIN {1} ON {0}.userId = {1}.lectureId " +
                                            "WHERE lectureId = @LectureId", DBTableNames.User, DBTableNames.Student);

            List<SqlParameter> parameters = new List<SqlParameter>{ new SqlParameter("@LectureId", lectureId) };

            return DBGetModel.GetModel(sqlStr, parameters, new LectureMapper());
        }
    }
}
