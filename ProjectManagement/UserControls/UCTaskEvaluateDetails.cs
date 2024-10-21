using Guna.UI2.WinForms;
using Guna.UI2.WinForms.Enums;
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
using ProjectManagement.Models;
using ProjectManagement.Process;
using ProjectManagement.Enums;
using ProjectManagement.Utils;

namespace ProjectManagement
{
    public partial class UCTaskEvaluateDetails : UserControl
    {
        
        private Tasks task = new Tasks();
        private Users student = new Users();
        private Users host = new Users();
        private Evaluation evaluation = new Evaluation();

        private bool isProcessing = true;
        private bool flagCheck = false;

        public UCTaskEvaluateDetails()
        {
            InitializeComponent();
        }

        #region PROPERTIES

        public Guna2Button GButtonBack
        {
            get { return gButtonBack; }
        }

        #endregion

        #region FUNCTIONS

        public void SetUpUserControl(Project project, Tasks task, Users student, Evaluation evaluation, Users host, bool isProcessing)
        {
            this.task = task;
            this.student = student;
            this.evaluation = evaluation;
            this.host = host;
            this.isProcessing = isProcessing;
            InitUserControl();
        }
        private void InitUserControl()
        {
            this.flagCheck = true;
            gCirclePictureBoxAvatar.Image = WinformControlUtil.NameToImage(student.Avatar);
            lblUserName.Text = DataTypeUtil.FormatStringLength(student.FullName, 40);
            lblUserCode.Text = student.UserId;
            gTextBoxEvaluation.Text = evaluation.Content;
            gProgressBarToLine.Value = (int)evaluation.CompletionRate;
            gTextBoxCompleted.Text = evaluation.CompletionRate.ToString();
            gTextBoxScores.Text = evaluation.Score.ToString();
            gTextBoxStatus.Text = evaluation.Evaluated ? ("Evaluated") : ("NotEvaluated");
            gTextBoxStatus.FillColor = evaluation.GetStatusColor();
            SetEditState(false);

            if (this.isProcessing)
            {
                if (host.Role == EUserRole.LECTURE) SetEditState(true);
                else
                {
                    if (!evaluation.Evaluated) SetTextBoxEditState(gTextBoxCompleted, true);
                    else
                    {
                        SetTextBoxEditState(gTextBoxCompleted, false);
                        gButtonCancel.Hide();
                        gButtonConfirm.Hide();
                    }
                }
            }
            else
            {
                gButtonCancel.Hide();
                gButtonConfirm.Hide();
            }
        }
        private void SetTextBoxEditState(Guna2TextBox textBox, bool flag)
        {
            if (flag) 
            {
                textBox.FillColor = Color.FromArgb(242, 245, 244);
                textBox.BorderThickness = 1;
                textBox.ReadOnly = false;
            }
            else
            {
                textBox.FillColor = SystemColors.ButtonFace;
                textBox.BorderThickness = 0;
                textBox.ReadOnly = true;
            }
        }
        private void SetEditState(bool flag)
        {
            gTextBoxStatus.Text = evaluation.Evaluated ? ("Evaluated") : ("NotEvaluated");
            gTextBoxStatus.FillColor = evaluation.GetStatusColor();
            SetTextBoxEditState(gTextBoxEvaluation, flag);
            SetTextBoxEditState(gTextBoxCompleted, flag);
            SetTextBoxEditState(gTextBoxScores, flag);
        }
        private void UpdateContribute()
        {
            int contribution = 0;
            bool checkContribute = int.TryParse(gTextBoxCompleted.Text, out contribution);
            if (checkContribute) evaluation.CompletionRate = contribution;
            WinformControlUtil.RunCheckDataValid((checkContribute && evaluation.CheckCompletionRate()) || flagCheck, erpContribute, gTextBoxCompleted, "Can only take values from 0 to 100");
        }
        private void UpdateScores()
        {
            float scores = 0.0F;
            bool checkScores = float.TryParse(gTextBoxScores.Text, out scores) || host.Role == EUserRole.STUDENT;
            if (checkScores) evaluation.Score = scores;
            WinformControlUtil.RunCheckDataValid((checkScores && evaluation.CheckScore()) || flagCheck, erpScores, gTextBoxScores, "Can only take values from 0.0 to 10.0");
        }
        private bool CheckInformationValid()
        {
            WinformControlUtil.RunCheckDataValid(evaluation.CheckContent() || flagCheck || host.Role == EUserRole.STUDENT, erpEvaluation, gTextBoxEvaluation, "Comment be empty");
            UpdateContribute(); 
            UpdateScores();
            int contribution;
            float scores;
            return (evaluation.CheckContent() || flagCheck || host.Role == EUserRole.STUDENT)
                && (int.TryParse(gTextBoxCompleted.Text, out contribution) && task.CheckProgress()) 
                && (float.TryParse(gTextBoxScores.Text, out scores) && evaluation.CheckScore());
        }

        #endregion

        #region EVENT FORM

        private void gButtonConfirm_Click(object sender, EventArgs e)
        {
            this.flagCheck = false;
            if (CheckInformationValid())
            {
                this.evaluation = new Evaluation(evaluation.EvaluationId, gTextBoxEvaluation.Text, double.Parse(gTextBoxCompleted.Text), 
                    double.Parse(gTextBoxScores.Text), true, DateTime.Now, evaluation.CreatedBy, this.student.UserId, evaluation.TaskId);

                EvaluationDAO.Update(evaluation);

                if (host.Role == EUserRole.LECTURE && evaluation.Evaluated)
                {
                    string content = Notification.GetContentTypeEvaluation(host.FullName, task.Title);
                    Notification notification = new Notification("Evaluated", content, Notification.GetNotificationType(evaluation.EvaluationId), DateTime.Now);
                    NotificationDAO.Insert(notification, evaluation.StudentId);
                }
                this.flagCheck = true;
                SetEditState(false);
            }
        }
        private void gTextBoxEvaluation_TextChanged(object sender, EventArgs e)
        {
            this.evaluation.Content = gTextBoxEvaluation.Text;
            WinformControlUtil.RunCheckDataValid(evaluation.CheckContent() || flagCheck || host.Role == EUserRole.STUDENT, erpEvaluation, gTextBoxEvaluation, "Comment be empty");
        }
        private void gTextBoxContribute_TextChanged(object sender, EventArgs e)
        {
            UpdateContribute();
            int contribution = 0;
            if (int.TryParse(gTextBoxCompleted.Text, out contribution) && task.CheckProgress())
            {
                gProgressBarToLine.Value = contribution;
            }
        }
        private void gTextBoxScores_TextChanged(object sender, EventArgs e)
        {
            UpdateScores();
        }
        private void gButtonCancel_Click(object sender, EventArgs e)
        {
            gButtonBack.PerformClick();
        }

        #endregion

    }
}
