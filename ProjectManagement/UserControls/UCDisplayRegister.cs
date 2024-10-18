using Guna.UI2.WinForms;
using ProjectManagement.DAOs;
using ProjectManagement.Enums;
using ProjectManagement.Models;
using ProjectManagement.Utils;

namespace ProjectManagement
{
    public partial class UCDisplayRegister : UserControl
    {

        private Users user = new Users();

        private bool flagCheck = false;
        private Image pictureAvatar = Properties.Resources.PicAvatarDemoUser;

        public UCDisplayRegister()
        {
            InitializeComponent();
            SetUpUserControl();
        }

        #region PROPERTIES

        public Guna2Button GButtonBack
        {
            get { return this.gButtonBack; }
        }
        public bool FlagCheck
        {
            set { flagCheck = value; }
        }
        public Guna2Button GButtonLoadLogin
        {
            get { return this.gButtonLoadLogin; }
        }

        #endregion

        #region FUNCTIONS

        private void SetUpUserControl()
        {
            EnumUtil.AddEnumsToComboBox(gComboBoxGender, typeof(EUserGender));
            gButtonLoadLogin.Hide();
            InitAvatarList();
        }
        private void InitAvatarList()
        {
            flpAvatarList.Controls.Clear();
            flpAvatarList.Controls.Add(CreateAvatarPictureBox(Properties.Resources.PicAvatarNine));
            flpAvatarList.Controls.Add(CreateAvatarPictureBox(Properties.Resources.PicAvatarTwo));
            flpAvatarList.Controls.Add(CreateAvatarPictureBox(Properties.Resources.PicAvatarSeven));
            flpAvatarList.Controls.Add(CreateAvatarPictureBox(Properties.Resources.PicAvatarSix));
            flpAvatarList.Controls.Add(CreateAvatarPictureBox(Properties.Resources.PicAvatarTen));
            flpAvatarList.Controls.Add(CreateAvatarPictureBox(Properties.Resources.PicAvatarFour));
            flpAvatarList.Controls.Add(CreateAvatarPictureBox(Properties.Resources.PicAvatarThree));
            flpAvatarList.Controls.Add(CreateAvatarPictureBox(Properties.Resources.PicAvatarFive));
            flpAvatarList.Controls.Add(CreateAvatarPictureBox(Properties.Resources.PicAvatarOne));
            flpAvatarList.Controls.Add(CreateAvatarPictureBox(Properties.Resources.PicAvatarEight));
            flpAvatarList.Controls.Add(CreateAvatarPictureBox(Properties.Resources.PicAvatarDemoUser));
        }
        public void InitDataControls()
        {
            gTextBoxFullname.Text = string.Empty;
            gTextBoxCitizencode.Text = string.Empty;
            DateTime dateTime = DateTime.Now.AddYears(-18);
            gDateTimePickerBirthday.Value = dateTime;
            gComboBoxGender.StartIndex = 0;
            gTextBoxEmail.Text = string.Empty;
            gTextBoxPhonenumber.Text = string.Empty;
            gTextBoxUserName.Text = string.Empty;
            gTextBoxPassword.Text = string.Empty;
            gTextBoxConfirmPassword.Text = string.Empty;
            gTextBoxUniversity.Text = string.Empty;
            gTextBoxFaculty.Text = string.Empty;
            gTextBoxWorkcode.Text = string.Empty;
            gCirclePictureBoxAvatar.Image = Properties.Resources.PicAvatarDemoUser;
        }
        public void RunCheckUserInfor()
        {
            WinformControlUtil.RunCheckDataValid(user.CheckFullName() || flagCheck, erpFullName, gTextBoxFullname, "Full name cannot be empty");
            WinformControlUtil.RunCheckDataValid(user.CheckCitizenCode() || flagCheck, erpCitizenCode, gTextBoxCitizencode, "Citizen code is already exists or empty");
            WinformControlUtil.RunCheckDataValid(user.CheckBirthday() || flagCheck, erpBirthday, gDateTimePickerBirthday, "Not yet 18 years old");
            WinformControlUtil.RunCheckDataValid(user.CheckGender() || flagCheck, erpGender, gComboBoxGender, "Gender cannot be empty");
            WinformControlUtil.RunCheckDataValid(user.CheckEmail() || flagCheck, erpEmail, gTextBoxEmail, "Email address is already exists or invalid");
            WinformControlUtil.RunCheckDataValid(user.CheckPhoneNumber() || flagCheck, erpPhonenumber, gTextBoxPhonenumber, "Phone number is already exists or invalid");
            WinformControlUtil.RunCheckDataValid(user.CheckUserName() || flagCheck, erpHandle, gTextBoxUserName, "Username is already exists or invalid");
            WinformControlUtil.RunCheckDataValid(user.CheckPassWord(gTextBoxConfirmPassword.Text) || flagCheck, erpConfirmPassword, gTextBoxConfirmPassword, "Confirmation password does not match");
            WinformControlUtil.RunCheckDataValid(user.CheckUniversity() || flagCheck, erpUniversity, gTextBoxUniversity, "University cannot be empty");
            WinformControlUtil.RunCheckDataValid(user.CheckFaculty() || flagCheck, erpFaculty, gTextBoxFaculty, "Faculty cannot be empty");
            WinformControlUtil.RunCheckDataValid(user.CheckWorkCode() || flagCheck, erpWorkCode, gTextBoxWorkcode, "Work code is already exists or invalid");
        }
        private bool CheckUserInformationValid()
        {
            RunCheckUserInfor();

            return user.CheckFullName() && user.CheckCitizenCode() && user.CheckBirthday() && user.CheckGender() && user.CheckEmail()
                    && user.CheckPhoneNumber() && user.CheckUserName() && user.CheckPassWord(gTextBoxConfirmPassword.Text)
                    && user.CheckUniversity() && user.CheckFaculty() && user.CheckWorkCode();
        }

