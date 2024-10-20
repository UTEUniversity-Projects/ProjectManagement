using Guna.UI2.WinForms;
using System.Data;
using System.Globalization;
using ProjectManagement.DAOs;
using ProjectManagement.Models;
using ProjectManagement.Enums;
using ProjectManagement.Utils;

namespace ProjectManagement
{
    public partial class UCAccount : UserControl
    {
        private Users user = new Users();
        private Users dynamicUser = new Users();
        private int totalContributions = 0;

        private bool flagCheck = false;

        public UCAccount()
        {
            InitializeComponent();
        }

        #region FUNCTIONS FORM

        public void SetInformation(Users user)
        {
            this.user = user;
            this.dynamicUser = user.Clone();
            EnumUtil.AddEnumsToComboBox(gComboBoxGender, typeof(EUserGender));
            SetupComboBoxSelectYear();
            InitUserControl();
        }
        private void InitUserControl()
        {
            gCirclePictureBoxAvatar.Image = WinformControlUtil.NameToImage(user.Avatar);
            lblViewHandle.Text = user.UserName;
            lblViewRole.Text = EnumUtil.GetDisplayName(user.Role);

            gTextBoxFullname.Text = user.FullName;
            gTextBoxCitizencode.Text = user.CitizenCode;
            gDateTimePickerBirthday.Value = user.DateOfBirth;
            gComboBoxGender.SelectedItem = EnumUtil.GetDisplayName(user.Gender);
            gTextBoxEmail.Text = user.Email;
            gTextBoxPhonenumber.Text = user.PhoneNumber;
            gTextBoxUserName.Text = user.UserName;
            gTextBoxUniversity.Text = user.University;
            gTextBoxFaculty.Text = user.Faculty;
            gTextBoxWorkcode.Text = user.WorkCode;

            GunaControlUtil.SetTextBoxState(gTextBoxUniversity, true);
            GunaControlUtil.SetTextBoxState(gTextBoxFaculty, true);
            GunaControlUtil.SetTextBoxState(gTextBoxWorkcode, true);

            SetViewSate(true);
            SetTeamsList();

            UpdateBarChart();
            SetupTotalContributions();
        }
        private void SetViewSate(bool onlyView)
        {
            GunaControlUtil.SetTextBoxState(gTextBoxFullname, onlyView);
            GunaControlUtil.SetTextBoxState(gTextBoxCitizencode, onlyView);
            GunaControlUtil.SetTextBoxState(gTextBoxEmail, onlyView);
            GunaControlUtil.SetTextBoxState(gTextBoxPhonenumber, onlyView);
            GunaControlUtil.SetTextBoxState(gTextBoxUserName, onlyView);
            GunaControlUtil.SetDatePickerState(gDateTimePickerBirthday, onlyView);
            GunaControlUtil.SetComboBoxState(gComboBoxGender, onlyView);

            if (onlyView)
            {
                gButtonCancel.Hide();
                gGradientButtonSave.Hide();
                gPictureBoxGender.BackColor = Color.Gainsboro;
            }
            else
            {
                gButtonCancel.Show();
                gGradientButtonSave.Show();
                gPictureBoxGender.BackColor = Color.White;
            }
            gCirclePictureBoxAvatar.Focus();
        }
        private void SetTeamsList()
        {
            flpTeams.Controls.Clear();
            if (user.Role == EUserRole.LECTURE)
            {
                Guna2PictureBox pictureBox = GunaControlUtil.CreatePictureBox(Properties.Resources.PictureEmptyState, new Size(370, 325));
                flpTeams.Controls.Add(pictureBox);
                return;
            }

            List<Team> list = TeamDAO.SelectFollowUser(user.UserId);
            foreach (Team team in list)
            {
                UCTeamLine line = new UCTeamLine(team, user);
                line.SetSize(new Size(350, 60));
                line.SetBackColor(SystemColors.ButtonFace);
                line.SetSimpleLine();
                flpTeams.Controls.Add(line);
            }
        }
        public void RunCheckInformation()
        {
            WinformControlUtil.RunCheckDataValid(dynamicUser.CheckFullName() || flagCheck, erpFullName, gTextBoxFullname, "Name cannot be empty");
            WinformControlUtil.RunCheckDataValid(dynamicUser.CheckCitizenCode() || flagCheck || user.CitizenCode == dynamicUser.CitizenCode, erpCitizenCode, gTextBoxCitizencode, "Citizen code is already exists or empty");
            WinformControlUtil.RunCheckDataValid(dynamicUser.CheckBirthday() || flagCheck, erpBirthday, gDateTimePickerBirthday, "Not yet 18 years old");
            WinformControlUtil.RunCheckDataValid(dynamicUser.CheckGender() || flagCheck, erpGender, gComboBoxGender, "Gender cannot be empty");
            WinformControlUtil.RunCheckDataValid(dynamicUser.CheckEmail() || flagCheck || user.Email == dynamicUser.Email, erpEmail, gTextBoxEmail, "Email is already exists or invalid");
            WinformControlUtil.RunCheckDataValid(dynamicUser.CheckPhoneNumber() || flagCheck || user.PhoneNumber == dynamicUser.PhoneNumber, erpPhonenumber, gTextBoxPhonenumber, "Phone number is already exists or invalid");
            WinformControlUtil.RunCheckDataValid(dynamicUser.CheckUserName() || flagCheck || user.UserName == dynamicUser.UserName, erpHandle, gTextBoxUserName, "UserName is already exists or invalid");
        }
        private bool CheckInformationValid()
        {
            RunCheckInformation();

            return dynamicUser.CheckFullName() && dynamicUser.CheckBirthday() && dynamicUser.CheckGender()
                   && (dynamicUser.CheckCitizenCode() || user.CitizenCode == dynamicUser.CitizenCode)                   
                   && (dynamicUser.CheckEmail() || user.Email == dynamicUser.Email)
                   && (dynamicUser.CheckPhoneNumber() || user.PhoneNumber == dynamicUser.PhoneNumber)
                   && (dynamicUser.CheckUserName() || user.UserName == dynamicUser.UserName);
        }

