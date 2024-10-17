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
        
        private List<Project> listTheses;

        private ProjectDAO ProjectDAO = new ProjectDAO();

        public UCDashboardStatistics()
        {
            InitializeComponent();
            SetupFlpStatus();
        }

        #region FUNCTIONS

        public void SetInformation(List<Project> listTheses)
        {
            this.listTheses = listTheses;
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
            foreach (var status in statusList)
            {
                Guna2Button button = new Guna2Button();
                button.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
                button.Text = status.ToString();
                button.ForeColor = Color.White;
                button.Size = new Size(105, 25);
                button.BorderRadius = 5;
                button.FillColor = ModelUtil.GetProjectStatusColor(status);
                this.flpStatus.Controls.Add(button);
            }
        }

        #endregion

        #region DOUGHNUT CHART
        public void UpdateDoughnutChart()
        {
            this.lblTotal.Text = this.listTheses.Count.ToString();
            var projectGroupedByStatus = this.listTheses
            .GroupBy(project => project.Status)
            .Select(group => new
            {
                Status = group.Key,
                Count = group.Count(),
            });
            this.gDoughnutChart.Datasets.Clear();
            foreach (var group in projectGroupedByStatus)
            {
                int ind = ModelUtil.GetProjectStatusIndex(group.Status);
                this.gDoughnutDataset.DataPoints[ind].Y = group.Count;
                this.gDoughnutDataset.FillColors[ind] = ModelUtil.GetProjectStatusColor(group.Status);
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
        
        IEnumerable<object> ByField()
        {
            var projectGroupedByField = this.listTheses
              .GroupBy(project => project.FieldId)
              .Select(group => new
              {
                  Name = group.Key,
                  Count = group.Count()
              });
            projectGroupedByField = projectGroupedByField.OrderByDescending(item => item.Count);
            return projectGroupedByField;
        }
        public void UpdateHorizontalbarChart()
        {
            string selectedFilter = gComboBoxTop.SelectedItem.ToString();
            var projectGroupedByField = ByField();
            int max = 5;
            int i = 0;
            this.gHorizontalBarDataset.DataPoints.Clear();
            this.gHorizontalBarChart.Datasets.Clear();
            foreach (var group in projectGroupedByField)
            {
                var name = group.GetType().GetProperty("Name").GetValue(group, null).ToString();
                if (selectedFilter == "Lecture")
                {
                    Users user = UserDAO.SelectOnlyByID(name);
                    name = user.UserName;
                }
                var count = (int)group.GetType().GetProperty("Count").GetValue(group, null);
                this.gHorizontalBarDataset.DataPoints.Add(name, count);
                i++;
                if (i == max) break;
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
            var projectGroupedByMonth = allMonths
                .GroupJoin(this.listTheses,
                           month => month,
                           project => project.PublicDate.Month,
                           (month, theses) => new
                           {
                               Month = month,
                               Count = theses.Where(project => project.PublicDate.Year == selectedYear).Count()
                           })
                .Select(result => new
                {
                    result.Month,
                    result.Count
                });
            CultureInfo culture = CultureInfo.InvariantCulture;
            DateTimeFormatInfo dtfi = culture.DateTimeFormat;
            string monthName;
            this.gSplineDataset.DataPoints.Clear();
            this.gBarDataset.DataPoints.Clear();
            this.gMixedBarAndSplineChart.Datasets.Clear();
            foreach (var group in projectGroupedByMonth)
            {
                monthName = dtfi.GetMonthName(group.Month);
                this.gSplineDataset.DataPoints.Add(monthName, group.Count);
                this.gBarDataset.DataPoints.Add(monthName, group.Count);
            }
            this.gMixedBarAndSplineChart.Datasets.Add(gBarDataset);
            this.gMixedBarAndSplineChart.Datasets.Add(gSplineDataset);
            this.gMixedBarAndSplineChart.Update();
        }
        #endregion

    }
}
