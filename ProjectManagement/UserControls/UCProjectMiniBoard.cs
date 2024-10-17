using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjectManagement.DAOs;
using ProjectManagement.Forms;
using ProjectManagement.Models;
using ProjectManagement.Utils;

namespace ProjectManagement
{
    public partial class UCProjectMiniBoard : UserControl
    {
        
        private Project project = new Project();

        public UCProjectMiniBoard(Project project)
        {
            InitializeComponent();

            this.project = project;
            // GunaControlUtil.SetItemFavorite(gButtonStar, project.IsFavorite);
            gTextBoxStatus.Text = EnumUtil.GetDisplayName(project.Status);
            gTextBoxStatus.FillColor = project.GetStatusColor();
            gTextBoxTopic.Text = project.Topic;
            gTextBoxField.Text = FieldDAO.SelectOnlyById(project.FieldId).Name;
        }
        public void SetColorViewState(Color color)
        {
            gTextBoxTopic.FillColor = color;
            gTextBoxField.FillColor = color;
            gTextBoxTopic.BorderThickness = 0;
            gTextBoxField.BorderThickness = 0;
        }
        private void SetProjectView()
        {
            FProjectView fProjectView = new FProjectView(project);
            fProjectView.ShowDialog();
        }
        private void gButtonDetails_Click(object sender, EventArgs e)
        {
            SetProjectView();
        }
    }
}
