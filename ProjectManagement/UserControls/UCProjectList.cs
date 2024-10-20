using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using ProjectManagement.Models;
using ProjectManagement.Process;

namespace ProjectManagement
{
    public partial class UCProjectList : UserControl
    {
        public event EventHandler ProjectLineClicked;
        private int numProject = 0;

        public UCProjectList()
        {
            InitializeComponent();
            gGradientButtonFilter.Hide();
        }

        #region PROPERTIES

        public Guna2GradientButton GButtonCreateProject
        {
            get { return this.gGradientButtonCreateProject; }
        }
        public Guna2GradientButton GButtonFavorite
        {
            get { return this.gGradientButtonTag; }
        }
        public Guna2GradientButton GButtonTopic
        {
            get { return this.gGradientButtonProjectTopic; }
        }
        public Guna2GradientButton GButtonFilter
        {
            get { return this.gGradientButtonFilter; }
        }
        public Guna2TextBox GTextBoxSearch
        {
            get { return this.gTextBoxSearch; }
        }
        public Guna2Button GButtonReset
        {
            get { return this.gButtonResetList; }
        }

        #endregion

        #region FUNCTIONS

        public void Clear()
        {
            flpProjectList.Controls.Clear();
        }
        public void AddProject(UCProjectLine projectLine)
        {
            projectLine.ProjectLineClicked += ProjectLine_Clicked;
            projectLine.ProjectDeleteClicked += ProjectDelete_Clicked;
            flpProjectList.Controls.Add(projectLine);
            SetNumProject(1, false);
        }
        public void SetNumProject(int num, bool flagReset)
        {
            if (flagReset) numProject = num; else numProject += num;
            lblNumProject.Text = numProject.ToString();
        }
        public void SetFilter(bool flag)
        {
            if (flag)
            {
                gGradientButtonFilter.Image = Properties.Resources.PicItemGradientFilter;
                gGradientButtonFilter.FillColor = SystemColors.ButtonFace;
                gGradientButtonFilter.FillColor2 = SystemColors.ButtonFace;
            }
            else
            {
                gGradientButtonFilter.Image = Properties.Resources.PicItemFilter;
                gGradientButtonFilter.FillColor = Color.White;
                gGradientButtonFilter.FillColor2 = Color.White;
            }
        }
        public void NotificationJump(Notification notification)
        {
            //foreach (UCProjectLine line in flpProjectList.Controls)
            //{
            //    if (line != null)
            //    {
            //        if (line.GetIdProject == notification.ProjectId)
            //        {
            //            line.PerformNotificationClick(notification);
            //            return;
            //        }
            //    }
            //}
        }


        #endregion

        #region METHOD

        private void ProjectLine_Clicked(object sender, EventArgs e)
        {
            OnProjectLineClicked(EventArgs.Empty);
        }
        private void OnProjectLineClicked(EventArgs e)
        {
            ProjectLineClicked?.Invoke(this, e);
        }
        public void ProjectDelete_Clicked(object sender, EventArgs e)
        {
            UCProjectLine line = sender as UCProjectLine;

            if (line != null)
            {
                foreach (Control control in flpProjectList.Controls)
                {
                    if (control.GetType() == typeof(UCProjectLine))
                    {
                        UCProjectLine projectLine = (UCProjectLine)control;
                        if (projectLine == line)
                        {
                            flpProjectList.Controls.Remove(control);
                            control.Dispose();
                            SetNumProject(-1, false);
                            break;
                        }
                    }
                }
            }
        }

        #endregion

    }
}
