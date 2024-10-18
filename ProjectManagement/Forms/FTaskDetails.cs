using Guna.UI2.WinForms;
using ProjectManagement.DAOs;
using ProjectManagement.Models;
using ProjectManagement.Process;
using ProjectManagement.Enums;
using ProjectManagement.Utils;

namespace ProjectManagement.Forms
{
    public partial class FTaskDetails : Form
    {
        

        private Users host = new Users();
        private Users creator = new Users();
        private Users instructor = new Users();
        private Project project = new Project();
        private Tasks task = new Tasks();
        private Team team = new Team();
        private Tasks dynamicTask = new Tasks();

        private TaskDAO TaskDAO = new TaskDAO();
        private EvaluationDAO EvaluationDAO = new EvaluationDAO();

        private UCTaskComment uCTaskComment = new UCTaskComment();
        private UCTaskEvaluateList uCTaskEvaluateList = new UCTaskEvaluateList();
        private UCTaskEvaluateDetails uCTaskEvaluateDetails = new UCTaskEvaluateDetails();
        private UCUserMiniLine peopleLineClicked = new UCUserMiniLine();
        private bool isProcessing = true;
        private bool flagCheck = false;
        private bool edited = false;

        public FTaskDetails(Users host, Users instructor, Project project, Tasks task, Users creator, Team team, bool isProcessing)
        {
            InitializeComponent();
            this.host = host;
            this.instructor = instructor;
            this.project = project;
            this.task = task;
            this.creator = creator;
            this.team = team;
            this.isProcessing = isProcessing;
            SetUpUserControl();
        }

        #region PROPERTIES

        public bool Edited
        {
            get { return this.edited; }
        }

        #endregion

        #region FUNCTIONS

        private void SetUpUserControl()
        {
            this.dynamicTask = task.Clone();
            InitUserControl();
            SetViewState();
        }
        private void InitUserControl()
        {
            lblCreator.Text = creator.FullName;
            gTextBoxTitle.Text = task.Title;
            gTextBoxDescription.Text = task.Description;
            gTextBoxProgress.Text = task.Progress.ToString();
            gCirclePictureBoxCreator.Image = WinformControlUtil.NameToImage(creator.Avatar);
            // GunaControlUtil.SetItemFavorite(gButtonStar, task.IsFavorite);

            if (!isProcessing || (host.Role == EUserRole.STUDENT && task.CreatedBy != host.UserId))
            {
                gButtonEdit.Hide();
                gButtonStar.Location = new Point(383, 17);
            }

            uCTaskComment.SetUpUserControl(host, instructor, project, task, isProcessing);
            gShadowPanelView.Controls.Add(uCTaskComment);

            uCTaskEvaluateList.SetUpUserControl(project, task, team, host);
            uCTaskEvaluateList.ClickEvaluate += Line_ClickEvaluate;
            gShadowPanelView.Controls.Add(uCTaskEvaluateList);

            uCTaskEvaluateDetails.GButtonBack.Click += GButtonBack_Click;
            gShadowPanelView.Controls.Add(uCTaskEvaluateDetails);

            gGradientButtonComment.PerformClick();
        }
        private void SetViewState()
        {
            gButtonCancel.Hide();
            gButtonSave.Hide();
            GunaControlUtil.SetTextBoxState(new List<Guna2TextBox> { gTextBoxTitle, gTextBoxDescription, gTextBoxProgress }, true);
        }
        private void SetEditState()
        {
            gButtonCancel.Show();
            gButtonSave.Show();
            GunaControlUtil.SetTextBoxState(new List<Guna2TextBox> { gTextBoxTitle, gTextBoxDescription, gTextBoxProgress }, false);
        }
        private bool CheckInformationValid()
        {
            WinformControlUtil.RunCheckDataValid(task.CheckTitle() || flagCheck, erpTitle, gTextBoxTitle, "Title cannot be empty");
            WinformControlUtil.RunCheckDataValid(task.CheckDescription() || flagCheck, erpDescription, gTextBoxDescription, "Description cannot be empty");
            int progress = 0;
            bool checkProgress = int.TryParse(gTextBoxProgress.Text, out progress);
            if (checkProgress) task.Progress = progress;
            WinformControlUtil.RunCheckDataValid((checkProgress && task.CheckProgress()) || flagCheck, erpProgress, gTextBoxProgress, "Can only take values from 0 to 100");

            return task.CheckTitle() && task.CheckDescription() && (checkProgress && task.CheckProgress());
        }
        private void AllButtonStandardColor()
        {
            GunaControlUtil.ButtonStandardColor(gGradientButtonComment, Color.White, Color.White);
            GunaControlUtil.ButtonStandardColor(gGradientButtonEvaluate, Color.White, Color.White);
        }
        public void PerformNotificationClick(Notification notification)
        {
            if (notification.Type == ENotificationType.EVALUATION) gGradientButtonEvaluate.PerformClick();
            else gGradientButtonComment.PerformClick();
        }

