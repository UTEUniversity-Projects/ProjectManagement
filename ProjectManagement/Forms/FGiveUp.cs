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
using ProjectManagement.Utils;

namespace ProjectManagement.Forms
{
    public partial class FGiveUp : Form
    {
        public event EventHandler ConfirmedGivingUp;        

        private Project project = new Project();
        private Users represent = new Users();
        private Team team = new Team();
        private GiveUp giveUp = new GiveUp();

        private bool flagCheck = false;

        public FGiveUp()
        {
            InitializeComponent();
        }
        public FGiveUp(Project project, Users represent, Team team)
        {
            InitializeComponent();

            this.project = project;
            this.represent = represent;
            this.team = team;
            this.giveUp = new GiveUp(project.ProjectId, represent.UserId, gTextBoxReason.Text, DateTime.Now, EGiveUpStatus.PENDING);
            this.flagCheck = false;
            SetUpUserControl();
        }
        public void SetUpUserControl()
        {
            SetProject();
            SetRepresent();
            SetTeam();
        }
        private void SetProject()
        {
            UCProjectMiniBoard uCProjectMiniBoard = new UCProjectMiniBoard(project);
            uCProjectMiniBoard.SetColorViewState(SystemColors.ButtonFace);
            gPanelProject.Controls.Clear();
            gPanelProject.Controls.Add(uCProjectMiniBoard);
        }
        private void SetRepresent()
        {
            UCUserMiniLine uCUserMiniLine = new UCUserMiniLine(represent);
            uCUserMiniLine.SetSize(new Size(275, 60));
            uCUserMiniLine.SetBackGroundColor(SystemColors.ButtonFace);
            uCUserMiniLine.GButtonAdd.Hide();
            gPanelRepresent.Controls.Clear();
            gPanelRepresent.Controls.Add(uCUserMiniLine);
        }
        private void SetTeam()
        {
            UCTeamLine uCTeamMiniLine = new UCTeamLine(team);
            uCTeamMiniLine.SetSize(new Size(275, 60));
            gPanelTeam.Controls.Clear();
            gPanelTeam.Controls.Add(uCTeamMiniLine);
        }
        public void SetReadOnly(GiveUp giveUp)
        {
            this.represent = UserDAO.SelectOnlyByID(giveUp.UserId);
            this.giveUp = giveUp;
            SetReasonReadOnly();
            SetRepresent();
        }
        private void SetReasonReadOnly()
        {
            gTextBoxReason.Text = giveUp.Reason;
            gTextBoxReason.FillColor = SystemColors.ButtonFace;
            gTextBoxReason.BorderThickness = 0;
            gTextBoxReason.ReadOnly = true;
            gButtonCancel.Location = new Point(672, 624);
            gButtonCancel.Show();
            gButtonConfirm.Hide();
            gButtonCancel.Focus();
        }
        private bool CheckInformationValid()
        {
            WinformControlUtil.RunCheckDataValid(giveUp.CheckReason() || flagCheck, erpReason, gTextBoxReason, "Reason cannot be empty");

            return giveUp.CheckReason();
        }
        private void gButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void gButtonConfirm_Click(object sender, EventArgs e)
        {
            this.flagCheck = false;
            if (CheckInformationValid())
            {
                DialogResult result = MessageBox.Show("The " + team.TeamName + " team definitely refused to complete the " + project.Topic + " project",
                                                        "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    ProjectDAO.UpdateStatus(this.project, EProjectStatus.GAVEUP);
                    GiveUpDAO.Insert(this.giveUp);

                    string content = Notification.GetContentTypeGiveUp(team.TeamName, project.Topic);
                    var peoples = new List<Users>();
                    peoples.AddRange(TeamDAO.GetMembersByTeamId(team.TeamId));
                    peoples.Add(UserDAO.SelectOnlyByID(project.InstructorId));
                    NotificationDAO.InsertFollowTeam(this.team.TeamId, content, ENotificationType.PROJECT);

                    ConfirmedGivingUp?.Invoke(this, e);
                    this.Close();
                }
            }
        }
        private void gTextBoxReason_TextChanged(object sender, EventArgs e)
        {
            giveUp.Reason = gTextBoxReason.Text;
            WinformControlUtil.RunCheckDataValid(giveUp.CheckReason() || flagCheck, erpReason, gTextBoxReason, "Reason cannot be empty");
        }
    }
}
