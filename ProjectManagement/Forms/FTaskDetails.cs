using Guna.UI2.WinForms;
using ProjectManagement.DAOs;
using ProjectManagement.Models;
using ProjectManagement.MetaData;
using ProjectManagement.Enums;
using ProjectManagement.Utils;
using ProjectManagement.UserControls;

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

        private UCTaskDetails uCTaskDetails = new UCTaskDetails();
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
            InitUserControl();
        }

        #region PROPERTIES

        public bool Edited
        {
            get { return this.uCTaskDetails.Edited; }
        }

        #endregion

        #region FUNCTIONS

        private void InitUserControl()
        {
            uCTaskDetails.SetUpUserControl(host, instructor, project, taskMeta, creator, team, isProcessing);
            gShadowPanelView.Controls.Add(uCTaskDetails);

            uCTaskComment.SetUpUserControl(host, instructor, project, taskMeta.Task, isProcessing);
            gShadowPanelView.Controls.Add(uCTaskComment);

            uCTaskEvaluateList.SetUpUserControl(taskMeta.Task, host);
            uCTaskEvaluateList.ClickEvaluate += Line_ClickEvaluate;
            gShadowPanelView.Controls.Add(uCTaskEvaluateList);

            uCTaskEvaluateDetails.GButtonBack.Click += GButtonBack_Click;
            gShadowPanelView.Controls.Add(uCTaskEvaluateDetails);

            gGradientButtonDetails.PerformClick();
        }


       
        private void AllButtonStandardColor()
        {
            GunaControlUtil.ButtonStandardColor(gGradientButtonComment, Color.White, Color.White);
            GunaControlUtil.ButtonStandardColor(gGradientButtonEvaluate, Color.White, Color.White);
            GunaControlUtil.ButtonStandardColor(gGradientButtonDetails, Color.White, Color.White);
        }
        public void PerformNotificationClick(Notification notification)
        {
            if (notification.Type == ENotificationType.EVALUATION) gGradientButtonEvaluate.PerformClick();
            else gGradientButtonComment.PerformClick();
        }

        #endregion

        #region EVENT BUTTON CLICK
                
        private void gGradientButtonComment_Click(object sender, EventArgs e)
        {
            AllButtonStandardColor();
            GunaControlUtil.ButtonSettingColor(gGradientButtonComment);
            uCTaskEvaluateList.Hide();
            uCTaskEvaluateDetails.Hide();
            uCTaskDetails.Hide();
            uCTaskComment.Show();
        }
        private void gGradientButtonEvaluate_Click(object sender, EventArgs e)
        {
            AllButtonStandardColor();
            GunaControlUtil.ButtonSettingColor(gGradientButtonEvaluate);
            uCTaskComment.Hide();
            uCTaskEvaluateDetails.Hide();
            uCTaskDetails.Hide();
            uCTaskEvaluateList.Show();
        }
        private void gGradientButtonDetails_Click(object sender, EventArgs e)
        {
            AllButtonStandardColor();
            GunaControlUtil.ButtonSettingColor(gGradientButtonDetails);
            uCTaskComment.Hide();
            uCTaskEvaluateDetails.Hide();
            uCTaskDetails.Show();
            uCTaskEvaluateList.Hide();
        }
        #endregion

        #region EVENT TEXTCHANGED


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