        private PictureBox CreateAvatarPictureBox(Image image)
        {
            PictureBox pictureBox = new PictureBox();
            pictureBox.Size = new Size(100, 100);
            pictureBox.Image = image;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.BackColor = Color.White;
            pictureBox.Click += PictureBoxAvatar_Clicked;

            return pictureBox;
        }

        #endregion

        #region EVENT gCirclePictureBoxAvatar

        private void gCirclePictureBoxAvatar_MouseEnter(object sender, EventArgs e)
        {
            gCirclePictureBoxAvatar.Image = Properties.Resources.PictureCameraHover;
        }

        private void gCirclePictureBoxAvatar_MouseLeave(object sender, EventArgs e)
        {
            gCirclePictureBoxAvatar.Image = this.pictureAvatar;
        }

        #endregion

        #region EVENT gButtonRegister

        private EUserRole GetRole()
        {
            if (gRadioButtonStudent.Checked == true)
            {
                return EUserRole.STUDENT;
            }
            return EUserRole.LECTURE;
        }

        private void gButtonRegister_Click(object sender, EventArgs e)
        {
            this.user = new Users(gTextBoxUserName.Text, gTextBoxFullname.Text, gTextBoxPassword.Text, gTextBoxEmail.Text,
                gTextBoxPhonenumber.Text, gDateTimePickerBirthday.Value, gTextBoxCitizencode.Text, gTextBoxUniversity.Text, gTextBoxFaculty.Text, gTextBoxWorkcode.Text,
                (EUserGender)EnumUtil.ConvertStringToEnum(gComboBoxGender, typeof(EUserGender)), WinformControlUtil.ImageToName(gCirclePictureBoxAvatar.Image),
                GetRole(), DateTime.Now);

            this.flagCheck = false;
            if (CheckUserInformationValid())
            {
                UserDAO.Insert(this.user);
                this.flagCheck = true;
                gButtonLoadLogin.PerformClick();
            }
        }

        #endregion

        #region EVENT gCirclePictureBoxAvatar

        private void PictureBoxAvatar_Clicked(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            this.pictureAvatar = pictureBox.Image;
            gCirclePictureBoxAvatar.Image = this.pictureAvatar;
        }
        private void gCirclePictureBoxAvatar_Click(object sender, EventArgs e)
        {
            gShadowPanelAvatar.Show();
        }

        #endregion

        #region EVENT TextChanged and ValueChanged