        #endregion

        #region CONTROL CLICK

        private void gGradientButtonEdit_Click(object sender, EventArgs e)
        {
            SetViewSate(false);
        }
        private void gButtonCancel_Click(object sender, EventArgs e)
        {
            SetViewSate(true);
        }
        private void gGradientButtonSave_Click(object sender, EventArgs e)
        {
            this.dynamicUser = new Users(dynamicUser.UserId, gTextBoxUserName.Text, gTextBoxFullname.Text, dynamicUser.Password, gTextBoxEmail.Text,
                gTextBoxPhonenumber.Text, gDateTimePickerBirthday.Value, gTextBoxCitizencode.Text, gTextBoxUniversity.Text, gTextBoxFaculty.Text, gTextBoxWorkcode.Text,
                EnumUtil.GetEnumFromDisplayName<EUserGender>(gComboBoxGender.SelectedItem.ToString()), dynamicUser.Avatar, dynamicUser.Role, dynamicUser.JoinAt);

            this.flagCheck = false;
            if (CheckInformationValid())
            {
                this.user = dynamicUser.Clone();
                UserDAO.Update(this.user);
                this.flagCheck = true;
                SetViewSate(true);
                lblViewHandle.Text = this.user.UserName;
            }
        }

        #endregion

        #region EVENT TextChanged and ValueChanged

        private void gTextBoxFullname_TextChanged(object sender, EventArgs e)
        {
            this.dynamicUser.FullName = gTextBoxFullname.Text;
            WinformControlUtil.RunCheckDataValid(dynamicUser.CheckFullName() || flagCheck, erpFullName, gTextBoxFullname, "Name cannot be empty");
        }
        private void gTextBoxCitizencode_TextChanged(object sender, EventArgs e)
        {
            this.dynamicUser.CitizenCode = gTextBoxCitizencode.Text;
            WinformControlUtil.RunCheckDataValid(dynamicUser.CheckCitizenCode() || flagCheck || user.CitizenCode == dynamicUser.CitizenCode,
                erpCitizenCode, gTextBoxCitizencode, "Citizen code is already exists or empty");
        }
        private void gDateTimePickerBirthday_ValueChanged(object sender, EventArgs e)
        {
            this.dynamicUser.DateOfBirth = gDateTimePickerBirthday.Value;
            WinformControlUtil.RunCheckDataValid(dynamicUser.CheckBirthday() || flagCheck, erpBirthday, gDateTimePickerBirthday, "Not yet 18 years old");
        }
        private void gComboBoxGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dynamicUser.Gender = (EUserGender)EnumUtil.ConvertStringToEnum(gComboBoxGender, typeof(EUserGender));
            WinformControlUtil.RunCheckDataValid(dynamicUser.CheckGender() || flagCheck, erpGender, gComboBoxGender, "Gender cannot be empty");
        }
        private void gTextBoxEmail_TextChanged(object sender, EventArgs e)
        {
            this.dynamicUser.Email = gTextBoxEmail.Text;
            WinformControlUtil.RunCheckDataValid(dynamicUser.CheckEmail() || flagCheck || user.Email == dynamicUser.Email,
                erpEmail, gTextBoxEmail, "Email is already exists or invalid");
        }
        private void gTextBoxPhonenumber_TextChanged(object sender, EventArgs e)
        {
            this.dynamicUser.PhoneNumber = gTextBoxPhonenumber.Text;
            WinformControlUtil.RunCheckDataValid(dynamicUser.CheckPhoneNumber() || flagCheck || user.PhoneNumber == dynamicUser.PhoneNumber,
                erpPhonenumber, gTextBoxPhonenumber, "Phone number is already exists or invalid");
        }
        private void gTextBoxHandle_TextChanged(object sender, EventArgs e)
        {
            this.dynamicUser.UserName = gTextBoxUserName.Text;
            WinformControlUtil.RunCheckDataValid(dynamicUser.CheckUserName() || flagCheck || user.UserName == dynamicUser.UserName,
                erpHandle, gTextBoxUserName, "UserName is already exists or invalid");
        }

