using Guna.UI2.WinForms;
using ProjectManagement.DAOs;
using ProjectManagement.Enums;
using ProjectManagement.Forms;
using ProjectManagement.MetaData;
using ProjectManagement.Models;
using ProjectManagement.Utils;

namespace ProjectManagement
{
    public partial class UCProjectDetails : UserControl
    {

        private Project project = new Project();
        private Users host = new Users();
        private Team team = new Team();
        private Users instructor = new Users();
        private Notification notification = new Notification();
        private List<Team> teams = new List<Team>();

        private UCProjectDetailsTeam showTeam = new UCProjectDetailsTeam();
        private UCProjectDetailsRegistered uCProjectDetailsRegistered = new UCProjectDetailsRegistered();
        private UCProjectDetailsCreatedTeam uCProjectDetailsCreatedTeam = new UCProjectDetailsCreatedTeam();
        private UCProjectDetailsStatistical uCProjectDetailsStatistical = new UCProjectDetailsStatistical();

        private bool flagEdited = false;
        private bool flagDeleted = false;
        private bool flagStuMyTheses = false;
        private bool isFavorite = false;

        public UCProjectDetails()
        {
            InitializeComponent();
        }

        #region PROPERTIES

        public bool ProjectEdited
        {
            get { return this.flagEdited; }
        }
        public bool ProjectDeleted
        {
            get { return this.flagDeleted; }
        }
        public Project GetProject
        {
            get { return this.project; }
        }
        public Guna2Button GButtonBack
        {
            get { return this.gButtonBack; }
        }

        #endregion

        #region FUNCTIONS

