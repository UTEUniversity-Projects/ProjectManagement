namespace ProjectManagement
{
    partial class UCProjectMiniLine
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            lblProjectTopic = new Label();
            gShadowPanelBack = new Guna.UI2.WinForms.Guna2ShadowPanel();
            gTextBoxStatus = new Guna.UI2.WinForms.Guna2TextBox();
            gShadowPanelBack.SuspendLayout();
            SuspendLayout();
            // 
            // lblProjectTopic
            // 
            lblProjectTopic.AutoSize = true;
            lblProjectTopic.BackColor = Color.White;
            lblProjectTopic.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblProjectTopic.Location = new Point(13, 11);
            lblProjectTopic.Name = "lblProjectTopic";
            lblProjectTopic.Size = new Size(106, 23);
            lblProjectTopic.TabIndex = 14;
            lblProjectTopic.Text = "Project topic";
            // 
            // gShadowPanelBack
            // 
            gShadowPanelBack.BackColor = Color.Transparent;
            gShadowPanelBack.Controls.Add(gTextBoxStatus);
            gShadowPanelBack.FillColor = Color.White;
            gShadowPanelBack.Location = new Point(0, 0);
            gShadowPanelBack.Name = "gShadowPanelBack";
            gShadowPanelBack.Radius = 4;
            gShadowPanelBack.ShadowColor = Color.Black;
            gShadowPanelBack.ShadowShift = 3;
            gShadowPanelBack.ShadowStyle = Guna.UI2.WinForms.Guna2ShadowPanel.ShadowMode.ForwardDiagonal;
            gShadowPanelBack.Size = new Size(345, 50);
            gShadowPanelBack.TabIndex = 16;
            gShadowPanelBack.Click += UCProjectMiniLine_Click;
            // 
            // gTextBoxStatus
            // 
            gTextBoxStatus.BackColor = Color.Transparent;
            gTextBoxStatus.BorderRadius = 10;
            gTextBoxStatus.BorderThickness = 0;
            gTextBoxStatus.CustomizableEdges = customizableEdges3;
            gTextBoxStatus.DefaultText = "Published";
            gTextBoxStatus.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            gTextBoxStatus.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            gTextBoxStatus.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            gTextBoxStatus.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            gTextBoxStatus.FillColor = Color.Gray;
            gTextBoxStatus.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            gTextBoxStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gTextBoxStatus.ForeColor = Color.White;
            gTextBoxStatus.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            gTextBoxStatus.Location = new Point(218, 10);
            gTextBoxStatus.Margin = new Padding(3, 4, 3, 4);
            gTextBoxStatus.Name = "gTextBoxStatus";
            gTextBoxStatus.PasswordChar = '\0';
            gTextBoxStatus.PlaceholderText = "";
            gTextBoxStatus.ReadOnly = true;
            gTextBoxStatus.SelectedText = "";
            gTextBoxStatus.ShadowDecoration.CustomizableEdges = customizableEdges4;
            gTextBoxStatus.Size = new Size(110, 25);
            gTextBoxStatus.TabIndex = 60;
            gTextBoxStatus.TextAlign = HorizontalAlignment.Center;
            // 
            // UCProjectMiniLine
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonFace;
            Controls.Add(lblProjectTopic);
            Controls.Add(gShadowPanelBack);
            DoubleBuffered = true;
            Name = "UCProjectMiniLine";
            Size = new Size(350, 50);
            Click += UCProjectMiniLine_Click;
            gShadowPanelBack.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label lblProjectTopic;
        private Guna.UI2.WinForms.Guna2ShadowPanel gShadowPanelBack;
        private Guna.UI2.WinForms.Guna2TextBox gTextBoxStatus;
    }
}