        #endregion

        #region COMBO BOX
        public void SetupComboBoxSelectYear()
        {
            int currentYear = DateTime.Now.Year;
            List<int> recentYears = new List<int> { currentYear, currentYear - 1, currentYear - 2 };

            this.gComboBoxSelectYear.DataSource = recentYears;
            this.gComboBoxSelectYear.SelectedIndex = 0;
        }
        private void gComboBoxSelectYear_SelectedValueChanged(object sender, EventArgs e)
        {
            UpdateBarChart();
            SetupTotalContributions();
        }
        #endregion

        #region LABEL TOTAL CONTRIBUTIONS

        void SetupTotalContributions()
        {
            string textContribution = this.totalContributions > 1 ? "contributions" : "contribution";
            int year = (int)this.gComboBoxSelectYear.SelectedItem;
            this.lblTotalContributions.Text = $"{this.totalContributions} {textContribution} in {year}";
        }

        #endregion

        #region MIXED BAR AND SPLINE CHART
        IEnumerable<object> GetContributionForTeacher(int selectedYear)
        {
            List<Project> listTheses = ProjectDAO.SelectListRoleLecture(this.user.UserId)
                                     .Where(project => project.CreatedAt.Year == selectedYear)
                                     .ToList();

            this.totalContributions = listTheses.Count;
            var allMonths = Enumerable.Range(1, 12);
            var contributions = allMonths
            .GroupJoin(listTheses,
                       month => month,
                       project => project.CreatedAt.Month,
                       (month, evaluation) => new
                       {
                           Month = month,
                           Count = evaluation.Count(),
                       })
            .Select(result => new
            {
                result.Month,
                result.Count
            });
            return contributions;
        }

        IEnumerable<object> GetContributionForStudent(int selectedYear)
        {
            List<Evaluation> listEvaluations = EvaluationDAO.SelectListByUser(this.user.UserId);
            this.totalContributions = listEvaluations.Count;
            var allMonths = Enumerable.Range(1, 12);
            var contributions = allMonths
            .GroupJoin(listEvaluations,
                       month => month,
                       evaluation => evaluation.CreatedAt.Month,
                       (month, evaluation) => new
                       {
                           Month = month,
                           Count = evaluation.Where(project => project.CreatedAt.Year == selectedYear).Count()
                       })
            .Select(result => new
            {
                result.Month,
                result.Count
            });
            return contributions;
        }

        public void UpdateBarChart()
        {
            int selectedYear = (int)gComboBoxSelectYear.SelectedItem;

            var contributions = user.Role == EUserRole.LECTURE ? GetContributionForTeacher(selectedYear) : GetContributionForStudent(selectedYear);

            CultureInfo culture = CultureInfo.InvariantCulture;
            DateTimeFormatInfo dtfi = culture.DateTimeFormat;
            string monthName;
            this.gBarDataset.DataPoints.Clear();
            this.gBarChart.Datasets.Clear();
            foreach (var group in contributions)
            {
                var month = (int)group.GetType().GetProperty("Month").GetValue(group, null);
                var count = (int)group.GetType().GetProperty("Count").GetValue(group, null);

                monthName = dtfi.GetMonthName(month);
                this.gBarDataset.DataPoints.Add(monthName, count);
            }
            this.gBarChart.Datasets.Add(gBarDataset);
            this.gBarChart.Update();
        }
        #endregion
        
    }
}
