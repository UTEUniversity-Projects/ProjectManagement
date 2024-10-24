using System.Data;
using ProjectManagement.DAOs;
using ProjectManagement.Models;
using ProjectManagement.Enums;
using System.Globalization;
using Guna.UI2.WinForms;
using ProjectManagement.Process;
using ProjectManagement.Utils;

namespace ProjectManagement
{
    public partial class UCDashboardStatistics : UserControl
    {
        
        private List<Project> listProjects;

        public UCDashboardStatistics()
        {
            InitializeComponent();
            SetupFlpStatus();
        }

        #region FUNCTIONS

        public void SetInformation(List<Project> listTheses)
        {
            this.listProjects = listTheses;
            SetupUserControl();
        }
        void SetupUserControl()
        {
            UpdateDoughnutChart();
            SetupComboBoxTop();
            SetupComboBoxSelectYear();
        }

        #endregion

        #region FLOW PANEL STATUS

        void SetupFlpStatus()
        {
            List<EProjectStatus> statusList = new List<EProjectStatus>((EProjectStatus[])Enum.GetValues(typeof(EProjectStatus)));
            statusList = statusList
            .OrderBy(status => ModelUtil.GetProjectStatusIndex(status))
            .ToList();
            foreach (var status in statusList)
            {
                if (status == EProjectStatus.WAITING) continue;
                Guna2Button button = new Guna2Button();
                button.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold, GraphicsUnit.Point, 0);
                button.Text = status.ToString();
                button.ForeColor = Color.White;
                button.Size = new Size(115, 28);
                button.BorderRadius = 8;
                button.FillColor = ModelUtil.GetProjectStatusColor(status);
                this.flpStatus.Controls.Add(button);
            }
        }

        #endregion

        #region DOUGHNUT CHART
        public void UpdateDoughnutChart()
        {
            this.lblTotal.Text = this.listProjects.Count.ToString();
            var projectGroupedByStatus = ProjectDAO.GroupedByStatus(this.listProjects);
            this.gDoughnutChart.Datasets.Clear();
            foreach (var group in projectGroupedByStatus)
            {
                EProjectStatus status = group.Key;
                int count = group.Value;
                int ind = ModelUtil.GetProjectStatusIndex(status);
                this.gDoughnutDataset.DataPoints[ind].Y = count;
                this.gDoughnutDataset.FillColors[ind] = ModelUtil.GetProjectStatusColor(status);
            }
            this.gDoughnutChart.Datasets.Add(gDoughnutDataset);
            this.gDoughnutChart.Update();
        }
        #endregion

        #region COMBO BOX
        public void SetupComboBoxSelectYear()
        {
            int currentYear = DateTime.Now.Year;
            List<int> recentYears = new List<int> { currentYear, currentYear - 1, currentYear - 2 };

            this.gComboBoxSelectYear.DataSource = recentYears;
            this.gComboBoxSelectYear.SelectedIndex = 0;
        }
        private void gComboBoxSelectYear_SelectedValueChanged(object sender, EventArgs e)
        {
            UpdateMixedBarAndSplineChart();
        }
        public void SetupComboBoxTop()
        {
            this.gComboBoxTop.SelectedIndex = 0;
        }
        private void gComboBoxTop_SelectedValueChanged(object sender, EventArgs e)
        {
            UpdateHorizontalbarChart();
        }
        #endregion

        #region HORIZONTALBAR CHART
        public void UpdateHorizontalbarChart()
        {
            string selectedFilter = gComboBoxTop.SelectedItem.ToString();
            Dictionary<string, int> groups;
            if (selectedFilter == "Field")
            {
                lblTop.Text = "Top 5 Field";
                groups = FieldDAO.TopField();
                //groups = ProjectDAO.GetTopField(this.listProjects);
            } 
            else
            {
                lblTop.Text = "Top 5 Technology";
                groups = TechnologyDAO.TopTechnology();
            }
            this.gHorizontalBarDataset.DataPoints.Clear();
            this.gHorizontalBarChart.Datasets.Clear();
            foreach (var group in groups)
            {
                var name = group.Key;
                var count = group.Value;
                this.gHorizontalBarDataset.DataPoints.Add(name, count);
            }
            this.gHorizontalBarChart.Datasets.Add(gHorizontalBarDataset);
            this.gHorizontalBarChart.Update();
        }
        #endregion

        #region MIXED BAR AND SPLINE CHART
        public void UpdateMixedBarAndSplineChart()
        {
            var allMonths = Enumerable.Range(1, 12);
            int selectedYear = (int)gComboBoxSelectYear.SelectedItem;
            var filterProjects = this.listProjects
                                .Where(project => project.CreatedAt.Year == selectedYear)
                                .ToList();
            var projectGroupedByMonth = ProjectDAO.GroupedByMonth(filterProjects);

            this.gSplineDataset.DataPoints.Clear();
            this.gBarDataset.DataPoints.Clear();
            this.gMixedBarAndSplineChart.Datasets.Clear();
            foreach (var group in projectGroupedByMonth)
            {
                string monthName = group.Key;
                int count = group.Value;
                this.gSplineDataset.DataPoints.Add(monthName, count);
                this.gBarDataset.DataPoints.Add(monthName, count);
            }
            this.gMixedBarAndSplineChart.Datasets.Add(gBarDataset);
            this.gMixedBarAndSplineChart.Datasets.Add(gSplineDataset);
            this.gMixedBarAndSplineChart.Update();
        }
        #endregion

    }
}
