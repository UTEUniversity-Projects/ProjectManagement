using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementModel.Models
{
    #region PEOPLE ENUM

        public enum EGender
        {
            //[Display(Name = "Male")]
            Male,
            //[Display(Name = "Female")]
            Female
        }
        public enum ERole
        {
            //[Display(Name = "Student")]
            Student,
            //[Display(Name = "Lecture")]
            Lecture
        }

        #endregion

    internal class User
    {
        #region USER ATTRIBUTES

        protected string userId;
        protected string userName;
        protected string fullName;
        protected string password;
        protected string email;
        protected string phoneNumber;
        protected DateTime dateOfBirth;
        protected string citizenCode;
        protected string university;
        protected string faculty;
        protected EGender gender;
        protected string avatar;
        protected ERole role;
        protected DateTime joinAt;

        #endregion

        #region USER CONTRUCTOR

        public User()
        {
            userId = string.Empty;
            userName = string.Empty;
            fullName = string.Empty;
            password = string.Empty;
            email = string.Empty;
            phoneNumber = string.Empty;
            dateOfBirth = DateTime.Now;
            citizenCode = string.Empty;
            university = "HCM City University of Technology and Education";
            faculty = "Faculty of Information Technology";
            gender = EGender.Male;
            avatar = "PicAvatarDemoUser";
            role = ERole.Student;
            joinAt = DateTime.Now;
        }

        public User(string userId, string userName, string fullName, string password, string email, string phoneNumber, DateTime dateOfBirth, string citizenCode, string university, string faculty, EGender gender, string avatar, ERole role, DateTime joinAt)
        {
            this.userId = userId;
            this.userName = userName;
            this.fullName = fullName;
            this.password = password;
            this.email = email;
            this.phoneNumber = phoneNumber;
            this.dateOfBirth = dateOfBirth;
            this.citizenCode = citizenCode;
            this.university = university;
            this.faculty = faculty;
            this.gender = gender;
            this.avatar = avatar;
            this.role = role;
            this.joinAt = joinAt;
        }

        #endregion

        #region USER PROPERTIES

        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }
        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set { dateOfBirth = value; }
        }
        public string CitizenCode
        {
            get { return citizenCode; }
            set { citizenCode = value; }
        }
        public string University
        {
            get { return university; }
            set { university = value; }
        }
        public string Faculty
        {
            get { return faculty; }
            set { faculty = value; }
        }
        public EGender Gender
        {
            get { return gender; }
            set { gender = value; }
        }
        public string Avatar
        {
            get { return avatar; }
            set { avatar = value; }
        }
        public ERole Role
        {
            get { return role; }
            set { role = value; }
        }
        public DateTime JoinAt
        {
            get { return joinAt; }
            set { joinAt = value; }
        }

        #endregion

        #region CHECK INFORMATIONS

        public bool CheckFullName()
        {
            return this.fullName != string.Empty;
        }
        public bool CheckPassWord()
        {
            return this.password != string.Empty;
        }
        public bool CheckEmail()
        {
            return this.email != string.Empty;
            //&& peopleDAO.CheckNonExist("email", this.email);
        }
        public bool CheckPhoneNumber()
        {
            return this.phoneNumber != string.Empty && this.phoneNumber.All(char.IsDigit);
            //&& peopleDAO.CheckNonExist("phonenumber", this.phoneNumber);
        }
        public bool CheckDateOfBirth()
        {
            TimeSpan difference = DateTime.Now - this.dateOfBirth;
            int age = (int)(difference.TotalDays / 365.25);
            return age >= 18;
        }
        public bool CheckCitizenCode()
        {
            return this.citizenCode != string.Empty;
                    //&& peopleDAO.CheckNonExist("citizencode", this.citizenCode);
        }
        public bool CheckGender()
        {
            return this.gender == EGender.Male || this.gender == EGender.Female;
        }
        public bool CheckRole()
        {
            return this.role == ERole.Lecture || this.role == ERole.Student;
        }
        
        #endregion
    }
}
