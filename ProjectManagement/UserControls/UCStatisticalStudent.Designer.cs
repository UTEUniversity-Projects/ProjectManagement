namespace ProjectManagement
{
    partial class UCStatisticalStudent
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCStatisticalStudent));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            gLineDataset = new Guna.Charts.WinForms.GunaLineDataset();
            lblAcademicChart = new Label();
            gChart = new Guna.Charts.WinForms.GunaChart();
            gShadowPanelProject = new Guna.UI2.WinForms.Guna2ShadowPanel();
            lblNoteStatisProjects = new Label();
            gPictureBoxItemProject = new Guna.UI2.WinForms.Guna2PictureBox();
            lblNumProject = new Label();
            gShadowPanelProject.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gPictureBoxItemProject).BeginInit();
            SuspendLayout();
            // 
            // gLineDataset
            // 
            gLineDataset.BorderColor = Color.Empty;
            gLineDataset.BorderWidth = 0;
            gLineDataset.FillColor = Color.Empty;
            gLineDataset.Label = "Score";
            gLineDataset.LegendBoxBorderColor = Color.Transparent;
            gLineDataset.LegendBoxFillColor = Color.FromArgb(255, 255, 192);
            // 
            // lblAcademicChart
            // 
            lblAcademicChart.AutoSize = true;
            lblAcademicChart.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAcademicChart.Location = new Point(288, 50);
            lblAcademicChart.Name = "lblAcademicChart";
            lblAcademicChart.Size = new Size(214, 31);
            lblAcademicChart.TabIndex = 35;
            lblAcademicChart.Text = "ACADEMIC CHART";
            // 
            // gChart
            // 
            gChart.Datasets.AddRange(new Guna.Charts.Interfaces.IGunaDataset[] { gLineDataset });
            gChart.Legend.Display = false;
            chartFont9.FontName = "Arial";
            gChart.Legend.LabelFont = chartFont9;
            gChart.Location = new Point(8, 86);
            gChart.Name = "gChart";
            gChart.Size = new Size(735, 206);
            gChart.TabIndex = 34;
            chartFont10.FontName = "Arial";
            chartFont10.Size = 12;
            chartFont10.Style = Guna.Charts.WinForms.ChartFontStyle.Bold;
            gChart.Title.Font = chartFont10;
            chartFont11.FontName = "Arial";
            gChart.Tooltips.BodyFont = chartFont11;
            chartFont12.FontName = "Arial";
            chartFont12.Size = 9;
            chartFont12.Style = Guna.Charts.WinForms.ChartFontStyle.Bold;
            gChart.Tooltips.TitleFont = chartFont12;
            gChart.XAxes.GridLines = grid4;
            chartFont13.FontName = "Arial";
            tick4.Font = chartFont13;
            gChart.XAxes.Ticks = tick4;
            gChart.YAxes.GridLines = grid5;
            chartFont14.FontName = "Arial";
            tick5.Font = chartFont14;
            tick5.HasMaximum = true;
            tick5.HasMinimum = true;
            tick5.Maximum = 10D;
            gChart.YAxes.Ticks = tick5;
            gChart.ZAxes.GridLines = grid6;
            chartFont15.FontName = "Arial";
            pointLabel2.Font = chartFont15;
            gChart.ZAxes.PointLabels = pointLabel2;
            chartFont16.FontName = "Arial";
            tick6.Font = chartFont16;
            gChart.ZAxes.Ticks = tick6;
            // 
            // gShadowPanelProject
            // 
            gShadowPanelProject.BackColor = Color.Transparent;
            gShadowPanelProject.Controls.Add(lblNoteStatisProjects);
            gShadowPanelProject.Controls.Add(gPictureBoxItemProject);
            gShadowPanelProject.Controls.Add(lblNumProject);
            gShadowPanelProject.FillColor = Color.White;
            gShadowPanelProject.Location = new Point(36, 10);
            gShadowPanelProject.Name = "gShadowPanelProject";
            gShadowPanelProject.Radius = 7;
            gShadowPanelProject.ShadowColor = Color.Black;
            gShadowPanelProject.ShadowShift = 4;
            gShadowPanelProject.Size = new Size(227, 70);
            gShadowPanelProject.TabIndex = 33;
            // 
            // lblNoteStatisProjects
            // 
            lblNoteStatisProjects.AutoSize = true;
            lblNoteStatisProjects.Font = new Font("Verdana", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblNoteStatisProjects.ForeColor = Color.Gray;
            lblNoteStatisProjects.Location = new Point(74, 40);
            lblNoteStatisProjects.Name = "lblNoteStatisProjects";
            lblNoteStatisProjects.Size = new Size(143, 17);
            lblNoteStatisProjects.TabIndex = 9;
            lblNoteStatisProjects.Text = "completed projects";
            // 
            // gPictureBoxItemProject
            // 
            gPictureBoxItemProject.CustomizableEdges = customizableEdges3;
            gPictureBoxItemProject.FillColor = Color.Transparent;
            gPictureBoxItemProject.Image = (Image)resources.GetObject("gPictureBoxItemProject.Image");
            gPictureBoxItemProject.ImageRotate = 0F;
            gPictureBoxItemProject.Location = new Point(17, 7);
            gPictureBoxItemProject.Name = "gPictureBoxItemProject";
            gPictureBoxItemProject.ShadowDecoration.CustomizableEdges = customizableEdges4;
            gPictureBoxItemProject.Size = new Size(50, 50);
            gPictureBoxItemProject.SizeMode = PictureBoxSizeMode.StretchImage;
            gPictureBoxItemProject.TabIndex = 10;
            gPictureBoxItemProject.TabStop = false;
            // 
            // lblNumProject
            // 
            lblNumProject.AutoSize = true;
            lblNumProject.Font = new Font("Trebuchet MS", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNumProject.Location = new Point(78, 7);
            lblNumProject.Name = "lblNumProject";
            lblNumProject.Size = new Size(44, 32);
            lblNumProject.TabIndex = 11;
            lblNumProject.Text = "24";
            // 
            // UCStatisticalStudent
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(lblAcademicChart);
            Controls.Add(gChart);
            Controls.Add(gShadowPanelProject);
            Name = "UCStatisticalStudent";
            Size = new Size(750, 295);
            gShadowPanelProject.ResumeLayout(false);
            gShadowPanelProject.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gPictureBoxItemProject).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Guna.Charts.WinForms.GunaLineDataset gLineDataset;
        private Label lblAcademicChart;
        private Guna.Charts.WinForms.GunaChart gChart;
        private Guna.UI2.WinForms.Guna2ShadowPanel gShadowPanelProject;
        private Label lblNoteStatisProjects;
        private Guna.UI2.WinForms.Guna2PictureBox gPictureBoxItemProject;
        private Label lblNumProject;
    }
}
