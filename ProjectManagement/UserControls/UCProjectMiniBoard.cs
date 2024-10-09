using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjectManagement.Forms;
using ProjectManagement.Models;
using ProjectManagement.Process;

namespace ProjectManagement
{
    public partial class UCProjectMiniBoard : UserControl
    {
        private MyProcess myProcess = new MyProcess();
        private Project thesis = new Project();

        public UCProjectMiniBoard(Project thesis)
        {
            InitializeComponent();

            this.thesis = thesis;
            myProcess.SetItemFavorite(gButtonStar, thesis.IsFavorite);
            gTextBoxStatus.Text = thesis.Status.ToString();
            gTextBoxStatus.FillColor = thesis.GetStatusColor();
            gTextBoxTopic.Text = thesis.Topic;
            gTextBoxField.Text = thesis.Field.ToString();
        }
        public void SetColorViewState(Color color)
        {
            gTextBoxTopic.FillColor = color;
            gTextBoxField.FillColor = color;
            gTextBoxTopic.BorderThickness = 0;
            gTextBoxField.BorderThickness = 0;
        }
        private void SetThesisView()
        {
            FProjectView fThesisView = new FProjectView(thesis);
            fThesisView.ShowDialog();
        }
        private void gButtonDetails_Click(object sender, EventArgs e)
        {
            SetThesisView();
        }
    }
}
