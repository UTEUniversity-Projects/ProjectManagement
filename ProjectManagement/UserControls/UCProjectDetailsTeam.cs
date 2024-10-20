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
using ProjectManagement.MetaData;
using ProjectManagement.Models;
using ProjectManagement.Process;
using ProjectManagement.Utils;

namespace ProjectManagement
{
    public partial class UCProjectDetailsTeam : UserControl
    {

        private Team team = new Team();
        private ProjectMeta projectMeta = new ProjectMeta();
        private List<Member> members = new List<Member>();

        public UCProjectDetailsTeam()
        {
            InitializeComponent();
        }
        public void SetInformation(Team team, ProjectMeta projectMeta)
        {
            this.team = team;
            this.projectMeta = projectMeta;
            this.members = TeamDAO.GetMembersByTeamId(team.TeamId);
            InitUserControl();
        }
        private void InitUserControl()
        {
            gCirclePictureBoxAvatar.Image = WinformControlUtil.NameToImage(team.Avatar);
            lblTeamName.Text = DataTypeUtil.FormatStringLength(team.TeamName, 20);
            gTextBoxTeamMemebrs.Text = members.Count.ToString() + " members";
        }
        private void ShowTeam()
        {
            FTeamDetails fTeamDetails = new FTeamDetails(team, projectMeta);
            fTeamDetails.ShowDialog();
        }

        private void gCirclePictureBoxAvatar_Click(object sender, EventArgs e)
        {
            ShowTeam();
        }
        private void gGradientButtonViewDetails_Click(object sender, EventArgs e)
        {
            ShowTeam();
        }
    }
}
