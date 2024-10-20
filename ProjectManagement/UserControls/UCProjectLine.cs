using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using ProjectManagement.DAOs;
using ProjectManagement.Enums;
using ProjectManagement.Models;
using ProjectManagement.Process;
using ProjectManagement.Utils;

namespace ProjectManagement
{
    public partial class UCProjectLine : UserControl
    {
        
        public event EventHandler NotificationJump;
        public event EventHandler ProjectLineClicked;
        public event EventHandler ProjectDeleteClicked;
        public event EventHandler ProjectFavoriteClicked;

        private Project project = new Project();
        private Users host = new Users();
        private Users creator = new Users();
        private Users instructor = new Users();
        private Notification notification = new Notification();

        private bool isFavorite = false;

        public UCProjectLine()
        {
            InitializeComponent();
        }

        #region PROPERTIES

        public string GetIdProject
        {
            get { return this.project.ProjectId; }
        }
        public bool IsFavorite
        {
            get { return this.isFavorite; }
        }
        public Notification GetNotification
        {
            get { return this.notification; }
        }

        #endregion

        #region FUNCTIONS

        public void SetInformation(Users host, Project project, bool isFavorite)
        {
            this.host = host;
            this.project = project;
            this.isFavorite = isFavorite;
            this.creator = UserDAO.SelectOnlyByID(project.CreatedBy);
            this.instructor = UserDAO.SelectOnlyByID(project.InstructorId);
            HideToolBar();
            InitUserControl();
        }
        private void InitUserControl()
        {
            GunaControlUtil.SetItemFavorite(gButtonStar, isFavorite);
            lblProjectTopic.Text = DataTypeUtil.FormatStringLength(project.Topic, 130);
            gTextBoxStatus.Text = EnumUtil.GetDisplayName(project.Status);
            gTextBoxStatus.FillColor = project.GetStatusColor();
            lblCreator.Text = creator.FullName;
            lblInstructor.Text = instructor.FullName;
        }
        private void SetColor(Color color)
        {
            this.BackColor = color;
        }
        private void HideToolBar()
        {
            if (project.Status == EProjectStatus.COMPLETED || project.Status == EProjectStatus.GAVEUP)
            {
                gButtonEdit.Hide();
                return;
            }
            if (host.Role == EUserRole.STUDENT && host.UserId != project.CreatedBy)
            {
                gButtonEdit.Hide();
                gButtonDelete.Hide();
                return;
            }
        }
        public void RemoveProject()
        {
            ProjectDAO.Delete(project.ProjectId);
            List<Team> listTeam = TeamDAO.SelectList(this.project.ProjectId);
            TeamDAO.DeleteListTeam(listTeam);

            OnProjectDeleteClicked(EventArgs.Empty);
        }
        public void PerformNotificationClick(Notification notification)
        {
            this.notification = notification;
            OnNotificationJump(EventArgs.Empty);
        }
        private void OnNotificationJump(EventArgs e)
        {
            NotificationJump?.Invoke(this, e);
        }

        #endregion

        #region EVENT USER CONTROL

        private void UCProjectLine_MouseEnter(object sender, EventArgs e)
        {
            SetColor(Color.Gainsboro);
        }
        private void UCProjectLine_MouseLeave(object sender, EventArgs e)
        {
            SetColor(Color.White);
        }
        private void UCProjectLine_Click(object sender, EventArgs e)
        {
            OnProjectLineClicked(EventArgs.Empty);
        }
        private void OnProjectLineClicked(EventArgs e)
        {
            ProjectLineClicked?.Invoke(this, e);
        }

        #endregion

        #region EVENT Toolbar

        private void gButtonEdit_Click(object sender, EventArgs e)
        {
            FProjectEdit fProjectEdit = new FProjectEdit(instructor, project);
            fProjectEdit.ShowDialog();
            this.project = ProjectDAO.SelectOnly(project.ProjectId);
            InitUserControl();
        }
        private void gButtonDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete project " + project.Topic,
                                                    "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                RemoveProject();
            }
        }
        public virtual void OnProjectDeleteClicked(EventArgs e)
        {
            ProjectDeleteClicked?.Invoke(this, e);
        }

        #endregion

        #region EVENT gButtonStar

        private void gButtonStar_Click(object sender, EventArgs e)
        {
            this.isFavorite = !this.isFavorite;
            ProjectDAO.UpdateFavorite(this.host.UserId, this.project.ProjectId, this.isFavorite);            
            GunaControlUtil.SetItemFavorite(gButtonStar, this.isFavorite);

            OnProjectFavoriteClicked(e);
        }
        public void OnProjectFavoriteClicked(EventArgs e)
        {
            ProjectFavoriteClicked?.Invoke(this, e);
        }

        #endregion

    }
}
