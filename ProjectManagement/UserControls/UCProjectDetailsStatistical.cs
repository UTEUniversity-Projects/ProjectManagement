﻿using System;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ProjectManagement
{
    public partial class UCProjectDetailsStatistical : UserControl
    {
        private Team team = new Team();
        private List<Tasks> listTasks = new List<Tasks>();
        private List<Student> members = new List<Student>();
        private List<double> evaluationOfMembers;
        private List<double> scoreOfMembers;

        private TaskDAO TaskDAO = new TaskDAO();
        

        public UCProjectDetailsStatistical()
        {
            InitializeComponent();
        }
        public void SetUpUserControl(Team team)
        {
            this.team = team;
            this.members = TeamDAO.GetMembersByTeamId(team.TeamId);
            this.members.OrderBy(member => member.UserId);
            this.listTasks = TaskDAO.SelectListByTeam(this.team.TeamId);
            UpdateMembers();
            UpdateChart();
            this.gProgressBar.Value = CalculationUtil.CalStatisticalProject(this.listTasks);
            this.lblTotalProgress.Text = this.gProgressBar.Value.ToString() + "%";
        }

        #region MEMBER STATISTICAL

        public void UpdateMembers()
        {
            this.evaluationOfMembers = CalculationUtil.CalEvaluations(this.listTasks, this.members.Count(), evaluation => evaluation.CompletionRate);
            this.scoreOfMembers = CalculationUtil.CalEvaluations(this.listTasks, this.members.Count(), evaluation => evaluation.Score);
            flpMemberStatistical.Controls.Clear();
            for (int i = 0; i < this.members.Count; i++)
            {
                UCUserMiniLine line = new UCUserMiniLine(this.members[i]);
                line.SetBackGroundColor(SystemColors.ButtonFace);
                line.SetSize(new Size(580, 63));
                line.SetDeleteMode(false);
                line.SetStatisticalMode((int)this.evaluationOfMembers[i], this.scoreOfMembers[i]);
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
