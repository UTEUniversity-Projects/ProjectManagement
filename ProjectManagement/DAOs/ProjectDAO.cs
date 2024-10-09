using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using ProjectManagement.Database;
using ProjectManagement.Models;
using ProjectManagement.Process;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ProjectManagement.DAOs
{
    internal class ProjectDAO : DBConnection
    {
        private MyProcess myProcess = new MyProcess();

        public ProjectDAO() { }

        #region SELECT THESIS

        public List<Project> SelectList(string command)
        {
            DataTable dataTable = Select(command);

            List<Project> list = new List<Project>();
            foreach (DataRow row in dataTable.Rows)
            {
                list.Add(GetFromDataRow(row));
            }

            return list;
        }
        public Project SelectOnly(string idThesis)
        {
            DataTable dt = Select(string.Format("SELECT * FROM {0} WHERE idthesis = '{1}'", DBTableNames.DBThesis, idThesis));

            if (dt.Rows.Count > 0) return GetFromDataRow(dt.Rows[0]);
            return new Project();
        }
        public Project SelectFollowTeam(string idTeam)
        {
            DataTable dt = Select(string.Format("SELECT * FROM {0} WHERE idteam = '{1}'", DBTableNames.DBThesisStatus, idTeam));

            if (dt.Rows.Count > 0) return SelectOnly(dt.Rows[0]["idthesis"].ToString());
            return new Project();
        }
        public List<Dictionary<Project, int>> GetMaxSubscribers()
        {
            string command = "SELECT idthesis, SUM(SL) AS total_SL " +
                             "FROM ThesisStatus " +
                             "JOIN (SELECT idteam, COUNT(idaccount) AS SL FROM Team GROUP BY idteam) AS Team " +
                             "ON ThesisStatus.idteam = Team.idteam " +
                             "WHERE ThesisStatus.status IN ('Registered', 'Processing', 'Completed') " +
                             "GROUP BY idthesis " +
                             "ORDER BY total_SL DESC";
            DataTable dataTable = Select(command);

            List<Dictionary<Project, int>> resultList = new List<Dictionary<Project, int>>();

            foreach (DataRow row in dataTable.Rows)
            {
                string idThesis = row["idthesis"].ToString();
                Project thesis = SelectOnly(idThesis);
                int total = Convert.ToInt32(row["total_SL"]);
                Dictionary<Project, int> resultDict = new Dictionary<Project, int>
                {
                    { thesis, total }
                };
                resultList.Add(resultDict);
            }
            return resultList;
        }

        #endregion

        #region SELECT THESIS FOLLOW ROLE

        public List<Project> SelectListRoleLecture(string idAccount)
        {
            string command = string.Format("SELECT * FROM {0} WHERE idinstructor = '{1}'", DBTableNames.DBThesis, idAccount);
            return SelectList(command);
        }
        public List<Project> SelectListRoleStudent(string idAccount)
        {
            string command = string.Format("SELECT * FROM {0} WHERE status IN ('Published', 'Registered') " +
                                           "AND NOT EXISTS(SELECT 1 FROM {1} WHERE {1}.idthesis = {0}.idthesis " +
                                           "AND idteam IN (SELECT idteam FROM {2} WHERE idaccount = '{3}'))",
                                           DBTableNames.DBThesis, DBTableNames.DBThesisStatus, DBTableNames.DBMember, idAccount);
            return SelectList(command);
        }
        public List<Project> SelectListModeMyTheses(string idAccount)
        {
            string command = string.Format("SELECT {0}.* FROM {0} INNER JOIN {1} ON {0}.idthesis = {1}.idthesis " +
                                           "WHERE {1}.idteam IN (SELECT idteam FROM {2} WHERE idaccount = '{3}')",
                                            DBTableNames.DBThesis, DBTableNames.DBThesisStatus, DBTableNames.DBMember, idAccount);
            return SelectList(command);
        }
        public List<Project> SelectListModeNotCompleted(string idAccount)
        {
            string command = string.Format("SELECT {0}.* FROM {0} INNER JOIN {1} ON {0}.idthesis = {1}.idthesis " +
                                           "WHERE {1}.idteam IN (SELECT idteam FROM {2} WHERE idaccount = '{3}') " +
                                           "AND {1}.status = 'NotCompleted'",
                                            DBTableNames.DBThesis, DBTableNames.DBThesisStatus, DBTableNames.DBMember, idAccount);
            return SelectList(command);
        }
        public List<Project> SelectListModeMyCompletedTheses(string idAccount)
        {
            string command = $"SELECT {DBTableNames.DBThesis}.* " +
                             $"FROM {DBTableNames.DBThesis} INNER JOIN {DBTableNames.DBThesisStatus} " +
                             $"ON {DBTableNames.DBThesis}.idthesis = {DBTableNames.DBThesisStatus}.idthesis " +
                             $"WHERE {DBTableNames.DBThesisStatus}.idteam IN (SELECT idteam FROM {DBTableNames.DBMember} WHERE idaccount = '{idAccount}') " +
                             $"and {DBTableNames.DBThesis}.status = 'Completed'";
            return SelectList(command);
        }
        #endregion

        #region SEARCH THESIS

        public List<Project> SearchRoleLecture(string idAccount, string topic)
        {
            string command = string.Format("SELECT * FROM {0} WHERE idinstructor = '{1}' and topic LIKE '{2}%'",
                                DBTableNames.DBThesis, idAccount, topic);
            return SelectList(command);

        }
        public List<Project> SearchRoleStudent(string topic)
        {
            string command = string.Format("SELECT * FROM {0} WHERE status IN ('{1}', '{2}') and topic LIKE '{3}%'",
                                    DBTableNames.DBThesis, EThesisStatus.Published.ToString(), EThesisStatus.Registered.ToString(), topic);
            return SelectList(command);

        }

        #endregion

        #region THESIS DAO EXECUTION

        public void Insert(Project thesis)
        {
            ExecuteQueryThesis(thesis, "INSERT INTO {0} " +
                "VALUES ('{1}', '{2}', '{3}', '{4}', {5}, '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', {12}, '{13}', '{14}')",
                "Create", true);
        }
        public void Delete(Project thesis)
        {
            ExecuteQueryThesis(thesis, "DELETE FROM {0} WHERE idthesis = '{1}'",
                "Delete", false);
        }
        public void Update(Project thesis)
        {
            ExecuteQueryThesis(thesis, "UPDATE {0} SET " +
                "idthesis = '{1}', topic = '{2}', field = '{3}', level = '{4}', maxmembers = {5}, " +
                "description = '{6}', publishdate = '{7}', technology = '{8}', functions = '{9}', requirements = '{10}', " +
                "idcreator = '{11}', isfavorite = {12}, status = '{13}', idinstructor = '{14}' WHERE idthesis = '{1}'",
                "Update", false);
        }
        public void UpdateStatus(Project thesis, EThesisStatus status)
        {
            string command = string.Format("UPDATE {0} SET status = '{1}' WHERE idthesis = '{2}'",
                                                DBTableNames.DBThesis, status.ToString(), thesis.IdThesis);
            SQLExecuteByCommand(command);
        }
        public void UpdateFavorite(Project thesis)
        {
            string command = string.Format("UPDATE {0} SET isfavorite = {1} WHERE idthesis = '{2}'",
                                                DBTableNames.DBThesis, thesis.IsFavorite ? 1 : 0, thesis.IdThesis);
            SQLExecuteByCommand(command);
        }

        #endregion

        #region Get Thesis From Data Row

        public Project GetFromDataRow(DataRow row)
        {
            string idThesis = row["idthesis"].ToString();
            string topic = row["topic"].ToString();
            EField field = myProcess.GetEnumFromDisplayName<EField>(row["field"].ToString());
            ELevel level = myProcess.GetEnumFromDisplayName<ELevel>(row["level"].ToString());
            int maxMembers = int.Parse(row["maxmembers"].ToString());
            string description = row["description"].ToString();
            DateTime publishDate = DateTime.Parse(row["publishdate"].ToString());
            string technology = row["technology"].ToString();
            string functions = row["functions"].ToString();
            string requirements = row["requirements"].ToString();
            string idCreator = row["idcreator"].ToString();
            bool isFavorite = row["isfavorite"].ToString() == "True" ? true : false;
            EThesisStatus status = myProcess.GetEnumFromDisplayName<EThesisStatus>(row["status"].ToString());
            string idInstructor = row["idinstructor"].ToString();

            Project thesis = new Project(idThesis, topic, field, level, maxMembers, description, publishDate, technology,
                                        functions, requirements, idCreator, isFavorite, status, idInstructor);
            return thesis;
        }

        #endregion 
    }
}
