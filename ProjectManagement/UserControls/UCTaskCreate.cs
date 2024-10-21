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
using ProjectManagement.Enums;
using ProjectManagement.MetaData;

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

        private List<Users> students = new List<Users>();

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
            EnumUtil.AddEnumsToComboBox(gComboBoxPriority, typeof(ETaskPriority));

            gDateTimePickerStart.Value = DateTime.Now.AddMinutes(5);
            gDateTimePickerStart.Format = DateTimePickerFormat.Custom;
            gDateTimePickerStart.CustomFormat = "dd/MM/yyyy HH:mm:ss tt";

            gDateTimePickerEnd.Value = DateTime.Now.AddMinutes(6);
            gDateTimePickerEnd.Format = DateTimePickerFormat.Custom;
            gDateTimePickerEnd.CustomFormat = "dd/MM/yyyy HH:mm:ss tt";

            flpMembers.Controls.Clear();
            foreach (Member member in TeamDAO.GetMembersByTeamId(team.TeamId))
            {
                UCUserMiniLine line = new UCUserMiniLine(member.User);
                line.SetBackGroundColor(SystemColors.ButtonFace);
                line.SetSize(new Size(285, 60));
                line.GButtonAdd.Location = new Point(230, 10);
                line.GButtonAdd.HoverState.FillColor = SystemColors.ButtonFace;
                line.GButtonAdd.HoverState.Image = null;
                line.ButtonAddClicked += (sender, e) => ButtonAdd_Clicked(sender, e, member.User);
                line.GButtonAdd.Show();
                flpMembers.Controls.Add(line);
            }
        }

        private void ButtonAdd_Clicked(object? sender, EventArgs e, Users user)
        {
            UCUserMiniLine line = (UCUserMiniLine)sender;

            if (line != null)
            {
                if (WinformControlUtil.ImageEquals(line.GButtonAdd.Image, Properties.Resources.PicItemComplete))
                {
                    line.GButtonAdd.Image = Properties.Resources.PicItemAdd;
                    this.students.Remove(user);
                }
                else
                {
                    line.GButtonAdd.Image = Properties.Resources.PicItemComplete;
                    this.students.Add(user);
                }

                WinformControlUtil.RunCheckDataValid(CheckAssignStudent(), erpAssign, lblAssignStudent, "You must assign at least one student");
            }
        }

        private bool CheckAssignStudent()
        {
            return this.students.Count > 0;
        }

        private bool CheckInformationValid()
        {
            WinformControlUtil.RunCheckDataValid(task.CheckTitle() || flagCheck, erpTitle, gTextBoxTitle, "Title cannot be empty");
            WinformControlUtil.RunCheckDataValid(task.CheckDescription() || flagCheck, erpDescription, gTextBoxDescription, "Description cannot be empty");
            WinformControlUtil.RunCheckDataValid(task.CheckStart() || flagCheck, erpStart, gDateTimePickerStart, "Invalid start time");
            WinformControlUtil.RunCheckDataValid(task.CheckEnd() || flagCheck, erpEnd, gDateTimePickerEnd, "The end time must be after the start time");
            WinformControlUtil.RunCheckDataValid(CheckAssignStudent() || flagCheck, erpAssign, lblAssignStudent, "You must assign at least one student");

            return task.CheckTitle() && task.CheckDescription() && task.CheckStart() && task.CheckEnd() && CheckAssignStudent();
        }

        #endregion

        #region EVENT gButtonCreate

        private void gButtonCreate_Click(object sender, EventArgs e)
        {
            this.flagCheck = false;
            if (CheckInformationValid())
            {
                this.task = new Tasks(gDateTimePickerStart.Value, gDateTimePickerEnd.Value, gTextBoxTitle.Text, gTextBoxDescription.Text, 0.0D, EnumUtil.GetEnumFromDisplayName<ETaskPriority>(gComboBoxPriority.SelectedItem.ToString()), DateTime.Now, this.creator.UserId, this.project.ProjectId);
                TaskDAO.Insert(task);
                foreach (Users student in students)
                {
                    TaskDAO.InsertAssignStudent(task.TaskId, student.UserId);
                    EvaluationDAO.InsertAssignStudent(instructor.UserId, task.TaskId, student.UserId);
                }

                List<Users> peoples = TeamDAO.GetMembersByTaskId(this.task.TaskId).Select(m => m.User).ToList();
                peoples.Add(this.instructor);
                string content = Notification.GetContentTypeTask(creator.FullName, task.Title, project.Topic);
                Notification notification = new Notification("Notification", content, ENotificationType.TASK, DateTime.Now);
                NotificationDAO.InsertOnly(notification);
                foreach (Users user in peoples)
                {
                    NotificationDAO.InsertViewNotification(user.UserId, notification.NotificationId, false);
                }

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

        #region EVENT TEXT CHANGED and VALUE CHANGED

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
        private void gDateTimePickerStart_ValueChanged(object sender, EventArgs e)
        {
            this.task.StartAt = gDateTimePickerStart.Value;
            WinformControlUtil.RunCheckDataValid(task.CheckStart() || flagCheck, erpStart, gDateTimePickerStart, "Invalid start time");
        }
        private void gDateTimePickerEnd_ValueChanged(object sender, EventArgs e)
        {
            this.task.EndAt = gDateTimePickerEnd.Value;
            WinformControlUtil.RunCheckDataValid(task.CheckEnd() || flagCheck, erpEnd, gDateTimePickerEnd, "The end time must be after the start time");
        }
        private void gComboBoxPriority_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gComboBoxPriority.SelectedItem != null &&
            Enum.TryParse<ETaskPriority>(gComboBoxPriority.SelectedItem.ToString(), out var priority))
            {
                this.task.Priority = priority;
            }

        }
        #endregion
    }
}
