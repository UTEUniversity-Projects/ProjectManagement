using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjectManagement.DAOs;
using ProjectManagement.Database;
using ProjectManagement.MetaData;
using ProjectManagement.Models;
using ProjectManagement.Process;
using ProjectManagement.Utils;

namespace ProjectManagement.Forms
{
    public partial class FProjectView : Form
    {
        private ProjectMeta projectMeta;

        public FProjectView(ProjectMeta projectMeta)
        {
            InitializeComponent();
            this.projectMeta = projectMeta;
            SetInformation(projectMeta.Project);
        }
        private void SetInformation(Project project)
        {
            gTextBoxStatus.Text = EnumUtil.GetDisplayName(project.Status);
            gTextBoxStatus.FillColor = project.GetStatusColor();
            gTextBoxTopic.Text = project.Topic;
            gTextBoxField.Text = FieldDAO.SelectOnlyById(project.FieldId).Name;
            gTextBoxMembers.Text = project.MaxMember.ToString();
            gTextBoxDescription.Text = project.Description;
            gTextBoxTechnology.Text =TechnologyDAO.GetListTechnology(project.ProjectId);
            gTextBoxFunctions.Text = project.Feature;
            gTextBoxRequirements.Text = project.Requirement;

            AddUserLine(UserDAO.SelectOnlyByID(project.CreatedBy), flpCreator);
            AddUserLine(UserDAO.SelectOnlyByID(project.InstructorId), flpInstructor);

            gTextBoxTeamRegistered.FillColor = gTextBoxStatus.FillColor;
            gTextBoxTeamRegistered.Text = TeamDAO.CountTeamFollowState(project.ProjectId, project.Status).ToString() + " teams";

            GunaControlUtil.SetItemFavorite(gButtonStar, this.projectMeta.IsFavorite);
        }
        private void AddUserLine(Users user, FlowLayoutPanel flowLayoutPanel)
        {
            UCUserMiniLine uCUser = new UCUserMiniLine();
            uCUser.GButtonAdd.Hide();
            uCUser.SetBackGroundColor(SystemColors.ButtonFace);
            uCUser.SetInformation(user);
            uCUser.SetSize(new Size(270, 60));
            flowLayoutPanel.Controls.Clear();
            flowLayoutPanel.Controls.Add(uCUser);
        }
    }
}