        public void SetInformation(Project project, Users host, bool isFavorite, bool flagStuMyTheses)
        {
            this.project = project;
            this.host = host;
            this.isFavorite = isFavorite;
            this.flagStuMyTheses = flagStuMyTheses;
            this.instructor = UserDAO.SelectOnlyByID(project.InstructorId);
            InitUserControl();
        }
        private void InitUserControl()
        {
            this.flagEdited = false;
            this.flagDeleted = false;

            gShadowPanelTeam.Controls.Add(showTeam);

            ResetProjectInfor();
            SetControlsReadOnly(true);
            SetInitialSate();
            SetButtonComplete();
            SetButtonEditOrDetails();
        }
        private void ResetProjectInfor()
        {
            this.project = ProjectDAO.SelectOnly(project.ProjectId);

            GunaControlUtil.SetItemFavorite(gButtonStar, this.isFavorite);
            gTextBoxStatus.Text = EnumUtil.GetDisplayName(project.Status);
            gTextBoxStatus.FillColor = project.GetStatusColor();
            gTextBoxTopic.Text = project.Topic;
            gTextBoxField.Text = FieldDAO.SelectOnlyById(project.FieldId).Name;
            gTextBoxMembers.Text = project.MaxMember.ToString();
            gTextBoxDescription.Text = project.Description;
        }
        private void SetControlsReadOnly(bool flagReadOnly)
        {
            GunaControlUtil.SetTextBoxState(gTextBoxTopic, flagReadOnly);
            GunaControlUtil.SetTextBoxState(gTextBoxDescription, flagReadOnly);
            GunaControlUtil.SetTextBoxState(gTextBoxField, flagReadOnly);
            GunaControlUtil.SetTextBoxState(gTextBoxMembers, flagReadOnly);
        }
        private void SetInitialSate()
        {
            SetTeamHere(false);
            gGradientButtonReasonDetails.Hide();

            if (project.Status == EProjectStatus.WAITING)
            {
                SetTeamMode(true);
                SetWaitingGiveUpMode(true);
                return;
            }

            if (project.Status == EProjectStatus.PROCESSING || project.Status == EProjectStatus.COMPLETED)
            {
                SetTeamMode(true);
                SetViewButtonMode(true);
                return;
            }
            if (project.Status == EProjectStatus.GAVEUP)
            {
                SetTeamMode(true);
                SetGiveUpMode(true);
                return;
            }
            if (project.Status == EProjectStatus.REGISTERED || project.Status == EProjectStatus.PUBLISHED)
            {
                SetViewButtonMode(false);
                if (host.Role == EUserRole.LECTURE)
                {
                    gGradientButtonRegistered.PerformClick();
                }
                else
                {
                    if (this.flagStuMyTheses)
                    {
                        SetWaitingMode(true);
                    }
                    else
                    {
                        SetStudentRegister(true);
                    }
                }
            }
        }
        private void SetUpDataViewState()
        {
            HideAllButtonMode();
            gPanelDataView.Controls.Clear();
            gPanelDataView.Controls.Add(gPictureBoxState);
            gPanelDataView.Controls.Add(gTextBoxState);
            gPanelDataView.Controls.Add(gGradientButtonReasonDetails);
            gPanelDataView.Controls.Add(gGradientButtonConfirm);
            gPanelDataView.Controls.Add(gGradientButtonReject);
        }
        private void SetTeamMode(bool flagShow)
        {
            if (flagShow)
            {
                this.team = TeamDAO.SelectFollowProject(this.project.ProjectId);
                if (team != null)
                {
                    showTeam.SetInformation(team, new ProjectMeta(project, isFavorite));
                    showTeam.Location = new Point(5, 5);
                    SetTeamHere(true);
                }
            }
            else
            {
                SetTeamHere(false);
            }
        }
        private void SetViewButtonMode(bool flagShow)
        {
            if (flagShow)
            {
                gGradientButtonRegistered.Hide();
                gGradientButtonTasks.Show();
                gGradientButtonMeetings.Show();
                gGradientButtonStatistics.Show();
                gGradientButtonTasks.PerformClick();
            }
            else
            {
                gGradientButtonTasks.Hide();
                gGradientButtonMeetings.Hide();
                gGradientButtonStatistics.Hide();
                gGradientButtonRegistered.Show();
                gGradientButtonRegistered.PerformClick();
            }
        }
        private void SetWaitingMode(bool flag)
        {
            if (flag == false) return;

            gPictureBoxState.Image = Properties.Resources.GiftWaiting;
            gTextBoxState.Text = "Please wait !";
            gTextBoxState.ForeColor = Color.FromArgb(0, 192, 192);
            SetUpDataViewState();
        }
        private void SetWaitingGiveUpMode(bool flag)
        {
            if (flag == false) return;

            gPictureBoxState.Image = Properties.Resources.GiftWaiting;
            gTextBoxState.Text = "The project cannot continue !";
            gTextBoxState.ForeColor = Color.FromArgb(0, 192, 192);
            gGradientButtonReasonDetails.Show();
            gGradientButtonReject.Show();
            gGradientButtonConfirm.Show();
            SetUpDataViewState();
        }
        private void SetGiveUpMode(bool flag)
        {
            if (flag == false) return;

            gPictureBoxState.Image = Properties.Resources.PictureEmptyState;
            gTextBoxState.Text = "The project cannot continue !";
            gTextBoxState.ForeColor = Color.Gray;
            gGradientButtonReasonDetails.Show();
            SetUpDataViewState();
        }
        private void SetSuccessfullyRegistered()
        {
            gPictureBoxState.Image = Properties.Resources.GifCompleted;
            gTextBoxState.Text = "You have successfully registered !";
            gTextBoxState.ForeColor = Color.FromArgb(0, 192, 192);
            SetUpDataViewState();
        }
        private void SetButtonComplete()
        {
            gGradientButtonComplete.Hide();
            gGradientButtonGiveUp.Hide();
            gGradientButtonConfirm.Hide();
            gGradientButtonReject.Hide();

            if (host.Role == EUserRole.LECTURE && project.Status == EProjectStatus.WAITING)
            {
                gGradientButtonConfirm.Show();
                gGradientButtonReject.Show();
                return;
            }
            if (host.Role == EUserRole.LECTURE && project.Status == EProjectStatus.PROCESSING)
            {
                gGradientButtonComplete.Show();
                gGradientButtonGiveUp.Show();
                return;
            }
            if (host.Role == EUserRole.STUDENT && project.Status == EProjectStatus.PROCESSING)
            {
                gGradientButtonGiveUp.Show();
                return;
            }
        }
        private void SetButtonEditOrDetails()
        {
            if (host.Role == EUserRole.STUDENT || project.Status == EProjectStatus.COMPLETED)
            {
                gButtonEdit.Hide();
            }
            else
            {
                gButtonEdit.Show();
            }
        }
        private void SetTeamHere(bool flag)
        {
            if (flag)
            {
                ptbEmptyState.Hide();
                lblThere.Hide();
                showTeam.Show();
            }
            else
            {
                showTeam.Hide();
                ptbEmptyState.Show();
                lblThere.Show();
            }
        }
        private void SetStudentRegister(bool flag)
        {
            if (flag == false) return;

            HideAllButtonMode();
            uCProjectDetailsCreatedTeam = new UCProjectDetailsCreatedTeam(this.host, this.project);
            uCProjectDetailsCreatedTeam.GPerform.Click += GPerformState_Click;
            gPanelDataView.Controls.Clear();
            gPanelDataView.Controls.Add(uCProjectDetailsCreatedTeam);
        }
        private void SetNewState(EProjectStatus status, Image image, string notification)
        {
            this.project.Status = status;
            gTextBoxStatus.Text = EnumUtil.GetDisplayName(project.Status);
            gTextBoxStatus.FillColor = project.GetStatusColor();
            gButtonEdit.Hide();
            SetButtonComplete();

            HideAllButtonMode();
            gPictureBoxState.Image = image;
            gTextBoxState.Text = notification;
            gPanelDataView.Controls.Clear();
            gPanelDataView.Controls.Add(gPictureBoxState);
            gPanelDataView.Controls.Add(gTextBoxState);

            if (status == EProjectStatus.GAVEUP)
            {
                gTextBoxState.ForeColor = Color.Gray;
            }
            else
            {
                gTextBoxState.ForeColor = Color.FromArgb(0, 192, 192);
            }

            gPanelDataView.Focus();
        }
        private void HideAllButtonMode()
        {
            gGradientButtonTasks.Hide();
            gGradientButtonStatistics.Hide();
            gGradientButtonMeetings.Hide();
            gGradientButtonRegistered.Hide();
        }
        private void AllButtonStandardColor()
        {
            GunaControlUtil.ButtonStandardColor(gGradientButtonRegistered, Color.White, Color.White);
            GunaControlUtil.ButtonStandardColor(gGradientButtonTasks, Color.White, Color.White);
            GunaControlUtil.ButtonStandardColor(gGradientButtonStatistics, Color.White, Color.White);
            GunaControlUtil.ButtonStandardColor(gGradientButtonMeetings, Color.White, Color.White);
        }
        public void PerformNotificationClick(Notification notification)
        {
            this.notification = notification;
            if (this.notification.Type == ENotificationType.MEETING)
            {
                gGradientButtonMeetings.PerformClick();
                return;
            }

            if (this.notification.Type != ENotificationType.PROJECT)
            {
                gGradientButtonTasks.PerformClick();
                UCProjectDetailsTasks uCProjectDetailsTasks = new UCProjectDetailsTasks();
                uCProjectDetailsTasks.SetUpUserControl(host, instructor, team, project, project.Status == EProjectStatus.PROCESSING);
                // uCProjectDetailsTasks.PerformNotificationClick(notification.Type);
            }
        }

