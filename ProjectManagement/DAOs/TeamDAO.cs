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

namespace ProjectManagement.DAOs
{
    internal class TeamDAO : DBConnection
    {

        #region SELECT TEAM

        public static Team SelectOnly(string teamId)
        {
            string sqlStr = string.Format("SELECT * FROM {0} WHERE teamId = @TeamId", DBTableNames.Team);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@TeamId", teamId)
            };

            return DBGetModel.GetModel(sqlStr, parameters, new TeamMapper());
        }
        public static List<Team> SelectList(string projectId)
        {
            string sqlStr = $"SELECT * FROM {DBTableNames.Team} WHERE projectId = @ProjectId";

            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@ProjectId", projectId) };

            DataTable dataTable = DBExecution.ExecuteQuery(sqlStr, parameters);
            List<Team> list = new List<Team>();

            foreach (DataRow row in dataTable.Rows)
            {
                Team team = SelectOnly(row["teamId"].ToString());
                list.Add(team);
            }

            return list;
        }
        public static Team SelectFollowProject(string projectId)
        {
            string sqlStr = $"SELECT * FROM {DBTableNames.Team} WHERE projectId = @ProjectId";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ProjectId", projectId)
            };

            DataTable dataTable = DBExecution.ExecuteQuery(sqlStr, parameters);
            return SelectOnly(dataTable.Rows[0]["teamId"].ToString());
        }
        public static List<Team> SelectFollowUser(string userId)
        {
            string sqlStr = $"SELECT teamId FROM {DBTableNames.JoinTeam} WHERE studentId = @StudentId";

            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@StudentId", userId) };

            DataTable dataTable = DBExecution.ExecuteQuery(sqlStr, parameters);
            List<Team> list = new List<Team>();

            foreach (DataRow row in dataTable.Rows)
            {
                Team team = SelectOnly(row["teamId"].ToString());
                list.Add(team);
            }

            return list;
        }

        #endregion

        #region TEAM DAO EXECUTION

        public static void Insert(Team team, List<Users> members)
        {
            DBExecution.Insert(team, DBTableNames.Team);

            string sqlStr = string.Format("INSERT INTO {0} (teamId, studentId, role, joinAt) " +
                "VALUES (@TeamId, @StudentId, @Role, @JoinAt)", DBTableNames.JoinTeam);

            foreach (Users student in members)
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@TeamId", team.TeamId),
                    new SqlParameter("@StudentId", student.UserId),
                    new SqlParameter("@Role", EnumUtil.GetDisplayName(ETeamRole.MEMBER)),
                    new SqlParameter("@JoinAt", DateTime.Now.ToString("yyyy-MM-dd hh:MM:ss"))
                };

                DBExecution.ExecuteNonQuery(sqlStr, parameters);
            }
        }

        public static void Delete(string teamId)
        {
            DBExecution.Delete(DBTableNames.JoinTeam, "teamId", teamId);
            DBExecution.Delete(DBTableNames.Team, "teamId", teamId);
        }
        public static void DeleteFollowProject(string projectId)
        {
            string sqlStr = $"SELECT teamId FROM {DBTableNames.Team} WHERE projectId = @ProjectId";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ProjectId", projectId)
            };

            DataTable dataTable = DBExecution.ExecuteQuery(sqlStr, parameters);

            foreach (DataRow row in dataTable.Rows)
            {
                Delete(row["teamId"].ToString());
            }
        }

        public static void DeleteListTeam(List<Team> listTeam)
        {
            foreach (Team team in listTeam)
            {
                string sqlStr = string.Format("UPDATE {0} SET status = @Rejected WHERE teamId = @TeamId", DBTableNames.Team);

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@TeamId", team.TeamId),
                    new SqlParameter("@Rejected", EnumUtil.GetDisplayName(ETeamStatus.REJECTED))
                };

                DBExecution.ExecuteNonQuery(sqlStr, parameters);
            }
        }

        public static void UpdateStatus(string teamId, string status)
        {
            string sqlStr = string.Format("UPDATE {0} SET status = @Status WHERE teamId = @TeamId", DBTableNames.Team);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Status", status),
                new SqlParameter("@TeamId", teamId)
            };

            DBExecution.ExecuteNonQuery(sqlStr, parameters);
        }       

        #endregion

        #region GET MEMBERS

        public static List<Users> GetMembersByTeamId(string teamId)
        {
            string sqlStr = $"SELECT * FROM {DBTableNames.JoinTeam} WHERE teamId = @TeamId";

            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@TeamId", teamId) };

            DataTable dataTable = DBExecution.ExecuteQuery(sqlStr, parameters);
            List<Users> list = new List<Users>();

            foreach (DataRow row in dataTable.Rows)
            {
                Users student = UserDAO.SelectOnlyByID(row["studentId"].ToString());
                list.Add(student);
            }

            return list;
        }

        #endregion

        #region TEAM UTILS

        public static int CountTeamFollowState(string projectId, EProjectStatus status)
        {
            string sqlStr = $"SELECT COUNT(*) AS NumTeams FROM {DBTableNames.Team} " +
                $"WHERE projectId = @ProjectId and status = @Status";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@Status", EnumUtil.GetDisplayName(status))
            };
            DataTable dataTable = DBExecution.ExecuteQuery(sqlStr, parameters);
            int num = 0;
            int.TryParse(dataTable.Rows[0]["NumTeams"].ToString(), out num);
            return num;
        }

        #endregion

    }
}
