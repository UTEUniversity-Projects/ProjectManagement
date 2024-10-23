﻿using System;
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
            string sqlStr = $"SELECT * FROM FUNC_GetMeetingById(@meetingId)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@meetingId", meetingId)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                MeetingMapper meetingMapper = new MeetingMapper();
                return meetingMapper.MapRow(dataTable.Rows[0]);
            }

            return null;
        }
        public static List<Meeting> SelectByProject(string projectId)
        {
            string sqlStr = string.Format("SELECT * FROM FUNC_GetMeetingsByProjectId(@projectId)");
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@projectId", projectId)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            List<Meeting> listMeetings = new List<Meeting>();

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    Meeting meeting = DBGetModel.GetModelFromDataRow(row, new MeetingMapper());
                    listMeetings.Add(meeting);
                }
            }

            return listMeetings;

        }

        #endregion

        #region MEETING DAO EXCUTION

        public static void Insert(Meeting meeting)
        {
            string sqlStr = "EXEC PROC_CreateMeeting @meetingId, @title, @description, @startAt, @location, @link, @createdAt, @createdBy, @projectId";
             
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                    new SqlParameter("@meetingId", meeting.MeetingId),
                    new SqlParameter("@title", meeting.Title),
                    new SqlParameter("@description", meeting.Description),
                    new SqlParameter("@startAt", meeting.StartAt),
                    new SqlParameter("@location", meeting.Location),
                    new SqlParameter("@link", meeting.Link),
                    new SqlParameter("@createdAt", meeting.CreatedAt),
                    new SqlParameter("@createdBy", meeting.CreatedBy),
                    new SqlParameter("@projectId", meeting.ProjectId)
            };
            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, "Create Meeting"); ;
        }
        public static void Delete(Meeting meeting)
        {
            string sqlStr = "EXEC dbo.PROC_DeleteMeeting @MeetingId";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@MeetingId", meeting.MeetingId)
            };
            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, "Delete Meeting");

        }

        public static void Update(Meeting meeting)
        {
            string sqlStr = "EXEC dbo.PROC_UpdateMeeting " +
                            "@MeetingId, @Title, @Description, " +
                            "@StartAt, @Location, @Link, " +
                            "@CreatedAt, @CreatedBy, @ProjectId";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@MeetingId", meeting.MeetingId),
                new SqlParameter("@Title", meeting.Title),
                new SqlParameter("@Description", meeting.Description),
                new SqlParameter("@StartAt", meeting.StartAt),
                new SqlParameter("@Location", meeting.Location),
                new SqlParameter("@Link", meeting.Link),
                new SqlParameter("@CreatedAt", meeting.CreatedAt),
                new SqlParameter("@CreatedBy", meeting.CreatedBy),
                new SqlParameter("@ProjectId", meeting.ProjectId)
            };
            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }

        #endregion

    }
}
