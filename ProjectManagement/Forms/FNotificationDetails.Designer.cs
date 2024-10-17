namespace ProjectManagement.Forms
{
    partial class FNotificationDetails
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FNotificationDetails));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            gShadowPanelReason = new Guna.UI2.WinForms.Guna2ShadowPanel();
            flpTeam = new FlowLayoutPanel();
            lblTitle = new Label();
            gSeparatorTopic = new Guna.UI2.WinForms.Guna2Separator();
            gTextBoxContent = new Guna.UI2.WinForms.Guna2TextBox();
            lblTi = new Label();
            lblTime = new Label();
            gTextBoxType = new Guna.UI2.WinForms.Guna2TextBox();
            gButtonStar = new Guna.UI2.WinForms.Guna2Button();
            gShadowPanelReason.SuspendLayout();
            flpTeam.SuspendLayout();
            SuspendLayout();
            // 
            // gShadowPanelReason
            // 
            gShadowPanelReason.BackColor = Color.Transparent;
            gShadowPanelReason.Controls.Add(flpTeam);
            gShadowPanelReason.Controls.Add(lblTi);
            gShadowPanelReason.Controls.Add(lblTime);
            gShadowPanelReason.Controls.Add(gTextBoxType);
            gShadowPanelReason.Controls.Add(gButtonStar);
            gShadowPanelReason.FillColor = Color.White;
            gShadowPanelReason.Location = new Point(29, 14);
            gShadowPanelReason.Name = "gShadowPanelReason";
            gShadowPanelReason.Radius = 7;
            gShadowPanelReason.ShadowColor = Color.Black;
            gShadowPanelReason.Size = new Size(772, 595);
            gShadowPanelReason.TabIndex = 44;
            // 
            // flpTeam
            // 
            flpTeam.AutoScroll = true;
            flpTeam.BackColor = Color.White;
            flpTeam.Controls.Add(lblTitle);
            flpTeam.Controls.Add(gSeparatorTopic);
            flpTeam.Controls.Add(gTextBoxContent);
            flpTeam.Location = new Point(30, 66);
            flpTeam.Name = "flpTeam";
            flpTeam.Size = new Size(710, 500);
            flpTeam.TabIndex = 56;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Trebuchet MS", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.Location = new Point(3, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(191, 28);
            lblTitle.TabIndex = 54;
            lblTitle.Text = "Notification title";
            // 
            // gSeparatorTopic
            // 
            gSeparatorTopic.Location = new Point(3, 31);
            gSeparatorTopic.Name = "gSeparatorTopic";
            gSeparatorTopic.Size = new Size(702, 12);
            gSeparatorTopic.TabIndex = 55;
            // 
            // gTextBoxContent
            // 
            gTextBoxContent.AutoScroll = true;
            gTextBoxContent.BorderColor = Color.FromArgb(74, 97, 94);
            gTextBoxContent.BorderRadius = 5;
            gTextBoxContent.CustomizableEdges = customizableEdges1;
            gTextBoxContent.DefaultText = "";
            gTextBoxContent.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            gTextBoxContent.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            gTextBoxContent.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            gTextBoxContent.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            gTextBoxContent.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            gTextBoxContent.Font = new Font("Segoe UI", 9F);
            gTextBoxContent.ForeColor = Color.Black;
            gTextBoxContent.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            gTextBoxContent.IconLeft = (Image)resources.GetObject("gTextBoxContent.IconLeft");
            gTextBoxContent.IconLeftOffset = new Point(5, 0);
            gTextBoxContent.IconLeftSize = new Size(22, 22);
            gTextBoxContent.Location = new Point(3, 50);
            gTextBoxContent.Margin = new Padding(3, 4, 3, 4);
            gTextBoxContent.Multiline = true;
            gTextBoxContent.Name = "gTextBoxContent";
            gTextBoxContent.PasswordChar = '\0';
            gTextBoxContent.PlaceholderForeColor = Color.Gray;
            gTextBoxContent.PlaceholderText = "notification content";
            gTextBoxContent.ReadOnly = true;
            gTextBoxContent.SelectedText = "";
            gTextBoxContent.ShadowDecoration.CustomizableEdges = customizableEdges2;
            gTextBoxContent.Size = new Size(702, 250);
            gTextBoxContent.TabIndex = 56;
            gTextBoxContent.TextOffset = new Point(5, 0);
            // 
            // lblTi
            // 
            lblTi.AutoSize = true;
            lblTi.BackColor = Color.Transparent;
            lblTi.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTi.ForeColor = Color.FromArgb(74, 97, 94);
            lblTi.Location = new Point(214, 26);
            lblTi.Name = "lblTi";
            lblTi.Size = new Size(50, 20);
            lblTi.TabIndex = 49;
            lblTi.Text = "Time :";
            // 
            // lblTime
            // 
            lblTime.AutoSize = true;
            lblTime.BackColor = Color.Transparent;
            lblTime.Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTime.ForeColor = Color.Black;
            lblTime.Location = new Point(263, 26);
            lblTime.Name = "lblTime";
            lblTime.Size = new Size(147, 20);
            lblTime.TabIndex = 48;
            lblTime.Text = "12/04/2024 21:34:55";
            // 
            // gTextBoxType
            // 
            gTextBoxType.BackColor = Color.Transparent;
            gTextBoxType.BorderRadius = 9;
            gTextBoxType.BorderThickness = 0;
            gTextBoxType.CustomizableEdges = customizableEdges3;
            gTextBoxType.DefaultText = "Comment";
            gTextBoxType.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            gTextBoxType.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            gTextBoxType.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            gTextBoxType.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            gTextBoxType.FillColor = Color.Gray;
            gTextBoxType.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            gTextBoxType.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gTextBoxType.ForeColor = Color.White;
            gTextBoxType.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            gTextBoxType.Location = new Point(89, 23);
            gTextBoxType.Margin = new Padding(3, 4, 3, 4);
            gTextBoxType.Name = "gTextBoxType";
            gTextBoxType.PasswordChar = '\0';
            gTextBoxType.PlaceholderText = "";
            gTextBoxType.ReadOnly = true;
            gTextBoxType.SelectedText = "";
            gTextBoxType.ShadowDecoration.CustomizableEdges = customizableEdges4;
            gTextBoxType.Size = new Size(110, 25);
            gTextBoxType.TabIndex = 47;
            gTextBoxType.TextAlign = HorizontalAlignment.Center;
            // 
            // gButtonStar
            // 
            gButtonStar.BackColor = Color.Transparent;
            gButtonStar.BorderRadius = 5;
            gButtonStar.CustomizableEdges = customizableEdges5;
            gButtonStar.DisabledState.BorderColor = Color.DarkGray;
            gButtonStar.DisabledState.CustomBorderColor = Color.DarkGray;
            gButtonStar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            gButtonStar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            gButtonStar.FillColor = Color.Transparent;
            gButtonStar.Font = new Font("Segoe UI", 9F);
            gButtonStar.ForeColor = Color.White;
            gButtonStar.Image = (Image)resources.GetObject("gButtonStar.Image");
            gButtonStar.ImageSize = new Size(25, 25);
            gButtonStar.Location = new Point(34, 16);
            gButtonStar.Name = "gButtonStar";
            gButtonStar.PressedColor = SystemColors.ButtonFace;
            gButtonStar.ShadowDecoration.CustomizableEdges = customizableEdges6;
            gButtonStar.Size = new Size(40, 40);
            gButtonStar.TabIndex = 13;
            // 
            // FNotificationDetails
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonFace;
            ClientSize = new Size(832, 643);
            Controls.Add(gShadowPanelReason);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FNotificationDetails";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Notification Details";
            gShadowPanelReason.ResumeLayout(false);
            gShadowPanelReason.PerformLayout();
            flpTeam.ResumeLayout(false);
            flpTeam.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2ShadowPanel gShadowPanelReason;
        private Guna.UI2.WinForms.Guna2Button gButtonStar;
        private Guna.UI2.WinForms.Guna2TextBox gTextBoxType;
        private Label lblTi;
        private Label lblTime;
        private FlowLayoutPanel flpTeam;
        private Label lblTitle;
        private Guna.UI2.WinForms.Guna2Separator gSeparatorTopic;
        private Guna.UI2.WinForms.Guna2TextBox gTextBoxContent;
    }
}