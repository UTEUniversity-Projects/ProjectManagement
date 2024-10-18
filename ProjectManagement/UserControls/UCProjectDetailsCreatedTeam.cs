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
    public partial class UCProjectDetailsCreatedTeam : UserControl
    {
        
        public event EventHandler RegisteredPerform;

        private Users user = new Users();
        private Project project = new Project();
        private List<Users> listUser = new List<Users>();
        private List<Users> members = new List<Users>();

        private Image pictureAvatar;
        private bool flagCheck = false;

        #region CONTRUCTERS

        public UCProjectDetailsCreatedTeam()
        {
            InitializeComponent();
            InitUserControl();
        }
        public UCProjectDetailsCreatedTeam(Users user, Project project)
        {
            InitializeComponent();
            this.user = user;
            this.project = project;
            InitUserControl();
            AddMember(this.user);
        }

        #endregion

        #region PROPERTIES

        public Guna2GradientButton GPerform
        {
            get { return this.gGradientButtonPerform; }
        }

        #endregion

        #region FUNCTIONS

        private void InitUserControl()
        {
            pictureAvatar = Properties.Resources.PicAvatarDemoUser;
            gGradientButtonRegister.Enabled = true;
            flpSearch.Location = new Point(11, 14);
            flpSearch.Hide();
            gGradientButtonPerform.Hide();
        }
        private void AddMember(Users user)
        {
            if (members.Count < this.project.MaxMember)
            {
                UCUserMiniLine line = new UCUserMiniLine(user);
                line.SetBackGroundColor(Color.White);
                line.SetSize(new Size(310, 60));
                if (user.UserId == this.user.UserId)
                {
                    line.SetDeleteMode(false);
                }
                else
                {
                    line.SetButtonDelete();
                    line.ButtonDeleteClicked += (sender, e) => ButtonDelete_Clicked(sender, e, line);
                }
                flpTeam.Controls.Add(line);
                members.Add(user);
            }
            else
            {
                MessageBox.Show("The number of members cannot be exceeded " + this.project.MaxMember,
                                                    "OK", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }
        private void LoadUserList()
        {
            flpSearch.Controls.Clear();
            int maxLine = 4;
            foreach (Users user in members)
            {
                Users foundUser = listUser.Find(p => p.UserId == user.UserId);
                if (foundUser != null)
                {
                    listUser.Remove(foundUser);
                }
            }
            int size = Math.Min(maxLine, listUser.Count);
            for (int i = 0; i < size; i++)
            {
                Users user = listUser[i];
                UCUserMiniLine uCUserMiniLine = new UCUserMiniLine(user);
                uCUserMiniLine.SetSize(new Size(310, 60));
                uCUserMiniLine.ButtonAddClicked += (sender, e) => ButtonAdd_Clicked(sender, e, user);
                flpSearch.Controls.Add(uCUserMiniLine);
            }
            flpSearch.Show();
            flpSearch.BringToFront();
        }
        public bool CheckEmpty(string text)
        {
            return !string.IsNullOrEmpty(text);
        }
        #endregion

        #region USER MINI LINE

        private void ButtonAdd_Clicked(object sender, EventArgs e, Users user)
        {
            AddMember(user);
            gTextBoxSearch.Text = string.Empty;
        }
        private void ButtonDelete_Clicked(object sender, EventArgs e, UCUserMiniLine user)
        {
            if (user.GetUser.UserId != this.user.UserId)
            {
                members.Remove(user.GetUser);
                flpTeam.Controls.Remove(user);
            }
            else
            {
                MessageBox.Show("Cannot delete yourself", "OK", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region FUNCTIONS SEARCH

        private void gTextBoxSearch_TextChanged(object sender, EventArgs e)
        {
            Guna2TextBox textBox = sender as Guna2TextBox;

            if (!string.IsNullOrEmpty(textBox.Text))
            {
                this.listUser = UserDAO.SelectListByUserName(textBox.Text, user.Role);
                flpTeam.Hide();
                LoadUserList();
            }
            else
            {
                flpSearch.Controls.Clear();
                flpSearch.Hide();
                flpTeam.Show();
                flpTeam.BringToFront();
                listUser = new List<Users>();
            }
        }

        #endregion

        #region EVENT gGradientButtonRegister

        private void gGradientButtonApply_Click(object sender, EventArgs e)
        {
            if (CheckEmpty(gTextBoxTeamName.Text))
            {
                this.project.Status = EProjectStatus.REGISTERED;

                Team team = new Team(gTextBoxTeamName.Text, WinformControlUtil.ImageToName(pictureAvatar), DateTime.Now, this.user.UserId,
                    this.project.ProjectId, ETeamStatus.REGISTERED);

                TeamDAO.Insert(team, this.members);
                ProjectDAO.UpdateStatus(this.project, EProjectStatus.REGISTERED);

                string content = Notification.GetContentTypeRegistered(team.TeamName, project.Topic);
                Notification notification = new Notification(team.TeamName + " just registered", content, Notification.GetNotificationType(project.ProjectId), DateTime.Now);
                NotificationDAO.Insert(notification, project.InstructorId);

                string message = Notification.GetContentRegisteredMembers(user.FullName, team.TeamName, project.Topic);
                NotificationDAO.InsertFollowTeam(team.TeamId, message, ENotificationType.PROJECT);

                MessageBox.Show("Registered successfully", "Notification", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                this.gGradientButtonRegister.Enabled = false;
                gGradientButtonPerform.PerformClick();
            } 
            else
            {
                gTextBoxTeamName_TextChanged(gTextBoxTeamName, new EventArgs());
            }
        }

        #endregion

        #region EVENT gCirclePictureBoxAvatar

        private void gCirclePictureBoxAvatar_MouseEnter(object sender, EventArgs e)
        {
            gCirclePictureBoxAvatar.Image = Properties.Resources.PictureCameraHover;
        }

        private void gCirclePictureBoxAvatar_MouseLeave(object sender, EventArgs e)
        {
            gCirclePictureBoxAvatar.Image = this.pictureAvatar;
        }

        private void gCirclePictureBoxAvatar_Click(object sender, EventArgs e)
        {
            FListAvatar fListAvatar = new FListAvatar();
            fListAvatar.FormClosed += FListAvatar_FormClosed;
            fListAvatar.ShowDialog();
        }
        private void FListAvatar_FormClosed(object sender, FormClosedEventArgs e)
        {
            FListAvatar fListAvatar = (FListAvatar)sender;
            this.pictureAvatar = fListAvatar.PictureAvatar;
            gCirclePictureBoxAvatar.Image = this.pictureAvatar;
        }

        #endregion

        #region EVENT gTextBoxTeamName

        private void gTextBoxTeamName_TextChanged(object sender, EventArgs e)
        {
            Guna2TextBox textBox = (Guna2TextBox)sender;
            WinformControlUtil.RunCheckDataValid(CheckEmpty(textBox.Text) || flagCheck, erpTeamName, gTextBoxTeamName, "Name can not empty");
        }

        #endregion

        #region EVENT gGradientButtonPerform

        private void gGradientButtonPerform_Click(object sender, EventArgs e)
        {
            OnRegisterClicked(EventArgs.Empty);
        }
        public virtual void OnRegisterClicked(EventArgs e)
        {
            RegisteredPerform?.Invoke(this, e);
        }

        #endregion

    }
}
