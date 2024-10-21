using Guna.UI2.WinForms;
using System.Data;
using ProjectManagement.DAOs;
using ProjectManagement.Database;
using ProjectManagement.Models;
using ProjectManagement.Process;
using ProjectManagement.Enums;
using ProjectManagement.Utils;
using System.Data.SqlClient;
using ProjectManagement.UserControls;

namespace ProjectManagement
{
    public partial class UCProjectCreate : UserControl
    {

        private Users user = new Users();
        private Project project = new Project();
        private List<Field> fields = new List<Field>();
        private List<Technology> technologies = new List<Technology>();

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
            this.fields = FieldDAO.SelectList();
            GunaControlUtil.SetComboBoxDisplayAndValue(gComboBoxField, fields, "Name", "FieldId");
            uCUserMiniLine.GButtonAdd.Hide();
            uCUserMiniLine.SetSize(new Size(397, 60));

            cmbIDInstructor.Items.Clear();
            List<string> list = UserDAO.SelectListId(EUserRole.LECTURE);
            foreach (var item in list)
            {
                cmbIDInstructor.Items.Add(item);
            }
        }
        private void InitCreateState()
        {
            flagInitEdit = false;
            InitUserControl();
            gTextBoxTopic.Text = string.Empty;
            gComboBoxField.StartIndex = 0;
            gComboBoxMembers.StartIndex = 0;
            gTextBoxDescription.Text = string.Empty;
            gTextBoxFunctions.Text = string.Empty;
            gTextBoxRequirements.Text = string.Empty;
            this.technologies.Clear();
            flpTechnologyList.Controls.Clear();
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
            gComboBoxTechnology_SelectedIndexChanged(gComboBoxTechnology, new EventArgs());
        }
        private void InitEditState()
        {
            flagInitEdit = true;
            InitUserControl();
            gTextBoxTopic.Text = project.Topic;
            gComboBoxField.SelectedValue = project.FieldId;
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

            this.technologies.Clear();
            flpTechnologyList.Controls.Clear();
            this.technologies = TechnologyDAO.SelectListByProject(project.ProjectId);
            
            foreach (Technology technology in technologies)
            {
                UCTechnologyLine line = new UCTechnologyLine(technology);
                line.TechnologyRemove += GButtonRemove_Click;
                flpTechnologyList.Controls.Add(line);
            }
        }
        private void SetComboBoxTechnology()
        {
            if (gComboBoxField.SelectedItem == null) return;

            Field field = (Field)gComboBoxField.SelectedItem;
            List<Technology> fieldTechnologies = TechnologyDAO.SelectListByField(field.FieldId);
            GunaControlUtil.SetComboBoxDisplayAndValue(gComboBoxTechnology, fieldTechnologies, "Name", "TechnologyId");
            gComboBoxTechnology.StartIndex = 0;
        }
        private bool CheckInformationValid()
        {
            WinformControlUtil.RunCheckDataValid(project.CheckTopic() || flagCheck, erpTopic, gTextBoxTopic, "Topic cannot be empty");
            WinformControlUtil.RunCheckDataValid(project.CheckDescription() || flagCheck, erpDescription, gTextBoxDescription, "Description cannot be empty");
            WinformControlUtil.RunCheckDataValid(this.technologies.Count > 0 || flagCheck, erpTechnology, flpTechnologyList, "Technologies cannot be empty");
            WinformControlUtil.RunCheckDataValid(project.CheckFeature() || flagCheck, erpFunctions, gTextBoxFunctions, "Feature cannot be empty");
            WinformControlUtil.RunCheckDataValid(project.CheckRequirement() || flagCheck, erpRequirements, gTextBoxRequirements, "Requirement cannot be empty");
            WinformControlUtil.RunCheckDataValid(project.CheckInstructorId() || flagCheck, erpInstructor, cmbIDInstructor, "Instructor cannot be empty");

            return project.CheckTopic() && project.CheckDescription() && this.technologies.Count > 0
                    && project.CheckFeature() && project.CheckRequirement() && project.CheckInstructorId();
        }

        #endregion

        #region EVENT CREATE for CREATE

