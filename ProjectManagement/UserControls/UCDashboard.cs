using Guna.UI2.WinForms;
using System.Data;
using ProjectManagement.DAOs;
using ProjectManagement.Models;
using ProjectManagement.Process;
using ProjectManagement.Enums;
using ProjectManagement.Utils;

namespace ProjectManagement
{
    public partial class UCDashboard : UserControl
    {
        
        private Project projectClicked = new Project();
        private Users user = new Users();
        private List<Project> currentList = new List<Project>();
        private List<Project> listProject = new List<Project>();

        private UCProjectList uCProjectList = new UCProjectList();
        private UCProjectCreate uCProjectCreate = new UCProjectCreate();
        private UCProjectDetails uCProjectDetails = new UCProjectDetails();
        private UCProjectLine projectLineClicked = new UCProjectLine();
        private UCDashboardStatistics uCDashboardStatistical = new UCDashboardStatistics();
        private FProjectFilter fProjectFilter = new FProjectFilter();

        private bool flagStuMyTheses = false;
        private List<string> favoriteProjects = new List<string>();

        public UCDashboard()
        {
            InitializeComponent();

            #region Record EVENT Users control

            uCProjectDetails.GButtonBack.Click += ProjectDetailsBack_Clicked;

            uCProjectList.GButtonCreateProject.Click += gGradientButtonCreateProject_Click;
            uCProjectList.GButtonFavorite.Click += ByFavorite_Clicked;
            uCProjectList.GButtonTopic.Click += ByTopic_Clicked;
            uCProjectList.GButtonFilter.Click += ByFilter_Clicked;
            uCProjectList.GButtonReset.Click += ResetProjectList_Clicked;
            uCProjectList.GTextBoxSearch.TextChanged += SearchProjectTopic_TextChanged;

            uCProjectCreate.GButtonCancel.Click += gGradientButtonViewProject_Click;

            #endregion

        }

        #region PROPERTIES

        public bool FlagStuMyTheses
        {
            set { this.flagStuMyTheses = value; }
        }

        #endregion

        #region FUNCTIONS

        public void SetInformation(Users user)
        {
            this.user = user;
            this.favoriteProjects = ProjectDAO.GetFavoriteList(this.user.UserId);
            gGradientButtonProjects.PerformClick();
            fProjectFilter.SetUpFilter(user);
            fProjectFilter.ListProject = this.currentList;
            fProjectFilter.GButtonFilter.Click += GFilter_Click;
        }
        private void UpdateProjectList()
        {
            if (flagStuMyTheses)
            {
                UpdateProjectListStuMyTheses();
            }
            else
            {
                if (user.Role == EUserRole.LECTURE) UpdateProjectListLecture();
                else UpdateProjectListStudent();
            }
        }
        private void UpdateUCProjectLine(bool flag, Project newProject)
        {
            if (flag)
            {
                this.projectLineClicked.SetInformation(this.user, newProject, this.favoriteProjects.Contains(newProject.ProjectId));
            }
        }
        private void UpdateProjectListLecture()
        {
            this.currentList = ProjectDAO.SelectListRoleLecture(this.user.UserId);
            this.listProject = currentList;
        }
        private void UpdateProjectListStudent()
        {
            this.currentList = ProjectDAO.SelectListRoleStudent(this.user.UserId);
            this.listProject = currentList;
        }
        private void UpdateProjectListStuMyTheses()
        {
            this.currentList = ProjectDAO.SelectListModeMyProjects(this.user.UserId);
            this.listProject = currentList;
        }
        private void AllButtonStandardColor()
        {
            GunaControlUtil.ButtonStandardColor(gGradientButtonProjects);
            GunaControlUtil.ButtonStandardColor(gGradientButtonStatistics);
        }
        private void AddUserControl(Guna2GradientButton button, UserControl userControl)
        {
            AllButtonStandardColor();
            GunaControlUtil.ButtonSettingColor(button);
            gPanelDataView.Controls.Clear();
            gPanelDataView.Controls.Add(userControl);
        }
        private void LoadProjectList()
        {
            uCProjectList.Clear();

            for (int i = 0; i < listProject.Count; i++)
            {
                UCProjectLine projectLine = new UCProjectLine();
                projectLine.SetInformation(this.user, listProject[i], this.favoriteProjects.Contains(listProject[i].ProjectId));
                projectLine.ProjectLineClicked += ProjectLine_Clicked;
                projectLine.ProjectFavoriteClicked += ProjectFavorite_Clicked;
                projectLine.NotificationJump += ProjectLine_NotificationJump;
                uCProjectList.AddProject(projectLine);
            }
            uCProjectList.SetNumProject(listProject.Count, true);
        }
        public void NotificationJump(Notification notification) 
        {
            uCProjectList.NotificationJump(notification);
        }

        #endregion

        #region EVENT gGradientButtonViewProject

        private void gGradientButtonViewProject_Click(object sender, EventArgs e)
        {
            AllButtonStandardColor();
            GunaControlUtil.ButtonSettingColor(gGradientButtonProjects);
            UpdateProjectList();

            gPanelDataView.Controls.Clear();
            uCProjectList.SetFilter(false);
            gPanelDataView.Controls.Add(uCProjectList);
            ByStatus_Clicked(sender, e);
            fProjectFilter.SetUpFilter(user);
        }

