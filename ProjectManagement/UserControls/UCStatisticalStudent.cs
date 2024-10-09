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
        private User people = new User();
        private List<Project> listThesis;
        private double avgContribute;
        private ProjectDAO thesisDAO = new ProjectDAO();
        private ProjectStatusDAO thesisStatusDAO = new ProjectStatusDAO();
        private TasksDAO tasksDAO = new TasksDAO();
        private MyProcess myProcess = new MyProcess();

        public UCStatisticalStudent()
        {
            InitializeComponent();
        }

        #region FUNTIONS
        public double AvgContribute { get => this.avgContribute; }
        public void SetInformation(User people)
        {
            this.people = people;
            SetupUserControl();
        }
        void SetupUserControl() 
        {
            this.listThesis = thesisDAO.SelectListModeMyCompletedTheses(people.IdAccount);
            this.lblNumThesis.Text = this.listThesis.Count.ToString();
            this.avgContribute = 0;

            SetupChart();
        }
        void SetupChart()
        {
            List<Tasks> listTasks;
            string idTeam;
            this.gLineDataset.DataPoints.Clear();
            this.gChart.Datasets.Clear();
            foreach (Project thesis in this.listThesis)
            {
                idTeam = thesisStatusDAO.SelectTeamByIdThesis(thesis.IdThesis);
                listTasks = tasksDAO.SelectListByTeam(idTeam);

                double score = myProcess.CalEvaluations(listTasks, 1, evaluation => evaluation.Scores)[0];
                double contibutute = myProcess.CalEvaluations(listTasks, 1, evaluation => evaluation.Contribute)[0];

                this.avgContribute += contibutute;

                this.gLineDataset.DataPoints.Add(thesis.IdThesis, score);
            }

            this.gChart.Datasets.Add(this.gLineDataset);
            this.gChart.Update();
        }

        #endregion
    }
}