        #endregion

        #region EVENT gButtonEdit

        private void gButtonEdit_Click(object sender, EventArgs e)
        {
            FProjectEdit fProjectView = new FProjectEdit(UserDAO.SelectOnlyByID(project.CreatedBy), project);
            fProjectView.ShowDialog();
            ResetProjectInfor();
            this.flagEdited = true;
            this.project = ProjectDAO.SelectOnly(project.ProjectId);
        }

        #endregion

        #region EVENT gButtonDetails

        private void gButtonDetails_Click(object sender, EventArgs e)
        {
            FProjectView fProjectView = new FProjectView(new ProjectMeta(project, this.isFavorite));
            fProjectView.ShowDialog();
        }

        #endregion

        #region EVENT gGradientButtonComplete

        private void gGradientButtonComplete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("You have definitely completed the " + project.Topic + " project",
                                                    "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                ProjectDAO.UpdateStatus(this.project, EProjectStatus.COMPLETED);

                string content = Notification.GetContentCompletedProject(project.Topic);
                NotificationDAO.InsertFollowTeam(this.team.TeamId, content, ENotificationType.PROJECT);

                this.flagEdited = true;
                SetNewState(EProjectStatus.COMPLETED, Properties.Resources.GiftCompleted, "Congratulations on completion !");
            }
        }

