using Microsoft.VisualBasic.ApplicationServices;
using ProjectManagement.Models;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjectManagement.MetaData;

namespace ProjectManagement.Forms
{
    public partial class FNotificationDetails : Form
    {
        private Notification notification = new Notification();
        private bool isFavorite = false;

        public FNotificationDetails(NotificationMeta notificationMeta)
        {
            InitializeComponent();
            this.notification = notificationMeta.Notification;
            this.isFavorite = notificationMeta.IsFavorite;
            InitUserControl();
        }

        private void InitUserControl()
        {
            lblTitle.Text = DataTypeUtil.FormatStringLength(notification.Title, 255);
            gTextBoxContent.Text = notification.Content;
            lblTime.Text = notification.CreatedAt.ToString("dd/MM/yyyy hh:mm:ss tt");
            gTextBoxType.Text = EnumUtil.GetDisplayName(notification.Type);
            gTextBoxType.FillColor = notification.GetTypeColor();
            GunaControlUtil.SetItemFavorite(gButtonStar, isFavorite);
        }
    }
}
