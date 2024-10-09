namespace ProjectManagement
{
    partial class UCStatisticalLecture
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.Charts.WinForms.ChartFont chartFont9 = new Guna.Charts.WinForms.ChartFont();
            Guna.Charts.WinForms.ChartFont chartFont10 = new Guna.Charts.WinForms.ChartFont();
            Guna.Charts.WinForms.ChartFont chartFont11 = new Guna.Charts.WinForms.ChartFont();
            Guna.Charts.WinForms.ChartFont chartFont12 = new Guna.Charts.WinForms.ChartFont();
            Guna.Charts.WinForms.Grid grid4 = new Guna.Charts.WinForms.Grid();
            Guna.Charts.WinForms.Tick tick4 = new Guna.Charts.WinForms.Tick();
            Guna.Charts.WinForms.ChartFont chartFont13 = new Guna.Charts.WinForms.ChartFont();
            Guna.Charts.WinForms.Grid grid5 = new Guna.Charts.WinForms.Grid();
            Guna.Charts.WinForms.Tick tick5 = new Guna.Charts.WinForms.Tick();
            Guna.Charts.WinForms.ChartFont chartFont14 = new Guna.Charts.WinForms.ChartFont();
            Guna.Charts.WinForms.Grid grid6 = new Guna.Charts.WinForms.Grid();
            Guna.Charts.WinForms.PointLabel pointLabel2 = new Guna.Charts.WinForms.PointLabel();
            Guna.Charts.WinForms.ChartFont chartFont15 = new Guna.Charts.WinForms.ChartFont();
            Guna.Charts.WinForms.Tick tick6 = new Guna.Charts.WinForms.Tick();
            Guna.Charts.WinForms.ChartFont chartFont16 = new Guna.Charts.WinForms.ChartFont();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            gMixedBarAndSplineChart = new Guna.Charts.WinForms.GunaChart();
            gBarDataset = new Guna.Charts.WinForms.GunaBarDataset();
            gSplineDataset = new Guna.Charts.WinForms.GunaSplineDataset();
            lblProjectsStatistical = new Label();
            gComboBoxSelectYear = new Guna.UI2.WinForms.Guna2ComboBox();
            SuspendLayout();
            // 
            // gMixedBarAndSplineChart
            // 
            gMixedBarAndSplineChart.Datasets.AddRange(new Guna.Charts.Interfaces.IGunaDataset[] { gBarDataset, gSplineDataset });
            chartFont9.FontName = "Arial";
            gMixedBarAndSplineChart.Legend.LabelFont = chartFont9;
            gMixedBarAndSplineChart.Location = new Point(3, 75);
            gMixedBarAndSplineChart.Name = "gMixedBarAndSplineChart";
            gMixedBarAndSplineChart.Size = new Size(744, 204);
            gMixedBarAndSplineChart.TabIndex = 1;
            chartFont10.FontName = "Arial";
            chartFont10.Size = 12;
            chartFont10.Style = Guna.Charts.WinForms.ChartFontStyle.Bold;
            gMixedBarAndSplineChart.Title.Font = chartFont10;
            chartFont11.FontName = "Arial";
            gMixedBarAndSplineChart.Tooltips.BodyFont = chartFont11;
            chartFont12.FontName = "Arial";
            chartFont12.Size = 9;
            chartFont12.Style = Guna.Charts.WinForms.ChartFontStyle.Bold;
            gMixedBarAndSplineChart.Tooltips.TitleFont = chartFont12;
            gMixedBarAndSplineChart.XAxes.GridLines = grid4;
            chartFont13.FontName = "Arial";
            chartFont13.Size = 10;
            tick4.Font = chartFont13;
            gMixedBarAndSplineChart.XAxes.Ticks = tick4;
            gMixedBarAndSplineChart.YAxes.GridLines = grid5;
            chartFont14.FontName = "Arial";
            tick5.Font = chartFont14;
            gMixedBarAndSplineChart.YAxes.Ticks = tick5;
            gMixedBarAndSplineChart.ZAxes.GridLines = grid6;
            chartFont15.FontName = "Arial";
            pointLabel2.Font = chartFont15;
            gMixedBarAndSplineChart.ZAxes.PointLabels = pointLabel2;
            chartFont16.FontName = "Arial";
            tick6.Font = chartFont16;
            gMixedBarAndSplineChart.ZAxes.Ticks = tick6;
            // 
            // gBarDataset
            // 
            gBarDataset.CornerRadius = 5;
            gBarDataset.Label = "Bar";
            gBarDataset.LegendBoxBorderColor = Color.FromArgb(192, 255, 192);
            gBarDataset.LegendBoxFillColor = Color.FromArgb(128, 255, 128);
            gBarDataset.TargetChart = gMixedBarAndSplineChart;
            // 
            // gSplineDataset
            // 
            gSplineDataset.BorderColor = Color.Empty;
            gSplineDataset.FillColor = Color.Empty;
            gSplineDataset.Label = "Spline";
            gSplineDataset.TargetChart = gMixedBarAndSplineChart;
            // 
            // lblProjectsStatistical
            // 
            lblProjectsStatistical.AutoSize = true;
            lblProjectsStatistical.BackColor = Color.Transparent;
            lblProjectsStatistical.Font = new Font("Trebuchet MS", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblProjectsStatistical.Location = new Point(238, 9);
            lblProjectsStatistical.Name = "lblProjectsStatistical";
            lblProjectsStatistical.Size = new Size(349, 28);
            lblProjectsStatistical.TabIndex = 10;
            lblProjectsStatistical.Text = "STATISTICAL PROJECTS IN YEAR";
            // 
            // gComboBoxSelectYear
            // 
            gComboBoxSelectYear.BackColor = Color.Transparent;
            gComboBoxSelectYear.BorderRadius = 10;
            gComboBoxSelectYear.CustomizableEdges = customizableEdges3;
            gComboBoxSelectYear.DrawMode = DrawMode.OwnerDrawFixed;
            gComboBoxSelectYear.DropDownStyle = ComboBoxStyle.DropDownList;
            gComboBoxSelectYear.FocusedColor = Color.FromArgb(94, 148, 255);
            gComboBoxSelectYear.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            gComboBoxSelectYear.Font = new Font("Segoe UI", 10F);
            gComboBoxSelectYear.ForeColor = Color.FromArgb(68, 88, 112);
            gComboBoxSelectYear.ItemHeight = 30;
            gComboBoxSelectYear.Location = new Point(42, 33);
            gComboBoxSelectYear.Name = "gComboBoxSelectYear";
            gComboBoxSelectYear.ShadowDecoration.CustomizableEdges = customizableEdges4;
            gComboBoxSelectYear.Size = new Size(175, 36);
            gComboBoxSelectYear.TabIndex = 9;
            gComboBoxSelectYear.SelectedValueChanged += gComboBoxSelectYear_SelectedValueChanged;
            // 
            // UCStatisticalLecture
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(lblProjectsStatistical);
            Controls.Add(gComboBoxSelectYear);
            Controls.Add(gMixedBarAndSplineChart);
            Name = "UCStatisticalLecture";
            Size = new Size(750, 295);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Guna.Charts.WinForms.GunaChart gMixedBarAndSplineChart;
        private Guna.Charts.WinForms.GunaBarDataset gBarDataset;
        private Guna.Charts.WinForms.GunaSplineDataset gSplineDataset;
        private Label lblProjectsStatistical;
        private Guna.UI2.WinForms.Guna2ComboBox gComboBoxSelectYear;
    }
}