        private void gTextBoxFullname_TextChanged(object sender, EventArgs e)
        {
            this.user.FullName = gTextBoxFullname.Text;
            WinformControlUtil.RunCheckDataValid(user.CheckFullName() || flagCheck, erpFullName, gTextBoxFullname, "Full name cannot be empty");
        }
        private void gTextBoxCitizencode_TextChanged(object sender, EventArgs e)
        {
            this.user.CitizenCode = gTextBoxCitizencode.Text;
            WinformControlUtil.RunCheckDataValid(user.CheckCitizenCode() || flagCheck, erpCitizenCode, gTextBoxCitizencode, "Citizen code is already exists or empty");
        }
        private void gDateTimePickerBirthday_ValueChanged(object sender, EventArgs e)
        {
            this.user.DateOfBirth = gDateTimePickerBirthday.Value;
            WinformControlUtil.RunCheckDataValid(user.CheckBirthday() || flagCheck, erpBirthday, gDateTimePickerBirthday, "Not yet 18 years old");
        }
        private void gComboBoxGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.user.Gender = (EUserGender)EnumUtil.ConvertStringToEnum(gComboBoxGender, typeof(EUserGender));
            WinformControlUtil.RunCheckDataValid(user.CheckGender() || flagCheck, erpGender, gComboBoxGender, "Gender cannot be empty");
        }
        private void gTextBoxEmail_TextChanged(object sender, EventArgs e)
        {
            this.user.Email = gTextBoxEmail.Text;
            WinformControlUtil.RunCheckDataValid(user.CheckEmail() || flagCheck, erpEmail, gTextBoxEmail, "Email address is already exists or invalid");
        }
        private void gTextBoxPhonenumber_TextChanged(object sender, EventArgs e)
        {
            this.user.PhoneNumber = gTextBoxPhonenumber.Text;
            WinformControlUtil.RunCheckDataValid(user.CheckPhoneNumber() || flagCheck, erpPhonenumber, gTextBoxPhonenumber, "Phone number is already exists or invalid");
        }
        private void gTextBoxHandle_TextChanged(object sender, EventArgs e)
        {
            this.user.UserName = gTextBoxUserName.Text;
            WinformControlUtil.RunCheckDataValid(user.CheckUserName() || flagCheck, erpHandle, gTextBoxUserName, "Username is already exists or invalid");
        }
        private void gTextBoxPassword_TextChanged(object sender, EventArgs e)
        {
            this.user.Password = gTextBoxPassword.Text;
            WinformControlUtil.RunCheckDataValid(user.CheckPassWord(gTextBoxConfirmPassword.Text) || flagCheck, erpPassword, gTextBoxPassword, "Confirmation password does not match");
            if (user.CheckPassWord(gTextBoxConfirmPassword.Text) || flagCheck) erpConfirmPassword.SetError(gTextBoxConfirmPassword, null);
        }
        private void gTextBoxConfirmPassword_TextChanged(object sender, EventArgs e)
        {
            WinformControlUtil.RunCheckDataValid(user.CheckPassWord(gTextBoxConfirmPassword.Text) || flagCheck, erpConfirmPassword, gTextBoxConfirmPassword, "Confirmation password does not match");
            if (user.CheckPassWord(gTextBoxConfirmPassword.Text) || flagCheck) erpPassword.SetError(gTextBoxPassword, null);
        }
        private void gTextBoxUniversity_TextChanged(object sender, EventArgs e)
        {
            this.user.University = gTextBoxUniversity.Text;
            WinformControlUtil.RunCheckDataValid(user.CheckUniversity() || flagCheck, erpUniversity, gTextBoxUniversity, "University cannot be empty");
        }
        private void gTextBoxFaculty_TextChanged(object sender, EventArgs e)
        {
            this.user.Faculty = gTextBoxFaculty.Text;
            WinformControlUtil.RunCheckDataValid(user.CheckFaculty() || flagCheck, erpFaculty, gTextBoxFaculty, "Faculty cannot be empty");
        }
        private void gTextBoxWorkcode_TextChanged(object sender, EventArgs e)
        {
            this.user.WorkCode = gTextBoxWorkcode.Text;
            WinformControlUtil.RunCheckDataValid(user.CheckWorkCode() || flagCheck, erpWorkCode, gTextBoxWorkcode, "Work code is already exists or invalid");
        }

        #endregion

        #region Radio Button Checked Changed

        private void gRadioButtonStudent_CheckedChanged(object sender, EventArgs e)
        {
            if (gRadioButtonStudent.Checked)
            {
                gTextBoxWorkcode.PlaceholderText = "Student code";
            }
            else
            {
                gTextBoxWorkcode.PlaceholderText = "Lecture code";
            }
        }

        private void gRadioButtonLecture_CheckedChanged(object sender, EventArgs e)
        {
            if (gRadioButtonLecture.Checked)
            {
                gTextBoxWorkcode.PlaceholderText = "Lecture code";
            }
            else
            {
                gTextBoxWorkcode.PlaceholderText = "Student code";
            }
        }

        #endregion

    }
}
