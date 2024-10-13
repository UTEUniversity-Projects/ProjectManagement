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
            string sqlStr = string.Format("SELECT * FROM {0} WHERE teamId = @TeamId", DBTableNames.Member);

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
            string sqlStr = $"SELECT * FROM {DBTableNames.Team} WHERE projectId = @ProjectId AND status = @Accepted";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@Accepted", EnumUtil.GetDisplayName(ETeamStatus.ACCEPTED))
            };

            DataTable dt = DBExecution.ExecuteQuery(sqlStr, parameters);
            return SelectOnly(dt.Rows[0]["teamId"].ToString());
        }
        public static string SelectTeamByIdProject(string projectId)
        {
            string sqlStr = $"SELECT teamId FROM {DBTableNames.ProjectStatus} WHERE projectId = @ProjectId";

            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@projectId", projectId) };

            DataTable dt = DBExecution.ExecuteQuery(sqlStr, parameters);
            return dt.Rows[0]["teamId"].ToString();
        }
        public static List<Team> SelectFollowUser(Users user)
        {
            string sqlStr = $"SELECT teamId FROM {DBTableNames.JoinTeam} WHERE studentId = @StudentId";

            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@StudentId", user.UserId) };

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

        public static void Insert(Team team)
        {
            DBExecution.Insert(team, DBTableNames.Team);
        }

        public static void Delete(Team team)
        {
            DeleteMember(team.TeamId);

            string sqlStr = string.Format("DELETE FROM {0} WHERE teamId = @TeamId", DBTableNames.Team);

            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@TeamId", team.TeamId) };

            DBExecution.ExecuteNonQuery(sqlStr, parameters);
        }
        private static void DeleteMember(string teamId)
        {
            string sqlStr = string.Format("DELETE FROM {0} WHERE teamId = @TeamId", DBTableNames.JoinTeam);

            List<SqlParameter> parameters = new List<SqlParameter>{ new SqlParameter("@TeamId", teamId) };

            DBExecution.ExecuteNonQuery(sqlStr, parameters);
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

        public static List<Student> GetMembersByTeamId(string teamId)
        {
            string sqlStr = $"SELECT * FROM {DBTableNames.JoinTeam} WHERE teamId = @TeamId";

            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@TeamId", teamId) };

            DataTable dataTable = DBExecution.ExecuteQuery(sqlStr, parameters);
            List<Student> list = new List<Student>();

            foreach (DataRow row in dataTable.Rows)
            {
                Student student = StudentDAO.SelectOnlyByID(row["studentId"].ToString());
                list.Add(student);
            }

            return list;
        }

        #endregion

        #region TEAM UTILS

        public static int CountTeamFollowState(Project project)
        {
            string sqlStr = $"SELECT COUNT(*) AS NumTeams FROM {DBTableNames.ProjectStatus} " +
                $"WHERE projectId = @ProjectId and status = @Status";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ProjectId", project.ProjectId),
                new SqlParameter("@Status", EnumUtil.GetDisplayName(project.Status))
            };

            DataTable dt = DBExecution.ExecuteQuery(sqlStr, parameters);
            int num = 0;
            int.TryParse(dt.Rows[0]["NumTeams"].ToString(), out num);
            return num;
        }

        #endregion

    }
}
