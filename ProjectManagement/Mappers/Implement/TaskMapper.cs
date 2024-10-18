using ProjectManagement.Enums;
using ProjectManagement.Models;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Mappers.Implement
{
    internal class TaskMapper : IRowMapper<Tasks>
    {
        public Tasks MapRow(DataRow row)
        {
            string taskId = row["taskId"].ToString();
            DateTime startAt = DateTime.Parse(row["startAt"].ToString());
            DateTime endAt = DateTime.Parse(row["endAt"].ToString());
            string title = row["title"].ToString();
            string description = row["description"].ToString();
            double progress = double.Parse(row["progress"].ToString());
            ETaskPriority priority = EnumUtil.GetEnumFromDisplayName<ETaskPriority>(row["priority"].ToString());
            DateTime createdAt = DateTime.Parse(row["createdAt"].ToString());
            string createdBy = row["createdBy"].ToString();
            string projectId = row["projectId"].ToString();

            Tasks task = new Tasks(taskId, startAt, endAt, title, description, progress, priority, createdAt, createdBy, projectId);
            return task;
        }
    }
}