        private void SolveForCreate()
        {
            this.project = new Project(cmbIDInstructor.SelectedIndex != -1 ? cmbIDInstructor.SelectedItem.ToString() : string.Empty,
                gTextBoxTopic.Text, gTextBoxDescription.Text, gTextBoxFunctions.Text, gTextBoxRequirements.Text,
                DataTypeUtil.ConvertStringToInt32(gComboBoxMembers.SelectedItem.ToString()),
                EProjectStatus.PUBLISHED, DateTime.Now, this.user.UserId, ((Field)gComboBoxField.SelectedItem).FieldId);

            this.flagCheck = false;
            if (CheckInformationValid())
            {
                ProjectDAO.Insert(this.project, this.technologies);
                if (project.CreatedBy != project.InstructorId)
                {
                    string content = Notification.GetContentTypeProject(user.FullName, project.Topic);
                    Notification notification = new Notification("New Project", content, Notification.GetNotificationType(project.ProjectId), DateTime.Now);
                    NotificationDAO.Insert(notification, project.InstructorId);
                }
                this.flagCheck = true;
                InitCreateState();
            }
        }
        private void SolveForEdit()
        {
            this.project.Topic = gTextBoxTopic.Text;
            this.project.Description = gTextBoxDescription.Text;
            this.project.Feature = gTextBoxFunctions.Text;
            this.project.Requirement = gTextBoxRequirements.Text;
            this.project.MaxMember = DataTypeUtil.ConvertStringToInt32(gComboBoxMembers.SelectedItem.ToString());
            this.project.FieldId = ((Field)gComboBoxField.SelectedItem).FieldId;

            this.flagCheck = false;
            if (CheckInformationValid())
            {
                ProjectDAO.Update(this.project, this.technologies);
                this.flagCheck = true;
                this.project = ProjectDAO.SelectOnly(project.ProjectId);
                gButtonCancel.PerformClick();
            }
        }
        private void gButtonCreateOrEdit_Click(object sender, EventArgs e)
        {
            if (flagCreate) SolveForCreate();
            else if (flagEdit) SolveForEdit();
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
            WinformControlUtil.RunCheckDataValid(project.CheckDescription() || flagCheck, erpDescription, gTextBoxDescription, "Description cannot be empty");
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
        private void flpTechnologyList_Paint(object sender, PaintEventArgs e)
        {
            WinformControlUtil.RunCheckDataValid(this.technologies.Count > 0 || flagCheck, erpTechnology, flpTechnologyList, "Technologies cannot be empty");
        }

        #endregion

        #region EVENT cmbIDInstructor

        private void cmbIDInstructor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbIDInstructor.SelectedIndex >= 0)
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

            WinformControlUtil.RunCheckDataValid(cmbIDInstructor.SelectedIndex >= 0 || flagCheck, erpInstructor, cmbIDInstructor, "You must select instructor");
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
            Technology technology = (Technology)gComboBoxTechnology.SelectedItem;
            if (flagInitEdit)
            {
                flpTechnologyList.Controls.Clear();
                flagInitEdit = false;
                return;
            }
            if (gComboBoxTechnology.SelectedIndex != -1)
            {
                UCTechnologyLine line = new UCTechnologyLine(technology);
                line.TechnologyRemove += GButtonRemove_Click;
                this.technologies.Add(technology);
                flpTechnologyList.Controls.Add(line);
            }
        }
        private void GButtonRemove_Click(object sender, EventArgs e)
        {
            UCTechnologyLine line = sender as UCTechnologyLine;

            if (line != null)
            {
                foreach (Control control in flpTechnologyList.Controls)
                {
                    if (control.GetType() == typeof(UCTechnologyLine))
                    {
                        UCTechnologyLine techLine = (UCTechnologyLine)control;
                        if (techLine == line)
                        {
                            flpTechnologyList.Controls.Remove(control);
                            this.technologies.Remove(line.GetTechnology);
                            control.Dispose();
                            break;
                        }
                    }
                }
            }
        }

        #endregion

        #region EVENT gButtonTechnologyClear

        private void gButtonTechnologyClear_Click(object sender, EventArgs e)
        {
            flpTechnologyList.Controls.Clear();
            this.technologies.Clear();
        }

        #endregion

    }
}
