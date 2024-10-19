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
using ProjectManagement.MetaData;

namespace ProjectManagement
{
    public partial class UCProjectMiniBoard : UserControl
    {
        
        private Project project = new Project();
        private bool isFavorite = false;

        public UCProjectMiniBoard(ProjectMeta projectMeta)
        {
            InitializeComponent();

            this.project = projectMeta.Project;
            this.isFavorite = projectMeta.IsFavorite;
            GunaControlUtil.SetItemFavorite(gButtonStar, this.isFavorite);
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
            FProjectView fProjectView = new FProjectView(new ProjectMeta(project, isFavorite));
            fProjectView.ShowDialog();
        }
        private void gButtonDetails_Click(object sender, EventArgs e)
        {
            SetProjectView();
        }
    }
}
