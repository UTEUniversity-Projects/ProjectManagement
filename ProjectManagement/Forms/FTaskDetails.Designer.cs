namespace ProjectManagement.Forms
{
    partial class FTaskDetails
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
            components = new System.ComponentModel.Container();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FTaskDetails));
            gShadowPanelBack = new Guna.UI2.WinForms.Guna2ShadowPanel();
            gShadowPanelView = new Guna.UI2.WinForms.Guna2ShadowPanel();
            erpTitle = new ErrorProvider(components);
            erpDescription = new ErrorProvider(components);
            erpProgress = new ErrorProvider(components);
            gGradientButtonComment = new Guna.UI2.WinForms.Guna2GradientButton();
            gGradientButtonEvaluate = new Guna.UI2.WinForms.Guna2GradientButton();
            gGradientButtonDetails = new Guna.UI2.WinForms.Guna2GradientButton();
            gShadowPanelBack.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)erpTitle).BeginInit();
            ((System.ComponentModel.ISupportInitialize)erpDescription).BeginInit();
            ((System.ComponentModel.ISupportInitialize)erpProgress).BeginInit();
            SuspendLayout();
            // 
            // gShadowPanelBack
            // 
            gShadowPanelBack.BackColor = Color.Transparent;
            gShadowPanelBack.Controls.Add(gShadowPanelView);
            gShadowPanelBack.FillColor = Color.White;
            gShadowPanelBack.Location = new Point(10, 43);
            gShadowPanelBack.Name = "gShadowPanelBack";
            gShadowPanelBack.Radius = 7;
            gShadowPanelBack.ShadowColor = Color.Black;
            gShadowPanelBack.ShadowStyle = Guna.UI2.WinForms.Guna2ShadowPanel.ShadowMode.ForwardDiagonal;
            gShadowPanelBack.Size = new Size(707, 520);
            gShadowPanelBack.TabIndex = 42;
            // 
            // gShadowPanelView
            // 
            gShadowPanelView.BackColor = Color.Transparent;
            gShadowPanelView.FillColor = Color.White;
            gShadowPanelView.Location = new Point(10, 15);
            gShadowPanelView.Name = "gShadowPanelView";
            gShadowPanelView.ShadowColor = Color.Black;
            gShadowPanelView.ShadowShift = 0;
            gShadowPanelView.ShadowStyle = Guna.UI2.WinForms.Guna2ShadowPanel.ShadowMode.ForwardDiagonal;
            gShadowPanelView.Size = new Size(680, 490);
            gShadowPanelView.TabIndex = 46;
            // 
            // erpTitle
            // 
            erpTitle.ContainerControl = this;
            erpTitle.Icon = (Icon)resources.GetObject("erpTitle.Icon");
            // 
            // erpDescription
            // 
            erpDescription.ContainerControl = this;
            erpDescription.Icon = (Icon)resources.GetObject("erpDescription.Icon");
            // 
            // erpProgress
            // 
            erpProgress.ContainerControl = this;
            erpProgress.Icon = (Icon)resources.GetObject("erpProgress.Icon");
            // 
            // gGradientButtonComment
            // 
            gGradientButtonComment.BackColor = Color.Transparent;
            gGradientButtonComment.BorderRadius = 10;
            gGradientButtonComment.CustomizableEdges = customizableEdges1;
            gGradientButtonComment.DisabledState.BorderColor = Color.DarkGray;
            gGradientButtonComment.DisabledState.CustomBorderColor = Color.DarkGray;
            gGradientButtonComment.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            gGradientButtonComment.DisabledState.FillColor2 = Color.FromArgb(169, 169, 169);
            gGradientButtonComment.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            gGradientButtonComment.FillColor = Color.White;
            gGradientButtonComment.FillColor2 = Color.White;
            gGradientButtonComment.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gGradientButtonComment.ForeColor = Color.Black;
            gGradientButtonComment.HoverState.FillColor = Color.FromArgb(94, 148, 255);
            gGradientButtonComment.HoverState.FillColor2 = Color.FromArgb(255, 77, 165);
            gGradientButtonComment.HoverState.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gGradientButtonComment.HoverState.ForeColor = Color.White;
            gGradientButtonComment.Location = new Point(153, 12);
            gGradientButtonComment.Name = "gGradientButtonComment";
            gGradientButtonComment.ShadowDecoration.CustomizableEdges = customizableEdges2;
            gGradientButtonComment.Size = new Size(117, 40);
            gGradientButtonComment.TabIndex = 44;
            gGradientButtonComment.Text = "Comment";
            gGradientButtonComment.Click += gGradientButtonComment_Click;
            // 
            // gGradientButtonEvaluate
            // 
            gGradientButtonEvaluate.BackColor = Color.Transparent;
            gGradientButtonEvaluate.BorderRadius = 10;
            gGradientButtonEvaluate.CustomizableEdges = customizableEdges3;
            gGradientButtonEvaluate.DisabledState.BorderColor = Color.DarkGray;
            gGradientButtonEvaluate.DisabledState.CustomBorderColor = Color.DarkGray;
            gGradientButtonEvaluate.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            gGradientButtonEvaluate.DisabledState.FillColor2 = Color.FromArgb(169, 169, 169);
            gGradientButtonEvaluate.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            gGradientButtonEvaluate.FillColor = Color.White;
            gGradientButtonEvaluate.FillColor2 = Color.White;
            gGradientButtonEvaluate.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gGradientButtonEvaluate.ForeColor = Color.Black;
            gGradientButtonEvaluate.HoverState.FillColor = Color.FromArgb(94, 148, 255);
            gGradientButtonEvaluate.HoverState.FillColor2 = Color.FromArgb(255, 77, 165);
            gGradientButtonEvaluate.HoverState.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gGradientButtonEvaluate.HoverState.ForeColor = Color.White;
            gGradientButtonEvaluate.Location = new Point(269, 12);
            gGradientButtonEvaluate.Name = "gGradientButtonEvaluate";
            gGradientButtonEvaluate.ShadowDecoration.CustomizableEdges = customizableEdges4;
            gGradientButtonEvaluate.Size = new Size(117, 40);
            gGradientButtonEvaluate.TabIndex = 45;
            gGradientButtonEvaluate.Text = "Evaluate";
            gGradientButtonEvaluate.Click += gGradientButtonEvaluate_Click;
            // 
            // gGradientButtonDetails
            // 
            gGradientButtonDetails.BackColor = Color.Transparent;
            gGradientButtonDetails.BorderRadius = 10;
            gGradientButtonDetails.CustomizableEdges = customizableEdges5;
            gGradientButtonDetails.DisabledState.BorderColor = Color.DarkGray;
            gGradientButtonDetails.DisabledState.CustomBorderColor = Color.DarkGray;
            gGradientButtonDetails.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            gGradientButtonDetails.DisabledState.FillColor2 = Color.FromArgb(169, 169, 169);
            gGradientButtonDetails.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            gGradientButtonDetails.FillColor = Color.White;
            gGradientButtonDetails.FillColor2 = Color.White;
            gGradientButtonDetails.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gGradientButtonDetails.ForeColor = Color.Black;
            gGradientButtonDetails.HoverState.FillColor = Color.FromArgb(94, 148, 255);
            gGradientButtonDetails.HoverState.FillColor2 = Color.FromArgb(255, 77, 165);
            gGradientButtonDetails.HoverState.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gGradientButtonDetails.HoverState.ForeColor = Color.White;
            gGradientButtonDetails.Location = new Point(37, 12);
            gGradientButtonDetails.Name = "gGradientButtonDetails";
            gGradientButtonDetails.ShadowDecoration.CustomizableEdges = customizableEdges6;
            gGradientButtonDetails.Size = new Size(117, 40);
            gGradientButtonDetails.TabIndex = 46;
            gGradientButtonDetails.Text = "Details";
            gGradientButtonDetails.Click += gGradientButtonDetails_Click;
            // 
            // FTaskDetails
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonFace;
            ClientSize = new Size(727, 586);
            Controls.Add(gShadowPanelBack);
            Controls.Add(gGradientButtonComment);
            Controls.Add(gGradientButtonEvaluate);
            Controls.Add(gGradientButtonDetails);
            DoubleBuffered = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FTaskDetails";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Task Details";
            gShadowPanelBack.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)erpTitle).EndInit();
            ((System.ComponentModel.ISupportInitialize)erpDescription).EndInit();
            ((System.ComponentModel.ISupportInitialize)erpProgress).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Guna.UI2.WinForms.Guna2ShadowPanel gShadowPanelBack;
        private ErrorProvider erpTitle;
        private ErrorProvider erpDescription;
        private ErrorProvider erpProgress;
        private Guna.UI2.WinForms.Guna2GradientButton gGradientButtonComment;
        private Guna.UI2.WinForms.Guna2GradientButton gGradientButtonEvaluate;
        private Guna.UI2.WinForms.Guna2ShadowPanel gShadowPanelView;
        private Guna.UI2.WinForms.Guna2GradientButton gGradientButtonDetails;
    }
}