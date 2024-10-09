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
    public partial class UCProjectMiniLine : UserControl
    {
        private MyProcess myProcess = new MyProcess();
        private Project thesis = new Project();

        public UCProjectMiniLine(Project thesis)
        {
            InitializeComponent();
            this.thesis = thesis;
            InitUserControl();
        }
        private void InitUserControl()
        {
            lblProjectTopic.Text = myProcess.FormatStringLength(thesis.Topic, 20);
            gTextBoxStatus.Text = thesis.Status.ToString();
            gTextBoxStatus.FillColor = thesis.GetStatusColor();
        }
        private void UCThesisMiniLine_Click(object sender, EventArgs e)
        {
            FProjectView fThesisView = new FProjectView(this.thesis);
            fThesisView.ShowDialog();
        }
    }
}
