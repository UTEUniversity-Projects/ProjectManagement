using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Database;
using ProjectManagement.Models;
using ProjectManagement.Process;

namespace ProjectManagement.DAOs
{
    internal class UserDAO : DBConnection
    {
        private MyProcess myProcess = new MyProcess();

        public UserDAO() { }

        #region SELECT PEOPLE

        public List<User> SelectListByUserName(string username, ERole role)
        {
            string command = string.Format("SELECT * FROM {0} WHERE handle LIKE '{1}%' and role = '{2}'",
                                DBTableNames.DBPeople, username, role);

            DataTable dataTable = Select(command);

            List<User> list = new List<User>();
            foreach (DataRow row in dataTable.Rows)
            {
                list.Add(GetFromDataRow(row));
            }

            return list;
        }
        public User SelectOnlyByID(string idPeople)
        {
            DataTable dt = Select(string.Format("SELECT * FROM {0} WHERE idaccount = '{1}'", DBTableNames.DBPeople, idPeople));

            if (dt.Rows.Count > 0) return GetFromDataRow(dt.Rows[0]);
            return new User();
        }
        public User SelectOnlyByEmailAndPassword(string email, string password)
        {
            DataTable dt = Select(string.Format("SELECT * FROM {0} WHERE email = '{1}' and password = '{2}'",
                                        DBTableNames.DBPeople, email, password));

            if (dt.Rows.Count > 0) return GetFromDataRow(dt.Rows[0]);
            return null;
        }        

        #endregion

        #region SELECT LIST ID PEOPLE

        public List<string> SelectListID(ERole role)
        {
            string command = string.Format("SELECT idaccount FROM {0} WHERE role = '{1}'", DBTableNames.DBPeople, role.ToString());
            DataTable table = Select(command);
            List<string> list = new List<string>();

            foreach (DataRow row in table.Rows)
            {
                list.Add(row["idaccount"].ToString());
            }
            return list;
        }

        #endregion

        #region PEOPLE DAO EXECUTION

        public void Insert(User people)
        {
            ExecuteQueryPeople(people,
                    "INSERT INTO {0} VALUES ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}'," +
                    " '{10}', '{11}', '{12}', '{13}', '{14}')", "Register", false);
        }
        public void Update(User people)
        {
            ExecuteQueryPeople(people, "UPDATE {0} SET " +
                "idaccount = '{1}', fullname = '{2}', citizencode = '{3}', birthday = {4}, gender = '{5}', " +
                "email = '{6}', phonenumber = '{7}', handle = '{8}', role = '{9}', university = '{10}', " +
                "faculty = '{11}', workcode = {12}, password = '{13}', avatarname = '{14}' WHERE idaccount = '{1}'",
                "Update", true);
        }
        public bool CheckNonExist(string field, string information)
        {
            return SQLCheckNonExist(string.Format("SELECT * FROM {0} WHERE {1} = '{2}'",
                                    DBTableNames.DBPeople, field, information));
        }

        #endregion

        #region Get From Data Row

        public User GetFromDataRow(DataRow row)
        {
            string idaccount = row["idaccount"].ToString();
            string fullname = row["fullname"].ToString();
            string citizenCode = row["citizencode"].ToString();
            DateTime birthday = DateTime.Parse(row["birthday"].ToString());
            EGender gender = myProcess.GetEnumFromDisplayName<EGender>(row["gender"].ToString());
            string email = row["email"].ToString();
            string phoneNumber = row["phonenumber"].ToString();
            string handle = row["handle"].ToString();
            ERole role = myProcess.GetEnumFromDisplayName<ERole>(row["role"].ToString());
            string workCode = row["workcode"].ToString();
            string password = row["password"].ToString();
            string avatarName = row["avatarname"].ToString();

            User people = new User(idaccount, fullname, citizenCode, birthday, gender,
                                        email, phoneNumber, handle, role, workCode, password, avatarName);

            return people;
        }

        #endregion

    }
}
