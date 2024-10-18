namespace ProjectManagement
{
    partial class UCUserMiniLine
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCUserMiniLine));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges17 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges18 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            lblUserCode = new Label();
            lblUserName = new Label();
            gCirclePictureBoxAvatar = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            gShadowPanelBack = new Guna.UI2.WinForms.Guna2ShadowPanel();
            gButtonComplete = new Guna.UI2.WinForms.Guna2Button();
            gProgressBarToLine = new Guna.UI2.WinForms.Guna2ProgressBar();
            gButtonAdd = new Guna.UI2.WinForms.Guna2Button();
            lblMemRole = new Label();
            gPictureBoxMemRole = new Guna.UI2.WinForms.Guna2PictureBox();
            ((System.ComponentModel.ISupportInitialize)gCirclePictureBoxAvatar).BeginInit();
            gShadowPanelBack.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gPictureBoxMemRole).BeginInit();
            SuspendLayout();
            // 
            // lblUserCode
            // 
            lblUserCode.AutoSize = true;
            lblUserCode.BackColor = Color.White;
            lblUserCode.Font = new Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblUserCode.ForeColor = Color.FromArgb(74, 97, 94);
            lblUserCode.Location = new Point(71, 30);
            lblUserCode.Name = "lblUserCode";
            lblUserCode.Size = new Size(71, 17);
            lblUserCode.TabIndex = 27;
            lblUserCode.Text = "243300001";
            // 
            // lblUserName
            // 
            lblUserName.AutoSize = true;
            lblUserName.BackColor = Color.White;
            lblUserName.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblUserName.Location = new Point(69, 6);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(90, 23);
            lblUserName.TabIndex = 26;
            lblUserName.Text = "UserName";
            // 
            // gCirclePictureBoxAvatar
            // 
            gCirclePictureBoxAvatar.BackColor = Color.White;
            gCirclePictureBoxAvatar.Image = (Image)resources.GetObject("gCirclePictureBoxAvatar.Image");
            gCirclePictureBoxAvatar.ImageRotate = 0F;
            gCirclePictureBoxAvatar.Location = new Point(14, 5);
            gCirclePictureBoxAvatar.Name = "gCirclePictureBoxAvatar";
            gCirclePictureBoxAvatar.ShadowDecoration.CustomizableEdges = customizableEdges10;
            gCirclePictureBoxAvatar.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            gCirclePictureBoxAvatar.Size = new Size(45, 45);
            gCirclePictureBoxAvatar.SizeMode = PictureBoxSizeMode.StretchImage;
            gCirclePictureBoxAvatar.TabIndex = 25;
            gCirclePictureBoxAvatar.TabStop = false;
            gCirclePictureBoxAvatar.Click += gCirclePictureBoxAvatar_Click;
            // 
            // gShadowPanelBack
            // 
            gShadowPanelBack.BackColor = Color.Transparent;
            gShadowPanelBack.Controls.Add(gPictureBoxMemRole);
            gShadowPanelBack.Controls.Add(lblMemRole);
            gShadowPanelBack.Controls.Add(gButtonComplete);
            gShadowPanelBack.Controls.Add(gProgressBarToLine);
            gShadowPanelBack.Controls.Add(gButtonAdd);
            gShadowPanelBack.FillColor = Color.White;
            gShadowPanelBack.Location = new Point(3, 0);
            gShadowPanelBack.Name = "gShadowPanelBack";
            gShadowPanelBack.Radius = 5;
            gShadowPanelBack.ShadowColor = Color.Black;
            gShadowPanelBack.ShadowShift = 4;
            gShadowPanelBack.ShadowStyle = Guna.UI2.WinForms.Guna2ShadowPanel.ShadowMode.ForwardDiagonal;
            gShadowPanelBack.Size = new Size(800, 60);
            gShadowPanelBack.TabIndex = 28;
            gShadowPanelBack.Click += gShadowPanelBack_Click;
            gShadowPanelBack.MouseEnter += gShadowPanelBack_MouseEnter;
            gShadowPanelBack.MouseLeave += gShadowPanelBack_MouseLeave;
            // 
            // gButtonComplete
            // 
            gButtonComplete.BackColor = Color.White;
            gButtonComplete.BorderRadius = 5;
            gButtonComplete.CustomizableEdges = customizableEdges13;
            gButtonComplete.DisabledState.BorderColor = Color.White;
            gButtonComplete.DisabledState.CustomBorderColor = Color.White;
            gButtonComplete.DisabledState.FillColor = Color.White;
            gButtonComplete.DisabledState.ForeColor = Color.White;
            gButtonComplete.FillColor = Color.White;
            gButtonComplete.Font = new Font("Segoe UI", 9F);
            gButtonComplete.ForeColor = Color.White;
            gButtonComplete.HoverState.FillColor = Color.White;
            gButtonComplete.HoverState.Image = (Image)resources.GetObject("resource.Image");
            gButtonComplete.Image = (Image)resources.GetObject("gButtonComplete.Image");
            gButtonComplete.ImageSize = new Size(25, 25);
            gButtonComplete.Location = new Point(547, 9);
            gButtonComplete.Name = "gButtonComplete";
            gButtonComplete.PressedColor = Color.White;
            gButtonComplete.ShadowDecoration.CustomizableEdges = customizableEdges14;
            gButtonComplete.Size = new Size(35, 35);
            gButtonComplete.TabIndex = 61;
            // 
            // gProgressBarToLine
            // 
            gProgressBarToLine.BorderRadius = 8;
            gProgressBarToLine.CustomizableEdges = customizableEdges15;
            gProgressBarToLine.Location = new Point(339, 17);
            gProgressBarToLine.Name = "gProgressBarToLine";
            gProgressBarToLine.ProgressColor = Color.FromArgb(94, 148, 255);
            gProgressBarToLine.ProgressColor2 = Color.FromArgb(255, 77, 165);
            gProgressBarToLine.ShadowDecoration.CustomizableEdges = customizableEdges16;
            gProgressBarToLine.Size = new Size(200, 20);
            gProgressBarToLine.TabIndex = 60;
            gProgressBarToLine.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            gProgressBarToLine.Value = 75;
            // 
            // gButtonAdd
            // 
            gButtonAdd.BackColor = Color.White;
            gButtonAdd.BorderRadius = 5;
            gButtonAdd.CustomizableEdges = customizableEdges17;
            gButtonAdd.DisabledState.BorderColor = Color.White;
            gButtonAdd.DisabledState.CustomBorderColor = Color.White;
            gButtonAdd.DisabledState.FillColor = Color.White;
            gButtonAdd.DisabledState.ForeColor = Color.White;
            gButtonAdd.FillColor = Color.White;
            gButtonAdd.Font = new Font("Segoe UI", 9F);
            gButtonAdd.ForeColor = Color.White;
            gButtonAdd.HoverState.FillColor = Color.White;
            gButtonAdd.HoverState.Image = (Image)resources.GetObject("resource.Image1");
            gButtonAdd.Image = (Image)resources.GetObject("gButtonAdd.Image");
            gButtonAdd.ImageOffset = new Point(1, 0);
            gButtonAdd.ImageSize = new Size(22, 22);
            gButtonAdd.Location = new Point(253, 10);
            gButtonAdd.Name = "gButtonAdd";
            gButtonAdd.PressedColor = Color.White;
            gButtonAdd.ShadowDecoration.CustomizableEdges = customizableEdges18;
            gButtonAdd.Size = new Size(35, 35);
            gButtonAdd.TabIndex = 14;
            gButtonAdd.Click += gButtonAdd_Click;
            // 
            // lblMemRole
            // 
            lblMemRole.AutoSize = true;
            lblMemRole.BackColor = Color.White;
            lblMemRole.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMemRole.ForeColor = Color.FromArgb(74, 97, 94);
            lblMemRole.Location = new Point(648, 17);
            lblMemRole.Name = "lblMemRole";
            lblMemRole.Size = new Size(55, 20);
            lblMemRole.TabIndex = 62;
            lblMemRole.Text = "Leader";
            // 
            // gPictureBoxMemRole
            // 
            gPictureBoxMemRole.BackColor = Color.White;
            gPictureBoxMemRole.CustomizableEdges = customizableEdges11;
            gPictureBoxMemRole.FillColor = SystemColors.ControlLight;
            gPictureBoxMemRole.Image = Properties.Resources.PicItemLeaderKey;
            gPictureBoxMemRole.ImageRotate = 0F;
            gPictureBoxMemRole.Location = new Point(617, 15);
            gPictureBoxMemRole.Name = "gPictureBoxMemRole";
            gPictureBoxMemRole.ShadowDecoration.CustomizableEdges = customizableEdges12;
            gPictureBoxMemRole.Size = new Size(25, 25);
            gPictureBoxMemRole.SizeMode = PictureBoxSizeMode.StretchImage;
            gPictureBoxMemRole.TabIndex = 63;
            gPictureBoxMemRole.TabStop = false;
            // 
            // UCUserMiniLine
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            Controls.Add(lblUserCode);
            Controls.Add(lblUserName);
            Controls.Add(gCirclePictureBoxAvatar);
            Controls.Add(gShadowPanelBack);
            DoubleBuffered = true;
            Name = "UCUserMiniLine";
            Size = new Size(850, 60);
            ((System.ComponentModel.ISupportInitialize)gCirclePictureBoxAvatar).EndInit();
            gShadowPanelBack.ResumeLayout(false);
            gShadowPanelBack.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gPictureBoxMemRole).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblUserCode;
        private Label lblUserName;
        private Guna.UI2.WinForms.Guna2CirclePictureBox gCirclePictureBoxAvatar;
        private Guna.UI2.WinForms.Guna2ShadowPanel gShadowPanelBack;
        private Guna.UI2.WinForms.Guna2Button gButtonAdd;
        private Guna.UI2.WinForms.Guna2ProgressBar gProgressBarToLine;
        private Guna.UI2.WinForms.Guna2Button gButtonComplete;
        private Label lblMemRole;
        private Guna.UI2.WinForms.Guna2PictureBox gPictureBoxMemRole;
    }
}
