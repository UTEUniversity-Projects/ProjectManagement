using Guna.UI2.WinForms;
using System.Data;
using ProjectManagement.DAOs;
using ProjectManagement.Database;
using ProjectManagement.Models;
using ProjectManagement.Process;
using ProjectManagement.Enums;
using ProjectManagement.Utils;
using System.Data.SqlClient;

namespace ProjectManagement
{
    public partial class UCProjectCreate : UserControl
    {              

        private Users user = new Users();
        private Project project = new Project();

        private ProjectDAO ProjectDAO = new ProjectDAO();
        private NotificationDAO NotificationDAO = new NotificationDAO();

        private UCUserMiniLine uCUserMiniLine = new UCUserMiniLine();
        private bool flagCheck = false;
        private bool flagCreate = false;
        private bool flagEdit = false;
        private bool flagInitEdit = false;

        public UCProjectCreate()
        {
            InitializeComponent();
            flagCreate = true;
        }

        #region PROPERTIES

        public Guna2Button GButtonCancel
        {
            get { return gButtonCancel; }
        }

        #endregion

        #region FUNCTIONS

        public void SetCreateState(Users user)
        {
            this.user = user;
            InitCreateState();
        }
        public void SetEditState(Users user, Project project)
        {
            this.user = user;
            this.project = project;
            InitEditState();
        }
        private void InitUserControl()
        {
            EnumUtil.AddEnumsToComboBox(gComboBoxField, typeof(EField));
            EnumUtil.AddEnumsToComboBox(gComboBoxLevel, typeof(ELevel));
            uCUserMiniLine.GButtonAdd.Hide();
            uCUserMiniLine.SetSize(new Size(397, 60));
            
            cmbIDInstructor.Items.Clear();
            List<string> list = UserDAO.SelectListID(EUserRole.LECTURE);
            foreach (var item in list)
            {
                cmbIDInstructor.Items.Add(item);
            }
        }
        private void InitCreateState()
        {
            flagInitEdit = false;
            gTextBoxTechnology.Text = string.Empty;
            InitUserControl();
            gTextBoxTopic.Text = string.Empty;
            gComboBoxField.StartIndex = 0;
            gComboBoxLevel.StartIndex = 0;
            gComboBoxMembers.StartIndex = 0;
            gTextBoxDescription.Text = string.Empty;
            gTextBoxFunctions.Text = string.Empty;
            gTextBoxRequirements.Text = string.Empty;
            cmbIDInstructor.Text = string.Empty;
            flagCreate = true;
            flagEdit = false;
            gButtonCreateOrEdit.Text = "Create";
            cmbIDInstructor.Enabled = true;

            if (user.Role == EUserRole.LECTURE)
            {
                cmbIDInstructor.SelectedItem = user.UserId;
                cmbIDInstructor.Enabled = false;
            }
            cmbIDInstructor_SelectedIndexChanged(cmbIDInstructor, new EventArgs());
        }
        private void InitEditState()
        {
            flagInitEdit = true;
            InitUserControl();
            gTextBoxTopic.Text = project.Topic;
            gComboBoxField.SelectedItem = project.FieldId;
            // gComboBoxLevel.SelectedItem = project.Level;
            gComboBoxMembers.SelectedItem = project.MaxMember.ToString();
            gTextBoxDescription.Text = project.Description;
            gTextBoxFunctions.Text = project.Feature;
            gTextBoxRequirements.Text = project.Requirement;
            flagCreate = false;
            flagEdit = true;
            gButtonCreateOrEdit.Text = "Save";
            cmbIDInstructor.SelectedItem = project.InstructorId;
            cmbIDInstructor.Enabled = false;
            cmbIDInstructor_SelectedIndexChanged(cmbIDInstructor, new EventArgs());
        }
        private void SetComboBoxTechnology()
        {
            if (gComboBoxField.SelectedItem == null) return;

            string sqlStr = string.Format("SELECT T.technologyId, T.name " +
                "FROM {0} AS T JOIN (SELECT * FROM {1} WHERE {1}.fieldId = @FieldId) AS FT " +
                "ON T.technologyId = FT.technologyId", DBTableNames.Technology, DBTableNames.FieldTechnology);

            List<SqlParameter> parameters = new List<SqlParameter> 
            { 
                new SqlParameter("@FieldId", gComboBoxField.Text)
            };

            DataTable dt = DBExecution.ExecuteQuery(sqlStr, parameters);

            gComboBoxTechnology.Items.Clear();
            foreach (DataRow row in dt.Rows)
            {
                gComboBoxTechnology.Items.Add(row["name"].ToString());
            }
            gComboBoxTechnology.StartIndex = 0;
        }
        private bool CheckInformationValid()
        {
            WinformControlUtil.RunCheckDataValid(project.CheckTopic() || flagCheck, erpTopic, gTextBoxTopic, "Topic cannot be empty");
            WinformControlUtil.RunCheckDataValid(project.CheckDesription() || flagCheck, erpDescription, gTextBoxDescription, "Description cannot be empty");
            WinformControlUtil.RunCheckDataValid(project.CheckTechnology() || flagCheck, erpTechnology, gTextBoxTechnology, "Technologies cannot be empty");
            WinformControlUtil.RunCheckDataValid(project.CheckFeature() || flagCheck, erpFunctions, gTextBoxFunctions, "Feature cannot be empty");
            WinformControlUtil.RunCheckDataValid(project.CheckRequirement() || flagCheck, erpRequirements, gTextBoxRequirements, "Requirement cannot be empty");
            WinformControlUtil.RunCheckDataValid(project.CheckInstructorId() || flagCheck, erpInstructor, cmbIDInstructor, "Instructor cannot be empty");

            return project.CheckTopic() && project.CheckDesription() && project.CheckTechnology()
                    && project.CheckFeature() && project.CheckRequirement() && project.CheckInstructorId();
        }

