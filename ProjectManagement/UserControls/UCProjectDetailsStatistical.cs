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
using ProjectManagement.MetaData;

namespace ProjectManagement
{
    public partial class UCProjectDetailsStatistical : UserControl
    {
        private Team team = new Team();
        private List<Tasks> listTasks = new List<Tasks>();
        private List<Member> members = new List<Member>();
        private List<double> evaluationOfMembers;
        private List<double> scoreOfMembers;

        public UCProjectDetailsStatistical()
        {
            InitializeComponent();
        }
        public void SetUpUserControl(Team team)
        {
            this.team = team;
            this.members = TeamDAO.GetMembersByTeamId(team.TeamId);
            this.members.OrderBy(member => member.Role);
            this.listTasks = TaskDAO.SelectListByTeam(this.team.TeamId);
            UpdateMembers();
            UpdateChart();
            this.gProgressBar.Value = CalculationUtil.CalStatisticalProject(this.listTasks);
            this.lblTotalProgress.Text = this.gProgressBar.Value.ToString() + "%";
        }

        #region MEMBER STATISTICAL

        public void UpdateMembers()
        {
            flpMemberStatistical.Controls.Clear();
            for (int i = 0; i < this.members.Count; i++)
            {
                UCUserMiniLine line = new UCUserMiniLine(this.members[i].User);
                line.SetBackGroundColor(SystemColors.ButtonFace);
                line.SetSize(new Size(580, 63));
                line.SetDeleteMode(false);
                int completion = (int)CalculationUtil.CalCompletionRatePeople(this.members[i].User.UserId, this.listTasks);
                int score = (int)CalculationUtil.CalScorePeople(this.members[i].User.UserId, this.listTasks);
                line.SetStatisticalMode(completion, score);
                flpMemberStatistical.Controls.Add(line);
            }
        }
        #endregion

        #region CHART
        public void UpdateChart()
        {
            this.gSplineAreaDataset.DataPoints.Clear();
            for (int i = 0; i < this.listTasks.Count; i++)
            {
                string name = "Task " + i.ToString();
                this.gSplineAreaDataset.DataPoints.Add(name, this.listTasks[i].Progress);
            }
            this.gChart.Datasets.Clear();
            this.gChart.Datasets.Add(gSplineAreaDataset);
            this.gChart.Update();
        }
        #endregion
    }
}
