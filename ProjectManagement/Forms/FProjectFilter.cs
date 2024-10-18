using Guna.UI2.WinForms;
using ProjectManagement.DAOs;
using ProjectManagement.Database;
using ProjectManagement.Models;
using ProjectManagement.Process;
using ProjectManagement.Enums;
using ProjectManagement.Utils;
using ProjectManagement.Mappers.Implement;
using System.Data.SqlClient;

namespace ProjectManagement
{
    public partial class FProjectFilter : Form
    {
        
        private Users user = new Users();
        private List<Project> listProject = new List<Project>();
        private List<Field> fields = new List<Field>();

        private UCUserMiniLine uCCreatorLine = new UCUserMiniLine();
        private UCUserMiniLine uCInstructorLine = new UCUserMiniLine();

        private bool flagAllTopic = true;
        private bool flagAllStatus = true;
        private bool flagAllFavorite = true;
        private bool flagFavorite = false;

        public FProjectFilter()
        {
            InitializeComponent();
            InitUserControl();
        }

        #region PROPERTIES

        public Guna2Button GButtonFilter
        {
            get { return this.gButtonFilter; }
        }
        public Guna2GradientButton GButtonSave
        {
            get { return this.gGradientButtonSave; }
        }
        public List<Project> ListProject
        {
            get { return this.listProject; }
            set { this.listProject = value; }
        }

        #endregion

        #region FUNCTIONS

        public void SetUpFilter(Users user)
        {
            this.user = user;
            this.fields = FieldDAO.SelectList();
            InitUserControl();
        }
        private void InitUserControl()
        {
            GunaControlUtil.SetComboBoxDisplayAndValue(gComboBoxField, fields, "Name", "FieldId");
            EnumUtil.AddEnumsToComboBox(gComboBoxStatus, typeof(EProjectStatus));
            flagAllTopic = false;
            gButtonTopicSelectAll.PerformClick();
            flagAllStatus = false;
            gButtonStatusSelectAll.PerformClick();
            flagAllFavorite = false;
            gButtonFavoriteSelectAll.PerformClick();
            uCCreatorLine.GButtonAdd.Hide();
            uCCreatorLine.SetSize(new Size(270, 60));
            uCInstructorLine.GButtonAdd.Hide();
            uCInstructorLine.SetSize(new Size(270, 60));
            gButtonFilter.Hide();

            List<string> list = new List<string>();

            cmbIDInstructor.Items.Clear();
            list.AddRange(UserDAO.SelectListId(EUserRole.LECTURE));
            foreach (var item in list) cmbIDInstructor.Items.Add(item);

            cmbIDCreator.Items.Clear();
            list.AddRange(UserDAO.SelectListId(EUserRole.STUDENT));
            foreach (var item in list) cmbIDCreator.Items.Add(item);

            cmbIDCreator.Text = string.Empty;
            cmbIDInstructor.Text = string.Empty;

            if (user.Role == EUserRole.LECTURE)
            {
                cmbIDInstructor.SelectedItem = user.UserId;
                cmbIDInstructor.Enabled = false;
            }
            cmbIDCreator_SelectedIndexChanged(cmbIDCreator, new EventArgs());
            cmbIDInstructor_SelectedIndexChanged(cmbIDInstructor, new EventArgs());
        }
        private void SetTextBoxEnable(List<Control> list, bool flag)
        {
            foreach (Control control in list)
            {
                control.Enabled = flag;
            }
        }
        private void ScanTextBoxOffOn(List<Control> list, ref bool flag, Guna2Button button, List<PictureBox> pictures)
        {
            flag = !flag;

            if (!flag)
            {
                SetTextBoxEnable(list, true);

                button.Image = Properties.Resources.PicItemOff;
                foreach (PictureBox picture in pictures)
                {
                    picture.BackColor = Color.White;
                }
            }
            else
            {
                SetTextBoxEnable(list, false);

                button.Image = Properties.Resources.PicItemOn;
                foreach (PictureBox picture in pictures)
                {
                    picture.BackColor = SystemColors.ControlLight;
                }
            }
        }

        #endregion

        #region EVENT FORM

