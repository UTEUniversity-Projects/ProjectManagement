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

        public bool Edited
        {
            get { return this.edited; }
        }

        public void SetUpUserControl(Users host, Users instructor, Project project, TaskMeta taskMeta, Users creator, Team team, bool isProcessing)
        {
            this.host = host;
            this.instructor = instructor;
            this.project = project;
            this.taskMeta = taskMeta;
            this.creator = creator;
            this.team = team;
            this.dynamicTask = taskMeta.Task;
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
            gDateTimePickerStart.Format = DateTimePickerFormat.Custom;
            gDateTimePickerStart.CustomFormat = "dd/MM/yyyy HH:mm:ss tt";

            gDateTimePickerEnd.Value = taskMeta.Task.EndAt;
            gDateTimePickerEnd.Format = DateTimePickerFormat.Custom;
            gDateTimePickerEnd.CustomFormat = "dd/MM/yyyy HH:mm:ss tt";

            gCirclePictureBoxCreator.Image = WinformControlUtil.NameToImage(creator.Avatar);
            GunaControlUtil.SetItemFavorite(gButtonStar, taskMeta.IsFavorite);

            if (!isProcessing || (host.Role == EUserRole.STUDENT && taskMeta.Task.CreatedBy != host.UserId))
            {
                gButtonEdit.Hide();
                gButtonStar.Location = new Point(610, 17);
            }

            SetViewState();
        }

        private void gButtonEdit_Click(object sender, EventArgs e)
        {
            SetEditState();
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
            gDateTimePickerStart.Enabled = false;
            gDateTimePickerEnd.Enabled = false;
            gComboBoxPriority.Enabled = false;
            GunaControlUtil.SetTextBoxState(new List<Guna2TextBox> { gTextBoxTitle, gTextBoxDescription, gTextBoxProgress }, true);
        }
        private void SetEditState()
        {
            gButtonCancel.Show();
            gButtonSave.Show();
            gDateTimePickerStart.Enabled = true;
            gDateTimePickerEnd.Enabled = true;
            gComboBoxPriority.Enabled = true;
            GunaControlUtil.SetTextBoxState(new List<Guna2TextBox> { gTextBoxTitle, gTextBoxDescription, gTextBoxProgress }, false);
        }
        private bool CheckInformationValid()
        {
            WinformControlUtil.RunCheckDataValid(dynamicTask.CheckTitle() || flagCheck, erpTitle, gTextBoxTitle, "Title cannot be empty");
            WinformControlUtil.RunCheckDataValid(dynamicTask.CheckDescription() || flagCheck, erpDescription, gTextBoxDescription, "Description cannot be empty");
            WinformControlUtil.RunCheckDataValid(dynamicTask.CheckProgress() || flagCheck, erpProgress, gTextBoxProgress, "Can only take values from 0 to 100");
            WinformControlUtil.RunCheckDataValid(dynamicTask.CheckEnd() || flagCheck, erpEnd, gDateTimePickerEnd, "The end time must be after the start time");

            return dynamicTask.CheckTitle() && dynamicTask.CheckDescription() && dynamicTask.CheckProgress() && dynamicTask.CheckEnd();
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
            this.dynamicTask.Progress = progress;
            gProgressBarToLine.Value = progress;
            WinformControlUtil.RunCheckDataValid(this.dynamicTask.CheckProgress() || flagCheck, erpProgress, gTextBoxProgress, "Can only take values from 0 to 100");
        }
        private void gDateTimePickerEnd_ValueChanged(object sender, EventArgs e)
        {
            this.dynamicTask.EndAt = gDateTimePickerEnd.Value;
            WinformControlUtil.RunCheckDataValid(dynamicTask.CheckEnd() || flagCheck, erpEnd, gDateTimePickerEnd, "The end time must be after the start time");
        }
        private void gComboBoxPriority_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gComboBoxPriority.SelectedItem != null &&
            Enum.TryParse<ETaskPriority>(gComboBoxPriority.SelectedItem.ToString(), out var priority))
            {
                this.dynamicTask.Priority = priority;
            }

        }
    }
}
