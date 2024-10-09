using Guna.UI2.WinForms;
using Guna.UI2.WinForms.Suite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjectManagement.DAOs;
using ProjectManagement.Models;
using ProjectManagement.Process;

namespace ProjectManagement
{
    public partial class FPeopleDetails : Form
    {
        private MyProcess myProcess = new MyProcess();
        private User people = new User();

        private UCStatisticalStudent uCStatisticalStudent = new UCStatisticalStudent();
        private UCStatisticalLecture uCstatisticalLecture = new UCStatisticalLecture();

        public FPeopleDetails(User people)
        {
            InitializeComponent();
            SetInformation(people);
        }

        #region FUNCTIONS

        public void SetInformation(User people)
        {
            this.people = people;
            InitUserControl();
        }
        private void InitUserControl()
        {
            gCirclePictureBoxAvatar.Image = myProcess.NameToImage(people.AvatarName);
            lblViewHandle.Text = people.Handle;
            lblViewRole.Text = people.Role.ToString();

            Action setupRole = (this.people.Role == ERole.Lecture) ? new Action(SetupLectureRole) : new Action(SetupStudentRole);
            setupRole();

            gTextBoxFullname.Text = people.FullName;
            gTextBoxCitizencode.Text = people.CitizenCode;
            gTextBoxBirthday.Text = people.Birthday.ToString("dd/MM/yyyy");   
            gTextBoxGender.Text = people.Gender.ToString();
            gTextBoxEmail.Text = people.Email;
            gTextBoxPhonenumber.Text = people.PhoneNumber;

            gTextBoxIDAccount.Text = people.IdAccount.ToString();
            gTextBoxUniversity.Text = people.University;
            gTextBoxFaculty.Text = people.Faculty;
            gTextBoxWorkcode.Text = people.WorkCode;
        }
        public void SetupLectureRole()
        {
            this.pnlShowStatistical.Controls.Clear();
            this.uCstatisticalLecture.SetInformation(this.people);
            this.pnlShowStatistical.Controls.Add(uCstatisticalLecture);

            this.gShadowPanelContribution.Controls.Clear();
            Guna2PictureBox gPictureBoxState = myProcess.CreatePictureBox(Properties.Resources.PictureEmptyState, new Size(280, 280));
            gPictureBoxState.SizeMode = PictureBoxSizeMode.StretchImage;
            gPictureBoxState.Location = new Point(15, 30);
            this.gShadowPanelContribution.Controls.Add(gPictureBoxState);

        }
        public void SetupStudentRole()
        {
            this.pnlShowStatistical.Controls.Clear();
            this.uCStatisticalStudent.SetInformation(this.people);
            this.pnlShowStatistical.Controls.Add(uCStatisticalStudent);

            this.gCircleProgressBar.Value = Convert.ToInt32(this.uCStatisticalStudent.AvgContribute);
        }

        #endregion
    
    }
}
