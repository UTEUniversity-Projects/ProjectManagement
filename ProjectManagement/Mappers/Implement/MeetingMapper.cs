using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Mappers.Implement
{
    internal class MeetingMapper : IRowMapper<Meeting>
    {
        public Meeting MapRow(DataRow row)
        {
            string meetingId = row["meetingId"].ToString();
            string title = row["title"].ToString();
            string description = row["description"].ToString();
            DateTime startAt = DateTime.Parse(row["startAt"].ToString());
            string location = row["location"].ToString();
            string link = row["link"].ToString();
            DateTime createdAt = DateTime.Parse(row["createdAt"].ToString());
            string createdBy = row["createdBy"].ToString();
            string projectId = row["projectId"].ToString();

            Meeting meeting = new Meeting(meetingId, title, description, startAt, location, link, createdAt, createdBy, projectId);

            return meeting;
        }
    }
}
