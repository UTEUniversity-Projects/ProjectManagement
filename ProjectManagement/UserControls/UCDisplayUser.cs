using Guna.UI2.WinForms;
using ProjectManagement.Models;
using ProjectManagement.Process;
using ProjectManagement.Enums;
using ProjectManagement.Utils;
using ProjectManagement.DAOs;

namespace ProjectManagement
{
    public partial class UCDisplayUser : UserControl
    {
        
        private Users user = new Users();

        private UCDashboard uCDashboard = new UCDashboard();
        private UCDashboard uCMyTheses = new UCDashboard();
        private UCNotification uCNotification = new UCNotification();
        private UCAccount uCAccount = new UCAccount();

        private List<Guna2Button> listButton = new List<Guna2Button>();
        private List<Image> listImage = new List<Image>();

        public UCDisplayUser()
        {
            InitializeComponent();

            pnlAddUserControl.Controls.Clear();
            pnlAddUserControl.Controls.Add(new UCWelcome());
            uCMyTheses.FlagStuMyTheses = true;
            this.listButton = new List<Guna2Button> { gButtonDashboard, gButtonMyProjects, gButtonNotification, gButtonAccount };
            this.listImage = new List<Image> { Properties.Resources.PictureTask, Properties.Resources.PictureProject,
                                                Properties.Resources.PictureNotification, Properties.Resources.PictureAccount };
        }

        #region PROPERTIES

        public Guna2Button GButtonLogOut
        {
            get { return this.gButtonLogOut; }
        }

        #endregion

        #region FUNCTIONS FORM

        public void SetInformation(Users user)
        {
            this.user = user;
            UserControlLoad();
        }
        private void UserControlLoad()
        {
            gCirclePictureBoxAvatar.Image = WinformControlUtil.NameToImage(user.Avatar);
            lblHandle.Text = user.UserName;
            lblRole.Text = EnumUtil.GetDisplayName(user.Role);
            GunaControlUtil.AllButtonStandardColor(this.listButton, this.listImage);

            pnlAddUserControl.Controls.Clear();
            pnlAddUserControl.Controls.Add(new UCWelcome(user));
            SetButtonBar();

            uCNotification.SetInformation(user);
            uCNotification.NotificationJump += NotificationType_Jump;
            if (uCNotification.HasNewNotification())
            {
                gButtonNotification.CustomImages.Image = Properties.Resources.PicNewNotification;
                gButtonNotification.Image = Properties.Resources.ItemDotNewNotification;
                gButtonNotification.FillColor = Color.White;
                gButtonNotification.ForeColor = Color.Black;
                gButtonNotification.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            }
        }
        private void SetButtonBar()
        {
            if (user.Role == EUserRole.LECTURE)
            {
                gButtonMyProjects.Hide();
                gButtonNotification.Location = new Point(22, 234);
                gButtonAccount.Location = new Point(22, 296);
            }
            else
            {
                gButtonMyProjects.Show();
                gButtonNotification.Location = new Point(22, 296);
                gButtonAccount.Location = new Point(22, 358);
            }
        }
        private void SetButtonClick(Guna2Button button, Image image, UserControl userControl)
        {
            GunaControlUtil.AllButtonStandardColor(this.listButton, this.listImage);
            GunaControlUtil.ButtonSettingColor(button);
            button.CustomImages.Image = image;
            pnlAddUserControl.Controls.Clear();
            pnlAddUserControl.Controls.Add(userControl);
        }

        #endregion

        #region CONTROLS CLICK

        private void gPanelBackAvatar_Click(object sender, EventArgs e)
        {
            GunaControlUtil.AllButtonStandardColor(this.listButton, this.listImage);
            this.user = UserDAO.SelectOnlyByID(user.UserId);
            pnlAddUserControl.Controls.Clear();
            pnlAddUserControl.Controls.Add(new UCWelcome(this.user));
        }
        private void gButtonDashboard_Click(object sender, EventArgs e)
        {
            uCDashboard.SetInformation(this.user);
            SetButtonClick(gButtonDashboard, Properties.Resources.PictureTaskGradient, uCDashboard);
        }
        private void gButtonMyTheses_Click(object sender, EventArgs e)
        {
            uCMyTheses.SetInformation(this.user);
            SetButtonClick(gButtonMyProjects, Properties.Resources.PictureProjectGradient, uCMyTheses);
        }
        private void gButtonNotification_Click(object sender, EventArgs e)
        {
            gButtonNotification.Image = null;
            gButtonNotification.FillColor = Color.Transparent;
            uCNotification.InitUserControl();
            SetButtonClick(gButtonNotification, Properties.Resources.PictureNotificationGradient, uCNotification);
        }
        private void gButtonAccount_Click(object sender, EventArgs e)
        {
            uCAccount.SetInformation(user);
            SetButtonClick(gButtonAccount, Properties.Resources.PictureAccountGradient, uCAccount);
        }

        #endregion

        #region METHOD NOTIFICATION JUMP

        private void NotificationType_Jump(object sender, EventArgs e)
        {
            Notification notification = sender as Notification;
            if (notification != null)
            {
                if (user.Role == EUserRole.LECTURE)
                {
                    gButtonDashboard.PerformClick();
                    uCDashboard.NotificationJump(notification);
                }
                else
                {
                    gButtonMyProjects.PerformClick();
                    uCMyTheses.NotificationJump(notification);
                }
            }
        }

        #endregion

    }
}
