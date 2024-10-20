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
using ProjectManagement.Utils;

namespace ProjectManagement
{
    public partial class UCProjectMiniLine : UserControl
    {
        
        private Project project = new Project();

        public UCProjectMiniLine(Project project)
        {
            InitializeComponent();
            this.project = project;
            InitUserControl();
        }
        private void InitUserControl()
        {
            lblProjectTopic.Text = DataTypeUtil.FormatStringLength(project.Topic, 20);
            gTextBoxStatus.Text = EnumUtil.GetDisplayName(project.Status);
            gTextBoxStatus.FillColor = project.GetStatusColor();
        }
        private void UCProjectMiniLine_Click(object sender, EventArgs e)
        {
            FProjectView fProjectView = new FProjectView(new (this.project, false));
            fProjectView.ShowDialog();
        }
    }
}
