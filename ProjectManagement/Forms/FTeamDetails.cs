using ProjectManagement.DAOs;
using ProjectManagement.Models;
using ProjectManagement.Process;
using ProjectManagement.Utils;

namespace ProjectManagement
{
    public partial class FTeamDetails : Form
    {
        
        private Team team = new Team();
        private Project project = new Project();
        private TaskDAO TaskDAO = new TaskDAO();

        private int progress = 0;
        private List<Tasks> listTasks;

        public FTeamDetails(Team team, Project project)
        {
            InitializeComponent();
            SetInformation(team, project);
        }

        #region FUNCTIONS

        private void SetInformation(Team team, Project project)
        {
            this.team = team;
            this.project = project;
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
                UCProjectMiniBoard uCProjectMiniBoard = new UCProjectMiniBoard(project);
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
            gTextBoxTeamMemebrs.Text = TeamDAO.GetMembersByTeamId(team.TeamId).Count.ToString() + " members";

            flpMembers.Controls.Clear();
            foreach (Users user in TeamDAO.GetMembersByTeamId(team.TeamId))
            {
                UCUserMiniLine line = new UCUserMiniLine(user);
                line.SetBackGroundColor(SystemColors.ButtonFace);
                line.SetSize(new Size(340, 60));
                line.GButtonAdd.Hide();
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