        #endregion

        #region EVENT gGradientButtonGiveUp

        private void gGradientButtonGiveUp_Click(object sender, EventArgs e)
        {
            FGiveUp fGiveUp = new FGiveUp(new ProjectMeta(this.project, this.isFavorite), this.host, this.team);
            fGiveUp.ConfirmedGivingUp += FGiveUp_ConfirmedGivingUp;
            fGiveUp.ShowDialog();
        }
        private void FGiveUp_ConfirmedGivingUp(object? sender, EventArgs e)
        {
            if (this.host.Role == EUserRole.STUDENT)
            {
                this.flagEdited = true;
                ProjectDAO.UpdateStatus(this.project, EProjectStatus.WAITING);
                SetUpDataViewState();
                SetNewState(EProjectStatus.WAITING, Properties.Resources.PictureEmptyState, "Waiting for Lecture confirm !");
            }
            else
            {
                ProjectDAO.UpdateStatus(this.project, EProjectStatus.GAVEUP);
                GiveUpDAO.UpdateStatus(this.project.ProjectId, EGiveUpStatus.ACCEPTED, EGiveUpStatus.PENDING);
                SetNewState(EProjectStatus.GAVEUP, Properties.Resources.PictureEmptyState, "The project cannot continue !");
            }

            string content = Notification.GetContentTypeGiveUp(team.TeamName, project.Topic);
            NotificationDAO.InsertFollowTeam(this.team.TeamId, content, ENotificationType.GIVEUP);

            if (this.host.Role == EUserRole.STUDENT)
            {
                Notification notification = new Notification("Notification", content, ENotificationType.GIVEUP, DateTime.Now);
                NotificationDAO.Insert(notification, project.InstructorId);
            }
        }

        #endregion

        #region EVENT gGradientButtonReasonDetails

        private void gGradientButtonReasonDetails_Click(object sender, EventArgs e)
        {
            FGiveUp fGiveUp = new FGiveUp(new ProjectMeta(this.project, this.isFavorite), this.host, this.team);
            GiveUp giveUp = GiveUpDAO.SelectFollowProject(project.ProjectId);
            fGiveUp.SetReadOnly(giveUp);
            fGiveUp.ShowDialog();
        }

        #endregion

        #region EVENT gGradientButtonTasks

        private void gGradientButtonTasks_Click(object sender, EventArgs e)
        {
            AllButtonStandardColor();
            GunaControlUtil.ButtonSettingColor(gGradientButtonTasks);
            UCProjectDetailsTasks uCProjectDetailsTasks = new UCProjectDetailsTasks();
            uCProjectDetailsTasks.SetUpUserControl(host, instructor, team, project, project.Status == EProjectStatus.PROCESSING);
            gPanelDataView.Controls.Clear();
            gPanelDataView.Controls.Add(uCProjectDetailsTasks);
        }

        #endregion

        #region EVENT gGradientButtonStatistical

        private void gGradientButtonStatistical_Click(object sender, EventArgs e)
        {
            AllButtonStandardColor();
            GunaControlUtil.ButtonSettingColor(gGradientButtonStatistics);
            uCProjectDetailsStatistical.SetUpUserControl(this.team);
            gPanelDataView.Controls.Clear();
            gPanelDataView.Controls.Add(uCProjectDetailsStatistical);
        }

        #endregion

        #region EVENT gGradientButtonMeeting

        private void gGradientButtonMeeting_Click(object sender, EventArgs e)
        {
            AllButtonStandardColor();
            GunaControlUtil.ButtonSettingColor(gGradientButtonMeetings);
            UCProjectDetailsMeeting uCProjectDetailsMeeting = new UCProjectDetailsMeeting();
            uCProjectDetailsMeeting.SetUpUserControl(host, project, team);
            gPanelDataView.Controls.Clear();
            gPanelDataView.Controls.Add(uCProjectDetailsMeeting);
        }

