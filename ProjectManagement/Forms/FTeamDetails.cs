using Guna.UI2.WinForms;
using Guna.UI2.WinForms.Suite;
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
using ProjectManagement.Process;
using ProjectManagement.Properties;

namespace ProjectManagement
{
    public partial class FTeamDetails : Form
    {
        private MyProcess myProcess = new MyProcess();
        private Team team = new Team();
        private Project thesis = new Project();
        private TasksDAO tasksDAO = new TasksDAO();

        private int progress = 0;
        private List<Tasks> listTasks;

        public FTeamDetails(Team team, Project thesis)
        {
            InitializeComponent();
            SetInformation(team, thesis);
        }

        #region FUNCTIONS

        private void SetInformation(Team team, Project thesis)
        {
            this.team = team;
            this.thesis = thesis;
            this.listTasks = tasksDAO.SelectListByTeam(this.team.IdTeam);
            InitUserControl();
        }
        private void InitUserControl()
        {
            SetTeam(this.team);

            if (this.thesis.IdThesis == string.Empty)
            {
                gShadowPanelThesis.Controls.Clear();
                gShadowPanelThesis.Controls.Add(myProcess.CreatePictureBox(Properties.Resources.PictureEmptyState, new Size(399, 266)));
            }
            else
            {
                gShadowPanelThesis.Controls.Clear();
                UCProjectMiniBoard uCThesisMiniBoard = new UCProjectMiniBoard(thesis);
                gShadowPanelThesis.Controls.Add(uCThesisMiniBoard);
            }
            UpdateChart();
        }
        public void SetTeam(Team team)
        {
            gCirclePictureBoxAvatar.Image = myProcess.NameToImage(team.AvatarName);
            lblViewHandle.Text = myProcess.FormatStringLength(team.TeamName, 20);
            gTextBoxTeamCode.Text = team.IdTeam;
            gTextBoxCreated.Text = team.Created.ToString("dd/MM/yyyy");
            gTextBoxTeamMemebrs.Text = team.Members.Count.ToString() + " members";

            flpMembers.Controls.Clear();
            foreach (User people in team.Members)
            {
                UCUserMiniLine line = new UCUserMiniLine(people);
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
