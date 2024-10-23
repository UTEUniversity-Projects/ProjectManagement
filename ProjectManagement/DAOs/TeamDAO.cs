using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Database;
using ProjectManagement.Models;
using ProjectManagement.Mappers.Implement;
using System.Data.SqlClient;
using ProjectManagement.Utils;
using ProjectManagement.Enums;
using ProjectManagement.MetaData;

namespace ProjectManagement.DAOs
{
    internal class TeamDAO : DBConnection
    {

        #region SELECT TEAM

        public static Team SelectFollowProject(string projectId)
        {
            string sqlStr = "SELECT * FROM FUNC_SelectTeamByProjectId(@ProjectId)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ProjectId", projectId)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                TeamMapper teamMapper = new TeamMapper();
                return teamMapper.MapRow(dataTable.Rows[0]);
            }

            return null;
        }
        public static List<Team> SelectList(string projectId)
        {
            string sqlStr = "SELECT * FROM FUNC_SelectTeamByProjectId(@ProjectId)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ProjectId", projectId)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            List<Team> teams = new List<Team>();

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                TeamMapper teamMapper = new TeamMapper();

                foreach (DataRow row in dataTable.Rows)
                {
                    Team team = teamMapper.MapRow(row);
                    teams.Add(team);
                }
            }

            return teams;
        }
        public static List<Team> SelectFollowUser(string userId)
        {
            string sqlStr = "SELECT * FROM FUNC_SelectTeamsByUserId(@UserId)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            List<Team> teams = new List<Team>();

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                TeamMapper teamMapper = new TeamMapper();

                foreach (DataRow row in dataTable.Rows)
                {
                    Team team = teamMapper.MapRow(row);
                    teams.Add(team);
                }
            }

            return teams;
        }

        #endregion

        #region SELECT MEMBERS

        public static List<Member> GetMembersByTeamId(string teamId)
        {
            string sqlStr = "SELECT * FROM FUNC_GetMembersByTeamId(@TeamId)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@TeamId", teamId)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            List<Member> list = new List<Member>();

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    Users student = UserDAO.SelectOnlyByID(row["studentId"].ToString());
                    ETeamRole role = EnumUtil.GetEnumFromDisplayName<ETeamRole>(row["role"].ToString());
                    DateTime joinAt = DateTime.Parse(row["joinAt"].ToString());

                    list.Add(new Member(student, role, joinAt));
                }
            }

            return list.OrderBy(m => m.Role).ToList();
        }

        #endregion

        #region TEAM DAO CRUD

        public static void Insert(Team team, List<Member> members)
        {
            string sqlStr = "EXEC PROC_InsertTeam @TeamId, @TeamName, @Avatar, @CreatedAt, @CreatedBy, @ProjectId, @Status";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@TeamId", team.TeamId),
                new SqlParameter("@TeamName", team.TeamName),
                new SqlParameter("@Avatar", team.Avatar),
                new SqlParameter("@CreatedAt", team.CreatedAt),
                new SqlParameter("@CreatedBy", team.CreatedBy),
                new SqlParameter("@ProjectId", team.ProjectId),
                new SqlParameter("@Status", EnumUtil.GetDisplayName(team.Status))
            };

            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);

            InsertTeamMembers(team.TeamId, members);
        }
        public static void InsertTeamMembers(string teamId, List<Member> members)
        {
            string sqlStr = "EXEC PROC_InsertTeamMember @TeamId, @StudentId, @Role, @JoinAt";

            foreach (Member member in members)
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@TeamId", teamId),
                    new SqlParameter("@StudentId", member.User.UserId),
                    new SqlParameter("@Role", EnumUtil.GetDisplayName(member.Role)),
                    new SqlParameter("@JoinAt", member.JoinAt.ToString("yyyy-MM-dd HH:mm:ss"))
                };

                DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
            }
        }

        public static void Delete(string teamId)
        {
            string sqlStr = "EXEC PROC_DeleteTeam @TeamId";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@TeamId", teamId)
            };

            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }
        public static void DeleteListTeam(List<Team> teams)
        {
            foreach (Team team in teams)
            {
                Delete(team.TeamId);
            }
        }
        public static void DeleteFollowProject(string projectId)
        {
            string sqlStr = "SELECT * FROM FUNC_GetTeamIdsByProjectId(@ProjectId)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ProjectId", projectId)
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            foreach (DataRow row in dataTable.Rows)
            {
                Delete(row["teamId"].ToString());
            }
        }

        #endregion

        #region TEAM DAO UTIL

        public static int CountTeamFollowState(string projectId, EProjectStatus status)
        {
            string sqlStr = "SELECT * FROM FUNC_CountTeamsFollowState(@ProjectId, @Status)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@Status", EnumUtil.GetDisplayName(status))
            };

            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            int num = 0;
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                int.TryParse(dataTable.Rows[0]["NumTeams"].ToString(), out num);
            }

            return num;
        }

        #endregion

    }
}
