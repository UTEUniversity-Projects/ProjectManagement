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

namespace ProjectManagement
{
    public partial class UCTaskEvaluateDetails : UserControl
    {
        private MyProcess myProcess = new MyProcess();
        private Project thesis = new Project();
        private Tasks tasks = new Tasks();
        private User student = new User();
        private User host = new User();
        private Evaluation evaluation = new Evaluation();

        private EvaluationDAO evaluationDAO = new EvaluationDAO();
        private NotificationDAO notificationDAO = new NotificationDAO();

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

        public void SetUpUserControl(Project thesis, Tasks tasks, User student, Evaluation evaluation, User host, bool isProcessing)
        {
            this.thesis = thesis;
            this.tasks = tasks;
            this.student = student;
            this.evaluation = evaluation;
            this.host = host;
            this.isProcessing = isProcessing;
            InitUserControl();
        }
        private void InitUserControl()
        {
            this.flagCheck = true;
            gCirclePictureBoxAvatar.Image = myProcess.NameToImage(student.AvatarName);
            lblUserName.Text = myProcess.FormatStringLength(student.FullName, 40);
            lblPeopleCode.Text = student.IdAccount;
            gTextBoxEvaluation.Text = evaluation.Content;
            gProgressBarToLine.Value = evaluation.Contribute;
            gTextBoxCompleted.Text = evaluation.Contribute.ToString();
            gTextBoxScores.Text = evaluation.Scores.ToString();
            gTextBoxStatus.Text = evaluation.IsEvaluated ? ("Evaluated") : ("NotEvaluated");
            gTextBoxStatus.FillColor = evaluation.GetStatusColor();
            SetEditState(false);

            if (this.isProcessing)
            {
                if (host.Role == ERole.Lecture) SetEditState(true);
                else
                {
                    if (!evaluation.IsEvaluated) SetTextBoxEditState(gTextBoxCompleted, true);
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
            gTextBoxStatus.Text = evaluation.IsEvaluated ? ("Evaluated") : ("NotEvaluated");
            gTextBoxStatus.FillColor = evaluation.GetStatusColor();
            SetTextBoxEditState(gTextBoxEvaluation, flag);
            SetTextBoxEditState(gTextBoxCompleted, flag);
            SetTextBoxEditState(gTextBoxScores, flag);
        }
        private void UpdateContribute()
        {
            int contribution = 0;
            bool checkContribute = int.TryParse(gTextBoxCompleted.Text, out contribution);
            if (checkContribute) evaluation.Contribute = contribution;
            myProcess.RunCheckDataValid((checkContribute && evaluation.CheckContribute()) || flagCheck, erpContribute, gTextBoxCompleted, "Can only take values from 0 to 100");
        }
        private void UpdateScores()
        {
            float scores = 0.0F;
            bool checkScores = float.TryParse(gTextBoxScores.Text, out scores) || host.Role == ERole.Student;
            if (checkScores) evaluation.Scores = scores;
            myProcess.RunCheckDataValid((checkScores && evaluation.CheckScores()) || flagCheck, erpScores, gTextBoxScores, "Can only take values from 0.0 to 10.0");
        }
        private bool CheckInformationValid()
        {
            myProcess.RunCheckDataValid(evaluation.CheckContent() || flagCheck || host.Role == ERole.Student, erpEvaluation, gTextBoxEvaluation, "Comment be empty");
            UpdateContribute(); 
            UpdateScores();
            int contribution;
            float scores;
            return (evaluation.CheckContent() || flagCheck || host.Role == ERole.Student)
                && (int.TryParse(gTextBoxCompleted.Text, out contribution) && tasks.CheckProgress()) 
                && (float.TryParse(gTextBoxScores.Text, out scores) && evaluation.CheckScores());
        }

        #endregion

        #region EVENT FORM

        private void gButtonConfirm_Click(object sender, EventArgs e)
        {
            this.flagCheck = false;
            if (CheckInformationValid())
            {
                this.evaluation = new Evaluation(evaluation.IdEvaluation, evaluation.IdTask, evaluation.IdPeople, gTextBoxEvaluation.Text,
                                            int.Parse(gTextBoxCompleted.Text), float.Parse(gTextBoxScores.Text), DateTime.Now, host.Role == ERole.Lecture);

                evaluationDAO.Update(evaluation);
                if (host.Role == ERole.Lecture && evaluation.IsEvaluated)
                {
                    string content = Notification.GetContentTypeEvaluation(host.FullName, tasks.Title);
                    notificationDAO.Insert(new Notification(student.IdAccount, host.IdAccount, thesis.IdThesis, evaluation.IdEvaluation, content, DateTime.Now, false, false));
                }
                this.flagCheck = true;
                SetEditState(false);
            }
        }
        private void gTextBoxEvaluation_TextChanged(object sender, EventArgs e)
        {
            this.evaluation.Content = gTextBoxEvaluation.Text;
            myProcess.RunCheckDataValid(evaluation.CheckContent() || flagCheck || host.Role == ERole.Student, erpEvaluation, gTextBoxEvaluation, "Comment be empty");
        }
        private void gTextBoxContribute_TextChanged(object sender, EventArgs e)
        {
            UpdateContribute();
            int contribution = 0;
            if (int.TryParse(gTextBoxCompleted.Text, out contribution) && tasks.CheckProgress())
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
