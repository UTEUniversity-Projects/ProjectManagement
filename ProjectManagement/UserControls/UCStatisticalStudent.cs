using Guna.UI2.WinForms;
using Guna.UI2.WinForms.Suite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjectManagement.DAOs;
using ProjectManagement.Models;
using ProjectManagement.Process;

namespace ProjectManagement
{
    public partial class UCStatisticalStudent : UserControl
    {
        private Users user = new Users();
        private List<Project> listProject;
        private double avgContribute;        

        public UCStatisticalStudent()
        {
            InitializeComponent();
        }

        #region FUNTIONS
        public double AvgContribute { get => this.avgContribute; }
        public void SetInformation(Users user)
        {
            this.user = user;
            SetupUserControl();
        }
        void SetupUserControl() 
        {
            this.listProject = ProjectDAO.SelectListModeMyCompletedProjects(user.UserId);
            this.lblNumProject.Text = this.listProject.Count.ToString();
            this.avgContribute = 0;

            SetupChart();
        }
        void SetupChart()
        {
            List<Tasks> listTasks;
            this.gLineDataset.DataPoints.Clear();
            this.gChart.Datasets.Clear();
            foreach (Project project in this.listProject)
            {
                listTasks = TaskDAO.SelectListTaskByStudent(this.user.UserId);

                double score = CalculationUtil.CalScorePeople(this.user.UserId, listTasks);
                double contribute = CalculationUtil.CalCompletionRatePeople(this.user.UserId, listTasks);
                this.avgContribute += contribute;
                this.gLineDataset.DataPoints.Add(project.ProjectId, score);
            }
            this.gChart.Datasets.Add(this.gLineDataset);
            this.gChart.Update();
        }

        #endregion
    }
}
