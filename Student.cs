using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementModel.Models
{
    internal class Student : User
    {
        #region STUDENT ATTRIBUTES

        private string studentCode;
        private string schoolYear;
        private string major;

        #endregion

        #region STUDENT CONTRUCTORS

        public Student() : base()
        {
            studentCode = string.Empty;
        }
        public Student(string userId, string userName, string fullName, string password, string email, string phoneNumber, DateTime dateOfBirth, string citizenCode, string university, string faculty, EGender gender, string avatar, ERole role, DateTime joinAt, string studentCode, string schoolYear, string major) : base(userId, userName, fullName, password, email, phoneNumber, dateOfBirth, citizenCode, university, faculty, gender, avatar, role, joinAt)
        {
            this.studentCode = studentCode;
            this.schoolYear = schoolYear;
            this.major = major;
        }

        #endregion

        #region USER PROPERTIES

        public string StudentCode
        {
            get { return studentCode; }
            set { studentCode = value; }
        }
        public string SchoolYear
        {
            get { return schoolYear; }
            set { schoolYear = value; }
        }
        public string Major
        {
            get { return major; }
            set { major = value; }
        }

        #endregion
    }
}