        #endregion

        #region EVENT gGradientButtonStatistical

        private void gGradientButtonStatistical_Click(object sender, EventArgs e)
        {
            AllButtonStandardColor();
            GunaControlUtil.ButtonSettingColor(gGradientButtonStatistics);
            UpdateProjectList();

            gPanelDataView.Controls.Clear();
            uCDashboardStatistical.SetInformation(this.listProject);
            gPanelDataView.Controls.Add(uCDashboardStatistical);
        }

        #endregion

        #region EVENT gGradientButtonCreateProject

        private void gGradientButtonCreateProject_Click(object sender, EventArgs e)
        {
            uCProjectCreate.SetCreateState(user);
            AddUserControl(new Guna2GradientButton(), uCProjectCreate);
        }

        #endregion

        #region PROJECT DETAILS

        private void ProjectDetailsBack_Clicked(object sender, EventArgs e)
        {
            UpdateUCProjectLine(uCProjectDetails.ProjectEdited, uCProjectDetails.GetProject);
            if (uCProjectDetails.ProjectDeleted) uCProjectList.ProjectDelete_Clicked(this.projectLineClicked, e);
            gPanelDataView.Controls.Clear();
            gPanelDataView.Controls.Add(uCProjectList);
        }

        #endregion

        #region PROJECT LINE 

        private void ProjectDetailsShow(UCProjectLine projectLine)
        {
            gPanelDataView.Controls.Clear();
            this.projectClicked = ProjectDAO.SelectOnly(projectLine.GetIdProject);
            this.projectLineClicked = projectLine;
            uCProjectDetails.SetInformation(this.projectClicked, user, this.favoriteProjects.Contains(this.projectClicked.ProjectId), this.flagStuMyTheses);
            gPanelDataView.Controls.Add(uCProjectDetails);
            
        }
        private void ProjectLine_Clicked(object sender, EventArgs e)
        {
            UCProjectLine projectLine = sender as UCProjectLine;

            if (projectLine != null)
            {
                ProjectDetailsShow(projectLine);
            }
        }
        private void ProjectFavorite_Clicked(object sender, EventArgs e)
        {
            UCProjectLine projectLine = sender as UCProjectLine;

            if (projectLine != null)
            {
                if (projectLine.IsFavorite) this.favoriteProjects.Add(projectLine.GetIdProject);
                else this.favoriteProjects.Remove(projectLine.GetIdProject);
            }
        }
        private void ProjectLine_NotificationJump(object sender, EventArgs e)
        {
            UCProjectLine projectLine = sender as UCProjectLine;

            if (projectLine != null)
            {
                ProjectDetailsShow(projectLine);
                uCProjectDetails.PerformNotificationClick(projectLine.GetNotification);
            }
        }

        #endregion

        #region PROJECT LIST

        private void ByFavorite_Clicked(object sender, EventArgs e)
        {
            // listProject.Sort((a, b) => a.IsFavorite == true ? -1 : b.IsFavorite == true ? 1 : 0);
            LoadProjectList();
        }
        private void ByTopic_Clicked(object sender, EventArgs e)
        {
            listProject = listProject.OrderBy(project => project.Topic).ToList();
            LoadProjectList();
        }
        private void ByFilter_Clicked(object sender, EventArgs e)
        {
            this.fProjectFilter = new FProjectFilter();

            fProjectFilter.SetUpFilter(user);
            fProjectFilter.ListProject = this.currentList;
            fProjectFilter.GButtonFilter.Click += GFilter_Click;
            this.fProjectFilter.ShowDialog();
        }
        private void ByStatus_Clicked(object sender, EventArgs e)
        {
            listProject.Sort((a, b) => a.GetPriority().CompareTo(b.GetPriority()));
            LoadProjectList();
        }
        private void ByProjectCode_Clicked(object sender, EventArgs e)
        {
            listProject = listProject.OrderBy(project => project.ProjectId).ToList();
            LoadProjectList();
        }
        private void GFilter_Click(object sender, EventArgs e)
        {
            uCProjectList.SetFilter(true);
            List<Project> listFilter = fProjectFilter.ListProject;
            this.listProject = currentList.Where(t => listFilter.Any(t2 => t2.ProjectId == t.ProjectId)).ToList();
            LoadProjectList();
        }
        private void ResetProjectList_Clicked(object sender, EventArgs e)
        {
            gGradientButtonViewProject_Click(sender, e);
        }

        #endregion

        #region FUNCTIONS SEARCH

        private List<Project> GetProjectListByTopic(Guna2TextBox textBox)
        {
            if (user.Role == EUserRole.LECTURE)
            {

                return ProjectDAO.SearchRoleLecture(this.user.UserId, textBox.Text);
            }
            else
            {
                return ProjectDAO.SearchRoleStudent(textBox.Text);
            }
        }
        private void SearchProjectTopic_TextChanged(object sender, EventArgs e)
        {
            Guna2TextBox textBox = sender as Guna2TextBox;

            if (textBox != null)
            {
                List<Project> listFilter = GetProjectListByTopic(textBox);
                List<Project> temp = this.listProject;
                this.listProject = listProject.Where(t => listFilter.Any(t2 => t2.ProjectId == t.ProjectId)).ToList();
                LoadProjectList();
                this.listProject = temp;
            }
        }

        #endregion

    }
}
