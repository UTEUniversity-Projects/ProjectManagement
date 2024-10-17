using ProjectManagement.DAOs;
using ProjectManagement.Models;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectManagement.UserControls
{
    public partial class UCTechnologyLine : UserControl
    {
        public event EventHandler TechnologyRemove;

        private Technology technology = new Technology();

        public UCTechnologyLine(Technology technology)
        {
            InitializeComponent();
            this.technology = technology;
            InitUserControl();
        }
        private void InitUserControl()
        {
            SetUserControlSize();
            rtbContent.Text = this.technology.Name;
        }

        public Technology GetTechnology
        {
            get { return this.technology; } 
        }

        private int CalculateTextWidth(string text, Font font)
        {
            using (var pictureBox = new PictureBox())
            {
                using (var g = pictureBox.CreateGraphics())
                {
                    SizeF size = g.MeasureString(text, font);
                    return (int)Math.Ceiling(size.Width);
                }
            }
        }
        private void SetUserControlSize()
        {
            int pixelContent = CalculateTextWidth(this.technology.Name, rtbContent.Font);
            int width = Math.Min(pixelContent + 15, 610);
            int height = Math.Max((pixelContent / 500 + (pixelContent % 500 != 0 ? 1 : 0)) * 25, 23);

            rtbContent.Size = new Size(width, height);
            gShadowPanelTeam.Size = new Size(Math.Min(width + 30, 610), height + 10);
            this.Size = new Size(Math.Min(width + 30, 640), height + 15);
            gCirclePictureBoxRemove.Location = new Point(Math.Min(width + 30, 610) - 30, 5);
        }
        private void gCirclePictureBoxRemove_Click(object sender, EventArgs e)
        {
            OnTechnologyRemoveClicked(EventArgs.Empty);
        }
        private void OnTechnologyRemoveClicked(EventArgs e)
        {
            TechnologyRemove?.Invoke(this, e);
        }
    }
}
