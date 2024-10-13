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
using ProjectManagement.Process;
using ProjectManagement.Utils;

namespace ProjectManagement
{
    public partial class UCNotificationLine : UserControl
    {
        
        public event EventHandler NotificationLineClicked;
        public event EventHandler NotificationDeleteClicked;

        private Users user = new Users();
        private Notification notification = new Notification();

        private Color lineColor = Color.White;

        public UCNotificationLine(Notification notification)
        {
            InitializeComponent();
            this.notification = notification;
            InitUserControl();
        }
        public Notification GetNotification
        {
            get { return this.notification; }
        }
        private void InitUserControl()
        {
            lblNotification.Text = DataTypeUtil.FormatStringLength(notification.Content.ToString(), 130);
            lblFrom.Text = user.FullName;
            lblTime.Text = notification.CreatedAt.ToString("dd/MM/yyyy hh:mm:ss tt");
            gTextBoxType.Text = notification.Type.ToString();
            gTextBoxType.FillColor = notification.GetTypeColor();
            // GunaControlUtil.SetItemFavorite(gButtonStar, notification.IsFavorite);
            //if (notification.IsSaw)
            //{
            //    this.lineColor = Color.FromArgb(222, 224, 224);
            //    this.BackColor = lineColor;
            //}
        }
        private void SetColor(Color color)
        {
            this.BackColor = color;
        }
        public void PerformClicked()
        {
            this.lineColor = Color.FromArgb(222, 224, 224);
            this.BackColor = lineColor;
            //if (notification.IsSaw != true)
            //{
            //    notification.IsSaw = true;
            //    NotificationDAO.UpdateIsSaw(user.UserId, notification.NotificationId, true);
            //}
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
            // notification.IsFavorite = !notification.IsFavorite;
            // GunaControlUtil.SetItemFavorite(gButtonStar, notification.IsFavorite);
            // NotificationDAO.UpdateIsFavorite(notification.NotificationId, notification.IsFavorite);
        }
        private void gButtonDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete notification " + notification.NotificationId,
                                                    "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                NotificationDAO.Delete(notification);
                OnNotificationDeleteClicked(EventArgs.Empty);
            }
        }
        private void OnNotificationDeleteClicked(EventArgs e)
        {
            NotificationDeleteClicked?.Invoke(this, e);
        }
    }
}
