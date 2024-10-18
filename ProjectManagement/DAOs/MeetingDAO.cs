using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Database;
using ProjectManagement.Models;
using ProjectManagement.Mappers.Implement;
using System.Data.SqlClient;

namespace ProjectManagement.DAOs
{
    internal class MeetingDAO : DBConnection
    {

        #region SELECT MEETING

        public static Meeting SelectOnly(string meetingId)
        {
            return DBGetModel.GetModel(DBTableNames.Meeting, "meetingId", meetingId, new MeetingMapper());
        }
        public static List<Meeting> SelectByProject(string projectId)
        {
            return DBGetModel.GetModelList(DBTableNames.Meeting, "projectId", projectId, new MeetingMapper());
        }

        #endregion

        #region MEETING DAO EXCUTION

        public static void Insert(Meeting meeting)
        {
            DBExecution.Insert(meeting, DBTableNames.Meeting);
        }
        public static void Delete(Meeting meeting)
        {
            DBExecution.Delete(DBTableNames.Meeting, "meetingId", meeting.MeetingId);
        }
        public static void Update(Meeting meeting)
        {
            DBExecution.Update(meeting, DBTableNames.Meeting, "meetingId", meeting.MeetingId);
        }

        #endregion

    }
}
