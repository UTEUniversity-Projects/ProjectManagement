using Guna.UI2.WinForms;
using ProjectManagement.Models;
using ProjectManagement.Enums;
using ProjectManagement.Utils;
using ProjectManagement.DAOs;

namespace ProjectManagement
{
    public partial class FUserDetails : Form
    {
        private Users user = new Users();

        private UCStatisticalStudent uCStatisticalStudent = new UCStatisticalStudent();
        private UCStatisticalLecture uCstatisticalLecture = new UCStatisticalLecture();

        public FUserDetails(Users user)
        {
            InitializeComponent();
            SetInformation(user);
        }

        #region FUNCTIONS

        public void SetInformation(Users user)
        {
            this.user = user;
            InitUserControl();
        }
        private void InitUserControl()
        {
            gCirclePictureBoxAvatar.Image = WinformControlUtil.NameToImage(user.Avatar);
            lblViewHandle.Text = user.UserName;
            lblViewRole.Text = EnumUtil.GetDisplayName(user.Role);

            Action setupRole = (this.user.Role == EUserRole.LECTURE) ? new Action(SetupLectureRole) : new Action(SetupStudentRole);
            setupRole();

            gTextBoxFullname.Text = user.FullName;
            gTextBoxCitizencode.Text = user.CitizenCode;
            gTextBoxBirthday.Text = user.DateOfBirth.ToString("dd/MM/yyyy");   
            gTextBoxGender.Text = EnumUtil.GetDisplayName(user.Gender);
            gTextBoxEmail.Text = user.Email;
            gTextBoxPhonenumber.Text = user.PhoneNumber;

            gTextBoxIDAccount.Text = user.UserId.ToString();
            gTextBoxUniversity.Text = user.University;
            gTextBoxFaculty.Text = user.Faculty;
            gTextBoxWorkcode.Text = user.WorkCode;
        }
        public void SetupLectureRole()
        {
            this.pnlShowStatistical.Controls.Clear();
            this.uCstatisticalLecture.SetInformation(this.user);
            this.pnlShowStatistical.Controls.Add(uCstatisticalLecture);

            this.gShadowPanelContribution.Controls.Clear();
            Guna2PictureBox gPictureBoxState = GunaControlUtil.CreatePictureBox(Properties.Resources.PictureEmptyState, new Size(280, 280));
            gPictureBoxState.SizeMode = PictureBoxSizeMode.StretchImage;
            gPictureBoxState.Location = new Point(15, 30);
            this.gShadowPanelContribution.Controls.Add(gPictureBoxState);

        }
        public void SetupStudentRole()
        {
            this.pnlShowStatistical.Controls.Clear();
            this.uCStatisticalStudent.SetInformation(this.user);
            this.pnlShowStatistical.Controls.Add(uCStatisticalStudent);

            this.gCircleProgressBar.Value = Convert.ToInt32(this.uCStatisticalStudent.AvgContribute);
        }

        #endregion
    
    }
}
