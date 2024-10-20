using Guna.UI2.WinForms;
using Guna.UI2.WinForms.Suite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjectManagement.DAOs;
using ProjectManagement.Models;

namespace ProjectManagement
{
    public partial class UCStatisticalLecture : UserControl
    {
        private Users user = new Users();
        private List<Project> listProject;
        private ProjectDAO ProjectDAO = new ProjectDAO();
        public UCStatisticalLecture()
        {
            InitializeComponent();
        }
        public void SetInformation(Users user)
        {
            this.user = user;
            this.listProject = ProjectDAO.SelectListRoleLecture(this.user.UserId);
            SetUserControl();
        }
        void SetUserControl() 
        {
            SetupComboBoxSelectYear();
        }

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
        #endregion

        #region MIXED BAR AND SPLINE CHART
        public void UpdateMixedBarAndSplineChart()
        {
            var allMonths = Enumerable.Range(1, 12);
            int selectedYear = (int)gComboBoxSelectYear.SelectedItem;
            var projectGroupedByMonth = allMonths
                .GroupJoin(this.listProject,
                           month => month,
                           project => project.CreatedAt.Month,
                           (month, theses) => new
                           {
                               Month = month,
                               Count = theses.Where(project => project.CreatedAt.Year == selectedYear).Count()
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
