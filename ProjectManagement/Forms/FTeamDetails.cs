using ProjectManagement.DAOs;
using ProjectManagement.Models;
using ProjectManagement.Enums;
using ProjectManagement.Utils;
using ProjectManagement.MetaData;

namespace ProjectManagement
{
    public partial class FTeamDetails : Form
    {
        
        private Team team = new Team();
        private Project project = new Project();

        private int progress = 0;
        private bool isFavorite = false;
        private List<Member> members = new List<Member>();
        private List<Tasks> listTasks = new List<Tasks>();

        public FTeamDetails(Team team, ProjectMeta projectMeta)
        {
            InitializeComponent();
            this.team = team;
            this.project = projectMeta.Project;
            this.isFavorite = projectMeta.IsFavorite;
            SetInformation();
        }

        #region FUNCTIONS

        private void SetInformation()
        {
            this.members = TeamDAO.GetMembersByTeamId(team.TeamId);
            this.listTasks = TaskDAO.SelectListByTeam(this.team.TeamId);
            InitUserControl();
        }
        private void InitUserControl()
        {
            SetTeam(this.team);

            if (this.project.ProjectId == string.Empty)
            {
                gShadowPanelProject.Controls.Clear();
                gShadowPanelProject.Controls.Add(GunaControlUtil.CreatePictureBox(Properties.Resources.PictureEmptyState, new Size(399, 266)));
            }
            else
            {
                gShadowPanelProject.Controls.Clear();
                UCProjectMiniBoard uCProjectMiniBoard = new UCProjectMiniBoard(new ProjectMeta(project, isFavorite));
                gShadowPanelProject.Controls.Add(uCProjectMiniBoard);
            }
            UpdateChart();
        }
        public void SetTeam(Team team)
        {
            gCirclePictureBoxAvatar.Image = WinformControlUtil.NameToImage(team.Avatar);
            lblViewHandle.Text = DataTypeUtil.FormatStringLength(team.TeamName, 20);
            gTextBoxTeamCode.Text = team.TeamId;
            gTextBoxCreated.Text = team.CreatedAt.ToString("dd/MM/yyyy");
            gTextBoxTeamMemebrs.Text = this.members.Count.ToString() + " members";

            flpMembers.Controls.Clear();
            foreach (Member member in this.members)
            {
                UCUserMiniLine line = new UCUserMiniLine(member.User);
                line.SetMemberMode(new Size(320, 60), SystemColors.ButtonFace, member.Role);
                flpMembers.Controls.Add(line);
            }
        }

        #endregion

        #region CHART

        public void UpdateChart()
        {
            this.gSplineAreaDataset.DataPoints.Clear();
            double sum = 0.0D;
            int numberOfTasks = this.listTasks.Count;
            for (int i = 0; i < numberOfTasks; i++)
            {
                string name = "Task " + i.ToString();
                sum = sum + listTasks[i].Progress;
                this.gSplineAreaDataset.DataPoints.Add(name, this.listTasks[i].Progress);
            }

            if (numberOfTasks != 0) this.progress = int.Parse(Math.Round(sum / numberOfTasks, 0).ToString());
            this.gProgressBar.Value = this.progress;
            lblTotalProgress.Text = this.progress.ToString() + "%";

            this.gChart.Datasets.Clear();
            this.gChart.Datasets.Add(gSplineAreaDataset);
            this.gChart.Update();
        }

        #endregion

    }
}