        #endregion

        #region EVENT BUTTON CLICK

        private void gButtonEdit_Click(object sender, EventArgs e)
        {
            SetEditState();
        }
        private void gButtonCancel_Click(object sender, EventArgs e)
        {
            gTextBoxTitle.Text = task.Title;
            gTextBoxDescription.Text = task.Description;
            gTextBoxProgress.Text = task.Progress.ToString();
            SetViewState();
        }
        private void gButtonSave_Click(object sender, EventArgs e)
        {
            this.flagCheck = false;
            if (CheckInformationValid())
            {
                this.task = new Tasks(task.TaskId, DateTime.MinValue, DateTime.MaxValue, gTextBoxTitle.Text, gTextBoxDescription.Text,
                    double.Parse(gTextBoxProgress.Text), ETaskPriority.LOW, task.CreatedAt, this.creator.UserId, this.project.ProjectId);

                TaskDAO.Update(task);
                this.flagCheck = true;
                this.edited = true;
                SetViewState();
            }
        }
        private void gGradientButtonComment_Click(object sender, EventArgs e)
        {
            AllButtonStandardColor();
            GunaControlUtil.ButtonSettingColor(gGradientButtonComment);
            uCTaskEvaluateList.Hide();
            uCTaskEvaluateDetails.Hide();
            uCTaskComment.Show();
        }
        private void gGradientButtonEvaluate_Click(object sender, EventArgs e)
        {
            AllButtonStandardColor();
            GunaControlUtil.ButtonSettingColor(gGradientButtonEvaluate);
            uCTaskComment.Hide();
            uCTaskEvaluateDetails.Hide();
            uCTaskEvaluateList.Show();
        }

        #endregion

        #region EVENT TEXTCHANGED

        private void gTextBoxTitle_TextChanged(object sender, EventArgs e)
        {
            this.dynamicTask.Title = gTextBoxTitle.Text;
            WinformControlUtil.RunCheckDataValid(dynamicTask.CheckTitle() || flagCheck, erpTitle, gTextBoxTitle, "Title cannot be empty");
        }
        private void gTextBoxDescription_TextChanged(object sender, EventArgs e)
        {
            this.dynamicTask.Description = gTextBoxDescription.Text;
            WinformControlUtil.RunCheckDataValid(dynamicTask.CheckDescription() || flagCheck, erpDescription, gTextBoxDescription, "Description cannot be empty");
        }
        private void gTextBoxProgress_TextChanged(object sender, EventArgs e)
        {
            int progress = 0;
            bool checkProgress = int.TryParse(gTextBoxProgress.Text, out progress);
            if (checkProgress)
            {
                task.Progress = progress;
                gProgressBarToLine.Value = progress;
            }
            WinformControlUtil.RunCheckDataValid((checkProgress && task.CheckProgress()) || flagCheck, erpProgress, gTextBoxProgress, "Can only take values from 0 to 100");
        }

        #endregion

        #region METHOD UCTASK EVALUATE

        private void Line_ClickEvaluate(object sender, EventArgs e)
        {
            UCUserMiniLine line = uCTaskEvaluateList.GetUserLine;

            if (line != null)
            {
                this.peopleLineClicked = line;
                uCTaskEvaluateDetails.SetUpUserControl(project, task, line.GetUser, line.GetEvaluation, host, isProcessing);
                uCTaskComment.Hide();
                uCTaskEvaluateList.Hide();
                uCTaskEvaluateDetails.Show();
            }
        }
        private void GButtonBack_Click(object sender, EventArgs e)
        {
            Evaluation evaluation = EvaluationDAO.SelectOnly(task.TaskId, this.peopleLineClicked.GetUser.UserId);
            this.peopleLineClicked.SetEvaluateMode(evaluation, true);
            gGradientButtonEvaluate.PerformClick();
        }

        #endregion

    }
}
