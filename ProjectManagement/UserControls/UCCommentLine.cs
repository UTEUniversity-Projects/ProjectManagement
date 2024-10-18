using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjectManagement.Models;
using ProjectManagement.Process;
using ProjectManagement.Forms;
using ProjectManagement.DAOs;
using ProjectManagement.Utils;

namespace ProjectManagement
{
    public partial class UCCommentLine : UserControl
    {

        private Comment comment = new Comment();
        private Users creator = new Users();

        public UCCommentLine()
        {
            InitializeComponent();
        }
        public UCCommentLine(Comment comment)
        {
            InitializeComponent();
            this.comment = comment;
            InitUserControl();
        }
        private void InitUserControl()
        {
            this.creator = UserDAO.SelectOnlyByID(comment.CreatedBy);
            SetUserControlSize();
            rtbContent.Text = comment.Content;
            lblCreator.Text = creator.FullName;
            gCirclePictureBoxCreator.Image = WinformControlUtil.NameToImage(creator.Avatar);
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
            int pixelContent = CalculateTextWidth(comment.Content, rtbContent.Font);
            int width = Math.Min(Math.Max(pixelContent + 15, creator.FullName.Length * 10), 510);
            int height = Math.Max((pixelContent / 500 + (pixelContent % 500 != 0 ? 1 : 0)) * 30, 35);

            rtbContent.Size = new Size(width, height);
            gShadowPanelTeam.Size = new Size(Math.Min(width + 30, 550), height + 35);
            this.Size = new Size(640, height + 42);
        }
        private void gCirclePictureBoxCreator_Click(object sender, EventArgs e)
        {
            FUserDetails fUserDetails = new FUserDetails(creator);
            fUserDetails.ShowDialog();
        }
    }
}
