using Guna.UI2.WinForms;
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
using ProjectManagement.Forms;
using ProjectManagement.Models;
using ProjectManagement.MetaData;
using ProjectManagement.Enums;
using ProjectManagement.Utils;

namespace ProjectManagement
{
    public partial class UCTaskMiniLine : UserControl
    {
        
        public event EventHandler TasksDeleteClicked;

        private Users creator = new Users();
        private Users instructor = new Users();
        private Project project = new Project();
        private TaskMeta taskMeta = new TaskMeta();
        private Team team = new Team();
        private Users host = new Users();

        private bool isProcessing = false;

        public UCTaskMiniLine(Users host, Users instructor, Project project, TaskMeta taskMeta, bool isProcessing)
        {
            InitializeComponent();
            this.host = host;
            this.instructor = instructor;
            this.project = project;
            this.taskMeta = taskMeta;
            this.isProcessing = isProcessing;
            InitUserControl();
        }
        public TaskMeta GetTask
        {
            get { return this.taskMeta; }
        }
        private void InitUserControl()
        {
            creator = UserDAO.SelectOnlyByID(taskMeta.Task.CreatedBy);
            team = TeamDAO.SelectFollowProject(this.project.ProjectId);

            lblTaskTitle.Text = DataTypeUtil.FormatStringLength(taskMeta.Task.Title, 60);
            lblCreator.Text = creator.FullName;
            gProgressBarToLine.Value = (int)taskMeta.Task.Progress;
            GunaControlUtil.SetItemFavorite(gButtonStar, taskMeta.IsFavorite);

            if (!isProcessing || (host.Role == EUserRole.STUDENT && taskMeta.Task.CreatedBy != host.UserId))
            {
                gButtonDelete.Hide();
                lblTaskTitle.Text = DataTypeUtil.FormatStringLength(taskMeta.Task.Title, 53);
            }
        }
        private void TaskDetailsShow(Notification notification, bool flag)
        {
            FTaskDetails fTaskDetails = new FTaskDetails(host, instructor, project, taskMeta, creator, team, isProcessing);
            if (flag) fTaskDetails.PerformNotificationClick(notification);
            fTaskDetails.FormClosed += FTaskDetails_FormClosed;
            fTaskDetails.ShowDialog();
        }
        public void PerformNotificationClick(Notification notification)
        {
            TaskDetailsShow(notification, true);
        }
        private void gShadowPanelTeam_Click(object sender, EventArgs e)
        {
            TaskDetailsShow(new Notification(), false);
        }
        private void FTaskDetails_FormClosed(object? sender, FormClosedEventArgs e)
        {
            FTaskDetails fTaskDetails = sender as FTaskDetails;

            if (fTaskDetails != null)
            {
                if (fTaskDetails.Edited)
                {
                    this.taskMeta.Task = TaskDAO.SelectOnly(taskMeta.Task.TaskId);
                    InitUserControl();
                }
            }
        }
        private void gButtonStar_Click(object sender, EventArgs e)
        {
            taskMeta.IsFavorite = !taskMeta.IsFavorite;
            TaskDAO.UpdateFavorite(host.UserId, taskMeta.Task.TaskId, taskMeta.IsFavorite);
            GunaControlUtil.SetItemFavorite(gButtonStar, taskMeta.IsFavorite);
        }
        private void gButtonDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete " + taskMeta.Task.TaskId,
                                                    "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                TaskDAO.Delete(taskMeta.Task.TaskId);
                OnTasksDeleteClicked(EventArgs.Empty);
            }
        }
        private void OnTasksDeleteClicked(EventArgs e)
        {
            TasksDeleteClicked?.Invoke(this, e);
        }
    }
}
