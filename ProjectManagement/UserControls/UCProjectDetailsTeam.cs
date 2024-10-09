using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjectManagement.Models;
using ProjectManagement.Process;

namespace ProjectManagement
{
    public partial class UCProjectDetailsTeam : UserControl
    {
        private MyProcess myProcess = new MyProcess();
        private Team team = new Team();
        private Project thesis = new Project();

        public UCProjectDetailsTeam()
        {
            InitializeComponent();
        }
        public void SetInformation(Team team, Project thesis)
        {
            this.team = team;
            this.thesis = thesis;
            InitUserControl();
        }
        private void InitUserControl()
        {
            gCirclePictureBoxAvatar.Image = myProcess.NameToImage(team.AvatarName);
            lblTeamName.Text = myProcess.FormatStringLength(team.TeamName, 20);
            gTextBoxTeamCode.Text = team.IdTeam;
            gTextBoxTeamMemebrs.Text = team.Members.Count.ToString() + " members";
        }
        private void ShowTeam()
        {
            FTeamDetails fTeamDetails = new FTeamDetails(team, thesis);
            fTeamDetails.ShowDialog();
        }

        private void UCThesisDetailsTeam_Click(object sender, EventArgs e)
        {
            ShowTeam();
        }
        private void gCirclePictureBoxAvatar_Click(object sender, EventArgs e)
        {
            ShowTeam();
        }
    }
}
