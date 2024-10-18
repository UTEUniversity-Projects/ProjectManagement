namespace ProjectManagement.UserControls
{
    partial class UCTechnologyLine
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            gShadowPanelTeam = new Guna.UI2.WinForms.Guna2ShadowPanel();
            gCirclePictureBoxRemove = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            rtbContent = new RichTextBox();
            gShadowPanelTeam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gCirclePictureBoxRemove).BeginInit();
            SuspendLayout();
            // 
            // gShadowPanelTeam
            // 
            gShadowPanelTeam.BackColor = Color.Transparent;
            gShadowPanelTeam.Controls.Add(gCirclePictureBoxRemove);
            gShadowPanelTeam.Controls.Add(rtbContent);
            gShadowPanelTeam.FillColor = Color.FromArgb(242, 245, 244);
            gShadowPanelTeam.Location = new Point(0, 0);
            gShadowPanelTeam.Name = "gShadowPanelTeam";
            gShadowPanelTeam.Radius = 5;
            gShadowPanelTeam.ShadowColor = Color.Black;
            gShadowPanelTeam.ShadowShift = 1;
            gShadowPanelTeam.Size = new Size(400, 35);
            gShadowPanelTeam.TabIndex = 65;
            // 
            // gCirclePictureBoxRemove
            // 
            gCirclePictureBoxRemove.Image = Properties.Resources.PicItemRemove;
            gCirclePictureBoxRemove.ImageRotate = 0F;
            gCirclePictureBoxRemove.Location = new Point(350, 5);
            gCirclePictureBoxRemove.Name = "gCirclePictureBoxRemove";
            gCirclePictureBoxRemove.ShadowDecoration.CustomizableEdges = customizableEdges1;
            gCirclePictureBoxRemove.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            gCirclePictureBoxRemove.Size = new Size(23, 23);
            gCirclePictureBoxRemove.SizeMode = PictureBoxSizeMode.Zoom;
            gCirclePictureBoxRemove.TabIndex = 66;
            gCirclePictureBoxRemove.TabStop = false;
            gCirclePictureBoxRemove.Click += gCirclePictureBoxRemove_Click;
            // 
            // rtbContent
            // 
            rtbContent.BackColor = Color.FromArgb(242, 245, 244);
            rtbContent.BorderStyle = BorderStyle.None;
            rtbContent.Font = new Font("Nirmala UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rtbContent.Location = new Point(10, 5);
            rtbContent.Name = "rtbContent";
            rtbContent.Size = new Size(210, 23);
            rtbContent.TabIndex = 65;
            rtbContent.Text = "Spring (Java)";
            // 
            // UCTechnologyLine
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(gShadowPanelTeam);
            Name = "UCTechnologyLine";
            Size = new Size(400, 80);
            gShadowPanelTeam.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gCirclePictureBoxRemove).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2ShadowPanel gShadowPanelTeam;
        private RichTextBox rtbContent;
        private Guna.UI2.WinForms.Guna2CirclePictureBox gCirclePictureBoxRemove;
    }
}
