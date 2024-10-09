using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Database;
using ProjectManagement.Models;
using ProjectManagement.Process;

namespace ProjectManagement.DAOs
{
    internal class TeamDAO : DBConnection
    {
        public TeamDAO() { }

        #region SELECT TEAM

        public List<Team> SelectList(string idThesis)
        {
            string command = string.Format("SELECT * FROM {0} WHERE idthesis = '{1}'", DBTableNames.DBThesisStatus, idThesis);

            DataTable dataTable = Select(command);

            List<Team> list = new List<Team>();
            foreach (DataRow row in dataTable.Rows)
            {
                Team team = SelectOnly(row["idteam"].ToString());
                list.Add(team);
            }

            return list;
        }
        public Team SelectOnly(string idTeam)
        {
            DataTable dt = Select(string.Format("SELECT * FROM {0} WHERE idteam = '{1}'", DBTableNames.DBMember, idTeam));

            if (dt.Rows.Count > 0) return GetFromDataRow(dt.Rows[0]);
            return new Team();
        }
        public Team SelectFollowThesis(Project thesis)
        {
            string command = string.Format("SELECT * FROM {0} WHERE idthesis = '{1}' and status = '{2}'",
                                            DBTableNames.DBThesisStatus, thesis.IdThesis, thesis.Status.ToString());

            DataTable table = Select(command);
            return SelectOnly(table.Rows[0]["idteam"].ToString());
        }
        public List<Team> SelectFollowPeople(User people)
        {
            string command = string.Format("SELECT * FROM {0} WHERE idaccount = '{1}'", DBTableNames.DBMember, people.IdAccount);

            DataTable dataTable = Select(command);

            List<Team> list = new List<Team>();
            foreach (DataRow row in dataTable.Rows)
            {
                Team team = SelectOnly(row["idteam"].ToString());
                list.Add(team);
            }

            return list;
        }

        #endregion

        #region TEAM DAO EXECUTION

        public void Insert(Team team)
        {
            foreach (User member in team.Members)
            {
                string command = string.Format("INSERT INTO {0} VALUES('{1}', '{2}', '{3}', '{4}', '{5}')",
                                            DBTableNames.DBMember, team.IdTeam, member.IdAccount, team.TeamName, team.Created, team.AvatarName);
                SQLExecuteByCommand(command);
            }
        }
        public void Delete(Team team)
        {
            foreach (User member in team.Members)
            {
                string command = string.Format("DELETE FROM {0} WHERE idteam = '{1}' and idaccount = '{2}'", DBTableNames.DBMember, team.IdTeam, member.IdAccount);
                SQLExecuteByCommand(command);
            }
        }
        public void DeleteListTeam(List<Team> teams)
        {
            foreach (Team team in teams) Delete(team);
        }

        #endregion

        #region Get Members by ID Team

        private List<User> GetMembersByIDTeam(string idTeam)
        {
            string command = string.Format("SELECT * FROM {0} WHERE idteam = '{1}'", DBTableNames.DBMember, idTeam);
            DataTable dataTable = Select(command);

            UserDAO peopleDAO = new UserDAO();
            List<User> list = new List<User>();
            foreach (DataRow row in dataTable.Rows)
            {
                User people = peopleDAO.SelectOnlyByID(row["idaccount"].ToString());
                list.Add(people);
            }

            return list;
        }

        #endregion

        #region Get Team From Data Row

        public Team GetFromDataRow(DataRow row)
        {
            string idTeam = row["idteam"].ToString();
            string teamName = row["name"].ToString();
            DateTime created = DateTime.Parse(row["created"].ToString());
            string avatarName = row["avatarname"].ToString();
            List<User> members = GetMembersByIDTeam(idTeam);

            Team team = new Team(idTeam, teamName, avatarName, created, members);
            return team;
        }

        #endregion

    }
}
