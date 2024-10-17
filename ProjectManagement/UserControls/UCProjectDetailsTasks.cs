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
using ProjectManagement.Models;
using ProjectManagement.Enums;

namespace ProjectManagement
{
    public partial class UCProjectDetailsTasks : UserControl
    {
        private Users user = new Users();
        private Users instructor = new Users();
        private Team team = new Team();
        private Project project = new Project();
        private List<Tasks> listTask = new List<Tasks>();

        private UCTaskCreate uCTaskCreate = new UCTaskCreate();
        private bool isProcessing = true;

        public UCProjectDetailsTasks()
        {
            InitializeComponent();
        }

        #region FUNCTIONS

        public void SetUpUserControl(Users user, Users instructor, Team team, Project project, bool isProcessing)
        {
            this.user = user;
            this.instructor = instructor;
            this.team = team;
            this.project = project;
            this.isProcessing = isProcessing;
            InitUserControl();
        }
        private void InitUserControl()
        {
            flpTaskList.Location = new Point(12, 12);
            uCTaskCreate.SetUpUserControl(user, instructor, team, project);
            uCTaskCreate.Location = new Point(10, 10);
            uCTaskCreate.GButtonCancel.Click += GButtonCancel_Click;
            uCTaskCreate.TasksCreateClicked += UCTaskCreate_TasksCreateClicked;
            gShadowPanelTaskCreate.Controls.Add(uCTaskCreate);
            gShadowPanelTaskCreate.Hide();

            if (!isProcessing)
            {
                gGradientButtonAddTask.Hide();
            }
            else
            {
                gGradientButtonAddTask.Show();
            }

            UpdateTaskList();
            LoadTaskList();
        }
        private void UCTaskCreate_TasksCreateClicked(object sender, EventArgs e)
        {
            Tasks task = TaskDAO.SelectOnly(uCTaskCreate.GetTasks.TaskId);

            this.listTask.Add(task);
            UCTaskMiniLine line = new UCTaskMiniLine(user, instructor, project, task, isProcessing);
            line.TasksDeleteClicked += GButtonDelete_Click;
            flpTaskList.Controls.Add(line);
            flpTaskList.Controls.SetChildIndex(line, 0);
        }
        private void UpdateTaskList()
        {
            this.listTask.Clear();
            this.listTask = TaskDAO.SelectListByTeam(this.team.TeamId);
        }
        private void LoadTaskList()
        {
            flpTaskList.Controls.Clear();
            foreach (Tasks task in listTask)
            {
                UCTaskMiniLine line = new UCTaskMiniLine(user, instructor, project, task, isProcessing);
                line.TasksDeleteClicked += GButtonDelete_Click;
                flpTaskList.Controls.Add(line);
            }
        }
        private void GButtonDelete_Click(object sender, EventArgs e)
        {
            UCTaskMiniLine line = sender as UCTaskMiniLine;

            if (line != null)
            {
                foreach (Control control in flpTaskList.Controls)
                {
                    if (control.GetType() == typeof(UCTaskMiniLine))
                    {
                        UCTaskMiniLine taskLine = (UCTaskMiniLine)control;
                        if (taskLine == line)
                        {
                            flpTaskList.Controls.Remove(control);
                            this.listTask.Remove(line.GetTask);
                            control.Dispose();
                            break;
                        }
                    }
                }
            }
        }
        public void PerformNotificationClick(ENotificationType type)
        {
            //Tasks task = new Tasks();
            //switch (type)
            //{
            //    case ENotificationType.TASK:
            //        task = TaskDAO.SelectOnly(notification.IdObject);
            //        break;
            //    case ENotificationType.COMMENT:
            //        task = TaskDAO.SelectFromComment(notification.IdObject);
            //        break;
            //    case ENotificationType.EVALUATION:
            //        task = TaskDAO.SelectFromEvaluation(notification.IdObject);
            //        break;
            //}

            //foreach (UCTaskMiniLine line in flpTaskList.Controls)
            //{
            //    if (line != null)
            //    {
            //        if (line.GetTask.TaskId == task.TaskId)
            //        {
            //            line.PerformNotificationClick(notification);
            //            return;
            //        }
            //    }
            //}            
        }

        #endregion

        #region EVENT UserControl ADDTASK and TASKCREATE

        private void gGradientButtonAddTask_Click(object sender, EventArgs e)
        {
            lblTaskList.Text = "CREATE TASK";
            flpTaskList.Hide();
            gShadowPanelTaskCreate.Show();
            gShadowPanelTaskCreate.BringToFront();
        }
        private void GButtonCancel_Click(object? sender, EventArgs e)
        {
            lblTaskList.Text = "TASK LIST";
            flpTaskList.Show();
            gShadowPanelTaskCreate.Hide();
            gShadowPanelTaskCreate.SendToBack();
        }

        #endregion

        #region EVENT TEXTBOX SEARCH

        private void gTextBoxSearch_TextChanged(object sender, EventArgs e)
        {
            this.listTask = TaskDAO.SearchTaskTitle(this.project.ProjectId, gTextBoxSearch.Text);
            LoadTaskList();
        }

        #endregion

    }
}
