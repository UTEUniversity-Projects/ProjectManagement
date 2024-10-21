using Guna.UI2.WinForms;
using ProjectManagement.DAOs;
using ProjectManagement.Enums;
using ProjectManagement.MetaData;
using ProjectManagement.Models;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectManagement.UserControls
{
    public partial class UCTaskDetails : UserControl
    {
        private Users host = new Users();
        private Users creator = new Users();
        private Users instructor = new Users();
        private Project project = new Project();
        private TaskMeta taskMeta = new TaskMeta();
        private Team team = new Team();
        private Tasks dynamicTask = new Tasks();

        private bool isProcessing = true;
        private bool flagCheck = false;
        private bool edited = false;
        public UCTaskDetails()
        {
            InitializeComponent();
        }

        public void SetUpUserControl(Users host, Users instructor, Project project, TaskMeta taskMeta, Users creator, Team team, bool isProcessing)
        {
            this.host = host;
            this.instructor = instructor;
            this.project = project;
            this.taskMeta = taskMeta;
            this.creator = creator;
            this.team = team;
            this.isProcessing = isProcessing;
            InitUserControl();
        }
        private void InitUserControl()
        {
            lblCreator.Text = creator.FullName;
            gTextBoxTitle.Text = taskMeta.Task.Title;
            gTextBoxDescription.Text = taskMeta.Task.Description;
            gTextBoxProgress.Text = taskMeta.Task.Progress.ToString();
            gProgressBarToLine.Value = int.Parse(taskMeta.Task.Progress.ToString());
            EnumUtil.AddEnumsToComboBox(gComboBoxPriority, typeof(ETaskPriority));

            if (taskMeta.Task.Priority == ETaskPriority.HIGH)
            {
                gComboBoxPriority.SelectedIndex = 0;
            }
            else if (taskMeta.Task.Priority == ETaskPriority.MEDIUM)
            {
                gComboBoxPriority.SelectedIndex = 1;
            }
            else
            {
                gComboBoxPriority.SelectedIndex = 2;
            }

            gDateTimePickerStart.Value = taskMeta.Task.StartAt;
            gDateTimePickerEnd.Value = taskMeta.Task.EndAt;

            gCirclePictureBoxCreator.Image = WinformControlUtil.NameToImage(creator.Avatar);
            GunaControlUtil.SetItemFavorite(gButtonStar, taskMeta.IsFavorite);

            if (!isProcessing || (host.Role == EUserRole.STUDENT && taskMeta.Task.CreatedBy != host.UserId))
            {
                gButtonEdit.Hide();
                gButtonStar.Location = new Point(383, 17);
            }

            SetViewState();
        }

        private void gButtonEdit_Click(object sender, EventArgs e)
        {
            SetEditState();
        }
        private void gTextBoxTitle_TextChanged(object sender, EventArgs e)
        {
            this.dynamicTask.Title = gTextBoxTitle.Text;
            WinformControlUtil.RunCheckDataValid(dynamicTask.CheckTitle() || flagCheck, erpTitle, gTextBoxTitle, "Title cannot be empty");
        }
        private void gTextBoxDescription_TextChanged(object sender, EventArgs e)
        {
            this.dynamicTask.Description = gTextBoxDescription.Text;
            WinformControlUtil.RunCheckDataValid(dynamicTask.CheckDescription() || flagCheck, erpDescription, gTextBoxDescription, "Description cannot be empty");
        }
        private void gTextBoxProgress_TextChanged(object sender, EventArgs e)
        {
            int progress = 0;
            bool checkProgress = int.TryParse(gTextBoxProgress.Text, out progress);
            if (checkProgress)
            {
                taskMeta.Task.Progress = progress;
                gProgressBarToLine.Value = progress;
            }
            WinformControlUtil.RunCheckDataValid((checkProgress && taskMeta.Task.CheckProgress()) || flagCheck, erpProgress, gTextBoxProgress, "Can only take values from 0 to 100");
        }
        private void gButtonCancel_Click(object sender, EventArgs e)
        {
            gTextBoxTitle.Text = taskMeta.Task.Title;
            gTextBoxDescription.Text = taskMeta.Task.Description;
            gTextBoxProgress.Text = taskMeta.Task.Progress.ToString();
            SetViewState();
        }
        private void gButtonSave_Click(object sender, EventArgs e)
        {
            this.flagCheck = false;
            if (CheckInformationValid())
            {
                this.taskMeta.Task = new Tasks(taskMeta.Task.TaskId, gDateTimePickerStart.Value, gDateTimePickerEnd.Value, gTextBoxTitle.Text, gTextBoxDescription.Text, double.Parse(gTextBoxProgress.Text), EnumUtil.GetEnumFromDisplayName<ETaskPriority>(gComboBoxPriority.SelectedItem.ToString()), taskMeta.Task.CreatedAt, this.creator.UserId, this.project.ProjectId);

                TaskDAO.Update(taskMeta.Task);
                this.flagCheck = true;
                this.edited = true;
                SetViewState();
            }
        }
        private void SetViewState()
        {
            gButtonCancel.Hide();
            gButtonSave.Hide();
            GunaControlUtil.SetTextBoxState(new List<Guna2TextBox> { gTextBoxTitle, gTextBoxDescription, gTextBoxProgress }, true);
        }
        private void SetEditState()
        {
            gButtonCancel.Show();
            gButtonSave.Show();
            GunaControlUtil.SetTextBoxState(new List<Guna2TextBox> { gTextBoxTitle, gTextBoxDescription, gTextBoxProgress }, false);
        }
        private bool CheckInformationValid()
        {
            WinformControlUtil.RunCheckDataValid(taskMeta.Task.CheckTitle() || flagCheck, erpTitle, gTextBoxTitle, "Title cannot be empty");
            WinformControlUtil.RunCheckDataValid(taskMeta.Task.CheckDescription() || flagCheck, erpDescription, gTextBoxDescription, "Description cannot be empty");
            int progress = 0;
            bool checkProgress = int.TryParse(gTextBoxProgress.Text, out progress);
            if (checkProgress) taskMeta.Task.Progress = progress;
            WinformControlUtil.RunCheckDataValid((checkProgress && taskMeta.Task.CheckProgress()) || flagCheck, erpProgress, gTextBoxProgress, "Can only take values from 0 to 100");

            return taskMeta.Task.CheckTitle() && taskMeta.Task.CheckDescription() && (checkProgress && taskMeta.Task.CheckProgress());
        }
    }
}
