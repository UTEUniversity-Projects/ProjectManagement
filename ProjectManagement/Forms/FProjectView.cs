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
using ProjectManagement.Database;
using ProjectManagement.Models;
using ProjectManagement.Process;

namespace ProjectManagement.Forms
{
    public partial class FProjectView : Form
    {
        private MyProcess myProcess = new MyProcess();
        private UserDAO peopleDAO = new UserDAO();
        private ProjectStatusDAO thesisStatusDAO = new ProjectStatusDAO();

        public FProjectView(Project thesis)
        {
            InitializeComponent();
            SetInformation(thesis);
        }
        private void SetInformation(Project thesis)
        {
            gTextBoxStatus.Text = thesis.Status.ToString();
            gTextBoxStatus.FillColor = thesis.GetStatusColor();
            gTextBoxTopic.Text = thesis.Topic;
            gTextBoxField.Text = thesis.Field.ToString();
            gTextBoxLevel.Text = thesis.Level.ToString();
            gTextBoxMembers.Text = thesis.MaxMembers.ToString();
            gTextBoxDescription.Text = thesis.Description;
            gTextBoxTechnology.Text = thesis.Technology;
            gTextBoxFunctions.Text = thesis.Functions;
            gTextBoxRequirements.Text = thesis.Requirements;

            AddPeopleLine(peopleDAO.SelectOnlyByID(thesis.IdCreator), flpCreator);
            AddPeopleLine(peopleDAO.SelectOnlyByID(thesis.IdInstructor), flpInstructor);

            gTextBoxTeamRegistered.FillColor = gTextBoxStatus.FillColor;
            gTextBoxTeamRegistered.Text = thesisStatusDAO.CountTeamFollowState(thesis).ToString() + " teams";

            myProcess.SetItemFavorite(gButtonStar, thesis.IsFavorite);
        }
        private void AddPeopleLine(User people, FlowLayoutPanel flowLayoutPanel)
        {
            UCUserMiniLine uCPeople = new UCUserMiniLine();
            uCPeople.GButtonAdd.Hide();
            uCPeople.SetBackGroundColor(SystemColors.ButtonFace);
            uCPeople.SetInformation(people);
            uCPeople.SetSize(new Size(270, 60));
            flowLayoutPanel.Controls.Clear();
            flowLayoutPanel.Controls.Add(uCPeople);
        }
    }
}
