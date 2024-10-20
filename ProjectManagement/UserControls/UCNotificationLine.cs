using Guna.UI2.WinForms;
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
using ProjectManagement.MetaData;
using ProjectManagement.Utils;

namespace ProjectManagement
{
    public partial class UCNotificationLine : UserControl
    {
        
        public event EventHandler NotificationLineClicked;
        public event EventHandler NotificationDeleteClicked;

        private Users user = new Users();
        private NotificationMeta notificationMeta = new NotificationMeta();

        private Color lineColor = Color.White;

        public UCNotificationLine(Users user, NotificationMeta notificationMeta)
        {
            InitializeComponent();
            this.user = user;
            this.notificationMeta = notificationMeta;
            InitUserControl();
        }
        public NotificationMeta GetNotificationMeta
        {
            get { return this.notificationMeta; }
        }
        private void InitUserControl()
        {
            lblNotification.Text = DataTypeUtil.FormatStringLength(notificationMeta.Notification.Content.ToString(), 130);
            lblTime.Text = notificationMeta.Notification.CreatedAt.ToString("dd/MM/yyyy hh:mm:ss tt");
            gTextBoxType.Text = EnumUtil.GetDisplayName(notificationMeta.Notification.Type);
            gTextBoxType.FillColor = notificationMeta.Notification.GetTypeColor();
            GunaControlUtil.SetItemFavorite(gButtonStar, notificationMeta.IsFavorite);

            if (notificationMeta.IsSaw)
            {
                this.lineColor = Color.FromArgb(222, 224, 224);
                this.BackColor = lineColor;
            }
        }
        private void SetColor(Color color)
        {
            this.BackColor = color;
        }
        public void PerformClicked()
        {
            this.lineColor = Color.FromArgb(222, 224, 224);
            this.BackColor = lineColor;

            if (notificationMeta.IsSaw == false)
            {
                notificationMeta.IsSaw = true;
                NotificationDAO.UpdateIsSaw(user.UserId, notificationMeta.Notification.NotificationId, true);
            }
        }
        private void UCNotificationLine_MouseEnter(object sender, EventArgs e)
        {
            SetColor(Color.Gainsboro);
        }
        private void UCNotificationLine_MouseLeave(object sender, EventArgs e)
        {
            SetColor(this.lineColor);
        }
        private void UCNotificationLine_Click(object sender, EventArgs e)
        {
            PerformClicked();
            OnNotificationLineClicked(EventArgs.Empty);
        }
        private void OnNotificationLineClicked(EventArgs e)
        {
            NotificationLineClicked?.Invoke(this, e);
        }
        private void gButtonStar_Click(object sender, EventArgs e)
        {
            notificationMeta.IsFavorite = !notificationMeta.IsFavorite;
            NotificationDAO.UpdateFavorite(user.UserId, notificationMeta.Notification.NotificationId, notificationMeta.IsFavorite);
            GunaControlUtil.SetItemFavorite(gButtonStar, notificationMeta.IsFavorite);
        }
        private void gButtonDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete notificationMeta " + notificationMeta.Notification.NotificationId,
                                                    "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                NotificationDAO.Delete(user.UserId, notificationMeta.Notification.NotificationId);
                OnNotificationDeleteClicked(EventArgs.Empty);
            }
        }
        private void OnNotificationDeleteClicked(EventArgs e)
        {
            NotificationDeleteClicked?.Invoke(this, e);
        }
    }
}
