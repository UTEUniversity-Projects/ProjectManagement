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
using ProjectManagement.Models;
using ProjectManagement.Utils;

namespace ProjectManagement
{
    public partial class UCTaskCreate : UserControl
    {
        
        public event EventHandler TasksCreateClicked;

        private Users creator = new Users();
        private Users instructor = new Users();
        private Tasks task = new Tasks();
        private Team team = new Team();
        private Project project = new Project();

        private TaskDAO TaskDAO = new TaskDAO();
        private EvaluationDAO EvaluationDAO = new EvaluationDAO();
        private NotificationDAO NotificationDAO = new NotificationDAO();

        private bool flagCheck = false;

        public UCTaskCreate()
        {
            InitializeComponent();
        }

        #region PROPERTIES

        public Guna2Button GButtonCancel
        {
            get { return this.gButtonCancel; }
        }
        public Tasks GetTasks
        {
            get { return this.task; }
        }

        #endregion

        #region FUNCTIONS

        public void SetUpUserControl(Users creator, Users instructor, Team team, Project project)
        {
            this.creator = creator;
            this.instructor = instructor;
            this.team = team;
            this.project = project;
            InitUserControl();
        }
        private void InitUserControl()
        {
            gTextBoxTitle.Text = string.Empty;
            gTextBoxDescription.Text = string.Empty;
        }
        private bool CheckInformationValid()
        {
            WinformControlUtil.RunCheckDataValid(task.CheckTitle() || flagCheck, erpTitle, gTextBoxTitle, "Title cannot be empty");
            WinformControlUtil.RunCheckDataValid(task.CheckDescription() || flagCheck, erpDescription, gTextBoxDescription, "Description cannot be empty");

            return task.CheckTitle() && task.CheckDescription();
        }

        #endregion

        #region EVENT gButtonCreate

        private void gButtonCreate_Click(object sender, EventArgs e)
        {
            this.flagCheck = false;
            if (CheckInformationValid())
            {
                this.task = new Tasks(DateTime.MinValue, DateTime.MaxValue, gTextBoxTitle.Text, gTextBoxDescription.Text,
                    0.0D, "Low", DateTime.Now, this.creator.UserId, this.project.ProjectId);
                TaskDAO.Insert(task);
                EvaluationDAO.InsertFollowTeam(instructor.UserId, task.TaskId, team.TeamId);

                List<Users> peoples = TeamDAO.GetMembersByTeamId(team.TeamId).ToList();
                peoples.Add(this.instructor);
                string content = Notification.GetContentTypeTask(creator.FullName, task.Title, project.Topic);
                NotificationDAO.InsertFollowTeam(this.team.TeamId, content, Enums.ENotificationType.TASK);

                this.flagCheck = true;
                InitUserControl();
                OnTasksCreateClicked(EventArgs.Empty);
            }
        }
        private void OnTasksCreateClicked(EventArgs e)
        { 
            TasksCreateClicked?.Invoke(this, e);
        }

        #endregion

        #region EVENT TEXTCHANGED

        private void gTextBoxTitle_TextChanged(object sender, EventArgs e)
        {
            this.task.Title = gTextBoxTitle.Text;
            WinformControlUtil.RunCheckDataValid(task.CheckTitle() || flagCheck, erpTitle, gTextBoxTitle, "Title cannot be empty");
        }
        private void gTextBoxDescription_TextChanged(object sender, EventArgs e)
        {
            this.task.Description = gTextBoxDescription.Text;
            WinformControlUtil.RunCheckDataValid(task.CheckDescription() || flagCheck, erpDescription, gTextBoxDescription, "Description cannot be empty");
        }

        #endregion

    }
}
