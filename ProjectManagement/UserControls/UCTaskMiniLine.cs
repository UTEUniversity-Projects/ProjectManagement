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
using ProjectManagement.Process;
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
        private Tasks task = new Tasks();
        private Team team = new Team();
        private Users host = new Users();

        private TeamDAO TeamDAO = new TeamDAO();
        private TaskDAO TaskDAO = new TaskDAO();

        private bool isProcessing = false;

        public UCTaskMiniLine(Users host, Users instructor, Project project, Tasks task, bool isProcessing)
        {
            InitializeComponent();
            this.host = host;
            this.instructor = instructor;
            this.project = project;
            this.task = task;
            this.isProcessing = isProcessing;
            InitUserControl();
        }
        public Tasks GetTask
        {
            get { return this.task; }
        }
        private void InitUserControl()
        {
            creator = UserDAO.SelectOnlyByID(task.CreatedBy);
            // team = TeamDAO.SelectOnly(task.TeamId);

            lblTaskTitle.Text = DataTypeUtil.FormatStringLength(task.Title, 60);
            lblCreator.Text = creator.FullName;
            gProgressBarToLine.Value = (int)task.Progress;
            // GunaControlUtil.SetItemFavorite(gButtonStar, task.IsFavorite);

            if (!isProcessing || (host.Role == EUserRole.STUDENT && task.CreatedBy != host.UserId))
            {
                gButtonDelete.Hide();
                lblTaskTitle.Text = DataTypeUtil.FormatStringLength(task.Title, 53);
            }
        }
        private void TaskDetailsShow(Notification notification, bool flag)
        {
            FTaskDetails fTaskDetails = new FTaskDetails(host, instructor, project, task, creator, team, isProcessing);
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
                    this.task = TaskDAO.SelectOnly(task.TaskId);
                    InitUserControl();
                }
            }
        }
        private void gButtonStar_Click(object sender, EventArgs e)
        {
            // task.IsFavorite = !task.IsFavorite;

            // GunaControlUtil.SetItemFavorite(gButtonStar, task.IsFavorite);
            TaskDAO.UpdateIsFavorite(task);
        }
        private void gButtonDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete " + task.TaskId,
                                                    "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                TaskDAO.Delete(task);
                OnTasksDeleteClicked(EventArgs.Empty);
            }
        }
        private void OnTasksDeleteClicked(EventArgs e)
        {
            TasksDeleteClicked?.Invoke(this, e);
        }
    }
}
