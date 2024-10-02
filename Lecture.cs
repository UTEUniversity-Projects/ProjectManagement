using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementModel.Models
{
    internal class Lecture : User
    {
        #region LECTURE ATTRIBUTES

        private string lectureCode;

        #endregion

        #region LECTURE CONTRUCTORS

        public Lecture() : base()
        {
            lectureCode = string.Empty;
        }
        public Lecture(string userId, string lectureCode, string userName, string fullName, string password, string email, string phoneNumber, DateTime dateOfBirth, string citizenCode, string university, string faculty, EGender gender, string avatar, ERole role, DateTime joinAt) : base(userId, userName, fullName, password, email, phoneNumber, dateOfBirth, citizenCode, university, faculty, gender, avatar, role, joinAt)
        {
            this.lectureCode = lectureCode;
        }

        #endregion

        #region USER PROPERTIES

        public string LectureCode
        {
            get { return lectureCode; }
            set { lectureCode = value; }
        }

        #endregion
    }
}