        #endregion

        #region EVENT CREATE for CREATE

        private void SolveForCreate()
        {
            this.project = new Project(cmbIDInstructor.SelectedIndex != -1 ? cmbIDInstructor.SelectedItem.ToString() : string.Empty, 
                gTextBoxTopic.Text, gTextBoxDescription.Text, gTextBoxFunctions.Text, gTextBoxRequirements.Text,
                DataTypeUtil.ConvertStringToInt32(gComboBoxMembers.SelectedItem.ToString()),
                DateTime.Now, EProjectStatus.PUBLISHED, DateTime.Now, this.user.UserId, "xxx");

            this.flagCheck = false;
            if (CheckInformationValid())
            {
                ProjectDAO.Insert(project);
                if (project.CreatedBy != project.InstructorId)
                {
                    string content = Notification.GetContentTypeProject(user.FullName, project.Topic);
                    NotificationDAO.Insert(new Notification("xxx", content, Notification.GetNotificationType(project.ProjectId), DateTime.Now));
                }
                this.flagCheck = true;
                InitCreateState();
            }
        }
        private void SolveForEdit()
        {
            this.project = new Project(this.project.ProjectId, gTextBoxTopic.Text, gTextBoxDescription.Text,
                gTextBoxFunctions.Text, gTextBoxRequirements.Text, DataTypeUtil.ConvertStringToInt32(gComboBoxMembers.SelectedItem.ToString()),
                DateTime.Now, EProjectStatus.PUBLISHED, this.project.CreatedAt, this.project.CreatedBy, "xxx");

            this.flagCheck = false;
            if (CheckInformationValid())
            {
                ProjectDAO.Update(project);
                this.flagCheck = true;
                this.project = ProjectDAO.SelectOnly(project.ProjectId);
                gButtonCancel.PerformClick();
            }
        }
        private void gButtonCreateOrEdit_Click(object sender, EventArgs e)
        {
            if (flagCreate) SolveForCreate();
            if (flagEdit) SolveForEdit();
        }

        #endregion

        #region EVENT TextChanged

        private void gTextBoxTopic_TextChanged(object sender, EventArgs e)
        {
            project.Topic = gTextBoxTopic.Text;
            WinformControlUtil.RunCheckDataValid(project.CheckTopic() || flagCheck, erpTopic, gTextBoxTopic, "Topic cannot be empty");
        }
        private void gTextBoxDescription_TextChanged(object sender, EventArgs e)
        {
            project.Description = gTextBoxDescription.Text;
            WinformControlUtil.RunCheckDataValid(project.CheckDesription() || flagCheck, erpDescription, gTextBoxDescription, "Description cannot be empty");
        }
        private void gTextBoxTechnology_TextChanged(object sender, EventArgs e)
        {
            // project.Technology = gTextBoxTechnology.Text;
            WinformControlUtil.RunCheckDataValid(project.CheckTechnology() || flagCheck, erpTechnology, gTextBoxTechnology, "Technologies cannot be empty");
        }
        private void gTextBoxFunctions_TextChanged(object sender, EventArgs e)
        {
            project.Feature = gTextBoxFunctions.Text;
            WinformControlUtil.RunCheckDataValid(project.CheckFeature() || flagCheck, erpFunctions, gTextBoxFunctions, "Feature cannot be empty");
        }
        private void gTextBoxRequirements_TextChanged(object sender, EventArgs e)
        {
            project.Requirement = gTextBoxRequirements.Text;
            WinformControlUtil.RunCheckDataValid(project.CheckRequirement() || flagCheck, erpRequirements, gTextBoxRequirements, "Requirement cannot be empty");
        }

        #endregion

        #region EVENT cmbIDInstructor

        private void cmbIDInstructor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbIDInstructor.SelectedItem != null)
            {
                string idInstructor = cmbIDInstructor.SelectedItem.ToString();
                Users user = UserDAO.SelectOnlyByID(idInstructor);
                uCUserMiniLine.SetInformation(user);
                flpInstructor.Controls.Clear();
                flpInstructor.Controls.Add(uCUserMiniLine);
            }
            else
            {
                Label label = WinformControlUtil.CreateLabel("There aren't any user selected !");
                flpInstructor.Controls.Clear();
                flpInstructor.Controls.Add(label);
            }
        }

        #endregion

        #region EVENT gComboBoxField

        private void gComboBoxField_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetComboBoxTechnology();
        }

        #endregion

        #region EVENT gComboBoxTechnology

        private void gComboBoxTechnology_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (flagInitEdit)
            {
                // gTextBoxTechnology.Text = project.Technology;
                flagInitEdit = false;
                return;
            }
            if (gComboBoxTechnology.SelectedIndex != -1)
            {
                gTextBoxTechnology.Text += gComboBoxTechnology.SelectedItem + ", ";
            }
        }

        #endregion

        #region EVENT gButtonTechnologyClear

        private void gButtonTechnologyClear_Click(object sender, EventArgs e)
        {
            gTextBoxTechnology.Clear();
        }

        #endregion

    }
}