        private void gButtonTopicStar_Click(object sender, EventArgs e)
        {
            flagFavorite = !flagFavorite;
            if (flagFavorite)
            {
                gButtonTopicStar.Image = Properties.Resources.PicInLineGradientStar;
            }
            else
            {
                gButtonTopicStar.Image = Properties.Resources.PicInLineStar;
            }
        }
        private void gButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void gGradientButtonSave_Click(object sender, EventArgs e)
        {
            string sqlStr = string.Format("SELECT P.* FROM {0} P ", DBTableNames.Project);
            List<SqlParameter> parameters = new List<SqlParameter>();
            int condition = 0;

            if (!flagAllTopic)
            {
                condition++;
                sqlStr += " WHERE P.fieldId = @FieldId AND P.maxMember BETWEEN @MembersFrom AND @MembersTo";
                parameters.Add(new SqlParameter("@FieldId", gComboBoxField.SelectedItem.ToString()));
                parameters.Add(new SqlParameter("@MembersFrom", gTextBoxMembersFrom.Text));
                parameters.Add(new SqlParameter("@MembersTo", gTextBoxMembersTo.Text));
            }

            if (!flagAllStatus)
            {
                condition++;
                sqlStr += (condition == 1) ? " WHERE " : " AND ";
                sqlStr += " P.status = @Status ";
                parameters.Add(new SqlParameter("@Status", gComboBoxStatus.SelectedItem.ToString()));
            }

            if (!flagAllFavorite)
            {
                condition++;
                sqlStr += (condition == 1) ? " WHERE " : " AND ";
                sqlStr += " EXISTS (SELECT 1 FROM FavouriteProject FP WHERE FP.projectId = P.projectId AND FP.userId = @UserId) ";
                parameters.Add(new SqlParameter("@UserId", user.UserId));
            }

            if (cmbIDCreator.SelectedIndex != -1)
            {
                condition++;
                sqlStr += (condition == 1) ? " WHERE " : " AND ";
                sqlStr += " P.createdBy = @CreatedBy ";
                parameters.Add(new SqlParameter("@CreatedBy", cmbIDCreator.SelectedItem.ToString()));
            }

            if (cmbIDInstructor.SelectedIndex != -1)
            {
                condition++;
                sqlStr += (condition == 1) ? " WHERE " : " AND ";
                sqlStr += " P.instructorId = @InstructorId ";
                parameters.Add(new SqlParameter("@InstructorId", cmbIDInstructor.SelectedItem.ToString()));
            }

            this.listProject = DBGetModel.GetModelList(sqlStr, parameters, new ProjectMapper());

            this.Close();
            gButtonFilter.PerformClick();
        }

        #endregion

        #region EVENT SELECT ALL CLICK

        private void gButtonTopicSelectAll_Click(object sender, EventArgs e)
        {
            ScanTextBoxOffOn(new List<Control> { pnlTopic }, ref flagAllTopic,
                                gButtonTopicSelectAll, new List<PictureBox> { gPictureBoxFaculty });
        }
        private void gButtonStatusSelectAll_Click(object sender, EventArgs e)
        {
            ScanTextBoxOffOn(new List<Control> { pnlStatus }, ref flagAllStatus,
                                gButtonStatusSelectAll, new List<PictureBox>());
        }

        private void gButtonFavoriteSelectAll_Click(object sender, EventArgs e)
        {
            ScanTextBoxOffOn(new List<Control> { pnlFavorite }, ref flagAllFavorite,
                                gButtonFavoriteSelectAll, new List<PictureBox>());
        }

        #endregion

        #region EVENT cmbIDInstructor

        private void GetUCUserLineByID(ComboBox comboBox, FlowLayoutPanel flowLayoutPanel, UCUserMiniLine uCUserMiniLine)
        {
            if (comboBox.SelectedItem != null)
            {
                string idInstructor = comboBox.SelectedItem.ToString();
                Users user = UserDAO.SelectOnlyByID(idInstructor);
                uCUserMiniLine.SetInformation(user);
                flowLayoutPanel.Controls.Clear();
                flowLayoutPanel.Controls.Add(uCUserMiniLine);
            }
            else
            {
                Label label = WinformControlUtil.CreateLabel("There aren't any user selected !");
                flowLayoutPanel.Controls.Clear();
                flowLayoutPanel.Controls.Add(label);
            }
        }
        private void cmbIDCreator_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetUCUserLineByID(cmbIDCreator, flpCreator, uCCreatorLine);
        }
        private void cmbIDInstructor_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetUCUserLineByID(cmbIDInstructor, flpInstructor, uCInstructorLine);
        }

        #endregion

        #region EVENT gButtonResetFilter

        private void gButtonResetFilter_Click(object sender, EventArgs e)
        {
            InitUserControl();
        }

        #endregion

    }
}
