using System.Data;
using ProjectManagement.Models;
using ProjectManagement.Process;
using ProjectManagement.Enums;
using ProjectManagement.Utils;

namespace ProjectManagement.Mappers.Implement
{
    internal class ProjectMapper : IRowMapper<Project>
    {
        public Project MapRow(DataRow row)
        {
            string projectId = row["projectId"].ToString();
            string instructorId = row["instructorId"].ToString();
            string topic = row["topic"].ToString();
            string description = row["description"].ToString();
            string feature = row["feature"].ToString();
            string requirement = row["requirement"].ToString();
            int maxMember = int.Parse(row["maxMember"].ToString());
            EProjectStatus status = EnumUtil.GetEnumFromDisplayName<EProjectStatus>(row["status"].ToString());
            DateTime createdAt = DateTime.Parse(row["createdAt"].ToString());
            string createdBy = row["createdBy"].ToString();
            string fieldId = row["fieldId"].ToString();

            Project project = new Project(projectId, instructorId, topic, description, feature, requirement, maxMember,
                status, createdAt, createdBy, fieldId);

            return project;
        }

    }
}