        #endregion

        #region EVENT gGradientButtonRegistered

        private void gGradientButtonRegistered_Click(object sender, EventArgs e)
        {
            AllButtonStandardColor();
            GunaControlUtil.ButtonSettingColor(gGradientButtonRegistered);
            gPanelDataView.Controls.Clear();

            this.teams = TeamDAO.SelectList(this.project.ProjectId);

            uCProjectDetailsRegistered.Clear();
            foreach (Team team in teams)
            {
                UCTeamLine line = new UCTeamLine(team, host);
                line.ProjectAddAccepted += ProjectAddAccepted_Clicked;
                uCProjectDetailsRegistered.AddTeam(line);
            }

            gPanelDataView.Controls.Add(uCProjectDetailsRegistered);
        }
        private void ProjectAddAccepted_Clicked(object sender, EventArgs e)
        {
            UCTeamLine line = sender as UCTeamLine;

            if (line != null)
            {
                Team team = line.GetTeam;
                DialogResult result = MessageBox.Show("Are you sure you want to accept team " + team.TeamName,
                                                        "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    this.teams.Remove(team);

                    TeamDAO.DeleteListTeam(this.teams);
                    string rejectedContent = Notification.GetContentTypeRejected(host.FullName, project.Topic);
                    foreach (Team t in this.teams)
                    {
                        NotificationDAO.InsertFollowTeam(t.TeamId, rejectedContent, ENotificationType.PROJECT);
                    }

                    ProjectDAO.UpdateStatus(this.project, EProjectStatus.PROCESSING);

                    this.flagEdited = true;
                    this.project.Status = EProjectStatus.PROCESSING;

                    string acceptedContent = Notification.GetContentTypeAccepted(host.FullName, project.Topic);
                    NotificationDAO.InsertFollowTeam(team.TeamId, acceptedContent, ENotificationType.PROJECT);

                    SetInitialSate();
                    SetButtonComplete();
                    SetButtonEditOrDetails();
                    gTextBoxStatus.Text = EnumUtil.GetDisplayName(project.Status);
                    gTextBoxStatus.FillColor = project.GetStatusColor();
                }
            }
        }

        #endregion

        #region EVENTHANDER GPerformState

        private void GPerformState_Click(object? sender, EventArgs e)
        {
            SetSuccessfullyRegistered();
            this.flagDeleted = true;
            this.project = ProjectDAO.SelectOnly(project.ProjectId);
            gTextBoxStatus.Text = EnumUtil.GetDisplayName(project.Status);
            gTextBoxStatus.FillColor = project.GetStatusColor();
        }

        #endregion

        private void gGradientButtonConfirm_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("You confirm that you allow " + team.TeamName + " team to abandon the " + project.Topic + " project",
                                                        "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                this.flagEdited = true;
                ProjectDAO.UpdateStatus(this.project, EProjectStatus.GAVEUP);
                GiveUpDAO.UpdateStatus(this.project.ProjectId, EGiveUpStatus.ACCEPTED, EGiveUpStatus.PENDING);
                SetNewState(EProjectStatus.GAVEUP, Properties.Resources.PictureEmptyState, "The project cannot continue !");

                string content = Notification.GetContentTypeGiveUpAccepted(team.TeamName, project.Topic);
                NotificationDAO.InsertFollowTeam(this.team.TeamId, content, ENotificationType.GIVEUP);
            }
        }

        private void gGradientButtonReject_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("You do not agree to let " + team.TeamName + " abandon the " + project.Topic + " project",
                                                        "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                ProjectDAO.UpdateStatus(this.project, EProjectStatus.PROCESSING);
                GiveUpDAO.Delete(project.ProjectId);
                InitUserControl();

                string content = Notification.GetContentTypeGiveUpRejected(team.TeamName, project.Topic);
                NotificationDAO.InsertFollowTeam(this.team.TeamId, content, ENotificationType.GIVEUP);
            }
        }
    }
}
