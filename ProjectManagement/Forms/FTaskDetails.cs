using Guna.UI2.WinForms;
using ProjectManagement.DAOs;
using ProjectManagement.Models;
using ProjectManagement.MetaData;
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
        private TaskMeta taskMeta = new TaskMeta();
        private Team team = new Team();
        private Tasks dynamicTask = new Tasks();

        private UCTaskComment uCTaskComment = new UCTaskComment();
        private UCTaskEvaluateList uCTaskEvaluateList = new UCTaskEvaluateList();
        private UCTaskEvaluateDetails uCTaskEvaluateDetails = new UCTaskEvaluateDetails();
        private UCUserMiniLine peopleLineClicked = new UCUserMiniLine();
        private bool isProcessing = true;
        private bool flagCheck = false;
        private bool edited = false;

        public FTaskDetails(Users host, Users instructor, Project project, TaskMeta taskMeta, Users creator, Team team, bool isProcessing)
        {
            InitializeComponent();
            this.host = host;
            this.instructor = instructor;
            this.project = project;
            this.taskMeta = taskMeta;
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
            this.dynamicTask = taskMeta.Task.Clone();
            InitUserControl();
            SetViewState();
        }
        private void InitUserControl()
        {
            lblCreator.Text = creator.FullName;
            gTextBoxTitle.Text = taskMeta.Task.Title;
            gTextBoxDescription.Text = taskMeta.Task.Description;
            gTextBoxProgress.Text = taskMeta.Task.Progress.ToString();
            gCirclePictureBoxCreator.Image = WinformControlUtil.NameToImage(creator.Avatar);
            GunaControlUtil.SetItemFavorite(gButtonStar, taskMeta.IsFavorite);

            if (!isProcessing || (host.Role == EUserRole.STUDENT && taskMeta.Task.CreatedBy != host.UserId))
            {
                gButtonEdit.Hide();
                gButtonStar.Location = new Point(383, 17);
            }

            uCTaskComment.SetUpUserControl(host, instructor, project, taskMeta.Task, isProcessing);
            gShadowPanelView.Controls.Add(uCTaskComment);

            uCTaskEvaluateList.SetUpUserControl(project, taskMeta.Task, team, host);
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
            WinformControlUtil.RunCheckDataValid(taskMeta.Task.CheckTitle() || flagCheck, erpTitle, gTextBoxTitle, "Title cannot be empty");
            WinformControlUtil.RunCheckDataValid(taskMeta.Task.CheckDescription() || flagCheck, erpDescription, gTextBoxDescription, "Description cannot be empty");
            int progress = 0;
            bool checkProgress = int.TryParse(gTextBoxProgress.Text, out progress);
            if (checkProgress) taskMeta.Task.Progress = progress;
            WinformControlUtil.RunCheckDataValid((checkProgress && taskMeta.Task.CheckProgress()) || flagCheck, erpProgress, gTextBoxProgress, "Can only take values from 0 to 100");

            return taskMeta.Task.CheckTitle() && taskMeta.Task.CheckDescription() && (checkProgress && taskMeta.Task.CheckProgress());
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
            gTextBoxTitle.Text = taskMeta.Task.Title;
            gTextBoxDescription.Text = taskMeta.Task.Description;
            gTextBoxProgress.Text = taskMeta.Task.Progress.ToString();
            SetViewState();
        }
        private void gButtonSave_Click(object sender, EventArgs e)
        {
            this.flagCheck = false;
            if (CheckInformationValid())
            {
                this.taskMeta.Task = new Tasks(taskMeta.Task.TaskId, DateTime.MinValue, DateTime.MaxValue, gTextBoxTitle.Text, gTextBoxDescription.Text,
                    double.Parse(gTextBoxProgress.Text), ETaskPriority.LOW, taskMeta.Task.CreatedAt, this.creator.UserId, this.project.ProjectId);

                TaskDAO.Update(taskMeta.Task);
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
                taskMeta.Task.Progress = progress;
                gProgressBarToLine.Value = progress;
            }
            WinformControlUtil.RunCheckDataValid((checkProgress && taskMeta.Task.CheckProgress()) || flagCheck, erpProgress, gTextBoxProgress, "Can only take values from 0 to 100");
        }

        #endregion

        #region METHOD UCTASK EVALUATE

        private void Line_ClickEvaluate(object sender, EventArgs e)
        {
            UCUserMiniLine line = uCTaskEvaluateList.GetUserLine;

            if (line != null)
            {
                this.peopleLineClicked = line;
                uCTaskEvaluateDetails.SetUpUserControl(project, taskMeta.Task, line.GetUser, line.GetEvaluation, host, isProcessing);
                uCTaskComment.Hide();
                uCTaskEvaluateList.Hide();
                uCTaskEvaluateDetails.Show();
            }
        }
        private void GButtonBack_Click(object sender, EventArgs e)
        {
            Evaluation evaluation = EvaluationDAO.SelectOnly(taskMeta.Task.TaskId, this.peopleLineClicked.GetUser.UserId);
            this.peopleLineClicked.SetEvaluateMode(evaluation, true);
            gGradientButtonEvaluate.PerformClick();
        }

        #endregion

    }
}
