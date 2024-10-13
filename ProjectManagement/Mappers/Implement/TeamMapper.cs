using System.Data;
using ProjectManagement.Models;
using ProjectManagement.Enums;
using ProjectManagement.Utils;

namespace ProjectManagement.Mappers.Implement
{
    internal class TeamMapper : IRowMapper<Team>
    {
        public Team MapRow(DataRow row)
        {
            string teamId = row["teamId"].ToString();
            string teamName = row["teamName"].ToString();
            string avatar = row["avatar"].ToString();
            DateTime createdAt = DateTime.Parse(row["createdAt"].ToString());
            string createdBy = row["createdBy"].ToString();
            string projectId = row["projectId"].ToString();
            ETeamStatus status = EnumUtil.GetEnumFromDisplayName<ETeamStatus>(row["status"].ToString());

            Team team = new Team(teamId, teamName, avatar, createdAt, createdBy, projectId, status);

            return team;
        }
    }
}
