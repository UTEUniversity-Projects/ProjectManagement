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
using ProjectManagement.MetaData;
using ProjectManagement.Utils;

namespace ProjectManagement
{
    public partial class UCTeamLine : UserControl
    {
        
        public event EventHandler ProjectAddAccepted;
        private Team team = new Team();
        private Users user = new Users();
        private Project project = new Project();
        private ProjectDAO ProjectDAO = new ProjectDAO();

        private bool isFavorite = false;

        public UCTeamLine(Team team, Users user)
        {
            InitializeComponent();
            this.team = team;
            this.user = user;
            SetInformation();
        }

        #region PROPERTIES

        public Team GetTeam
        {
            get { return this.team; }
        }

        #endregion

        #region FUNCTIONS 

        private void SetInformation()
        {
            this.project = ProjectDAO.SelectFollowTeam(team.TeamId);
            this.isFavorite = ProjectDAO.CheckIsFavorite(user.UserId, project.ProjectId);
            InitUserControl();
        }
        private void InitUserControl()
        {
            gCirclePictureBoxAvatar.Image = WinformControlUtil.NameToImage(team.Avatar);
            lblTeamName.Text = DataTypeUtil.FormatStringLength(team.TeamName, 30);
            lblTeamCode.Text = team.TeamId;
            gTextBoxTeamMemebrs.Text = TeamDAO.GetMembersByTeamId(team.TeamId).Count.ToString() + " members";
        }
        public void SetSize(Size size)
        {
            this.Size = size;
            gShadowPanelBack.Size = new Size(size.Width - 5, size.Height);
        }
        public void SetSimpleLine()
        {
            gTextBoxTeamMemebrs.Hide();
            gButtonAdd.Hide();
        }
        public void SetBackColor(Color color)
        {
            this.BackColor = color;
            gShadowPanelBack.BackColor = color;

            if (this.BackColor != Color.White) ExecuteBackGroundColor(Color.White);
            else ExecuteBackGroundColor(SystemColors.ButtonFace);
        }
        private void ExecuteBackGroundColor(Color color)
        {
            gShadowPanelBack.FillColor = color;
            gCirclePictureBoxAvatar.BackColor = color;
            lblTeamName.BackColor = color;
            lblTeamCode.BackColor = color;
        }

        #endregion

        #region EVENT CONTROLS

        private void gShadowPanelBack_Click(object sender, EventArgs e)
        {
            FTeamDetails fTeamDetails = new FTeamDetails(team, new ProjectMeta(project, isFavorite));
            fTeamDetails.ShowDialog();
        }
        private void gButtonAdd_Click(object sender, EventArgs e)
        {
            ProjectAddAcceptedClicked(EventArgs.Empty);
        }
        public virtual void ProjectAddAcceptedClicked(EventArgs e)
        {
            ProjectAddAccepted?.Invoke(this, e);
        }

        #endregion

    }
}
