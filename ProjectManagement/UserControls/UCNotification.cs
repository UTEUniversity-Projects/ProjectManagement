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
using ProjectManagement.Forms;
using ProjectManagement.Models;
using ProjectManagement.MetaData;

namespace ProjectManagement
{
    public partial class UCNotification : UserControl
    {
        public event EventHandler NotificationJump;

        private Users current = new Users();
        private Notification notificationClicked = new Notification();
        private List<NotificationMeta> notifications = new List<NotificationMeta>();

        public UCNotification()
        {
            InitializeComponent();
        }
        public void SetInformation(Users current)
        {
            this.current = current;
            InitUserControl();
        }
        public void InitUserControl()
        {
            this.notifications = NotificationDAO.SelectList(current.UserId);
            LoadNotificationList();
        }
        public bool HasNewNotification()
        {
            return notifications.Count(n => n.IsSaw == false) > 0;
        }
        private void LoadNotificationList()
        {
            flpNotificationList.Controls.Clear();
            foreach (NotificationMeta notificationMeta in notifications)
            {
                UCNotificationLine line = new UCNotificationLine(this.current, notificationMeta);
                line.NotificationDeleteClicked += NotificationDelete_Clicked;
                line.NotificationLineClicked += NotificationLine_Clicked;
                flpNotificationList.Controls.Add(line);
            }
        }
        private void NotificationDelete_Clicked(object sender, EventArgs e)
        {
            UCNotificationLine line = sender as UCNotificationLine;

            if (line != null)
            {
                foreach (Control control in flpNotificationList.Controls)
                {
                    if (control.GetType() == typeof(UCNotificationLine))
                    {
                        UCNotificationLine notificationLine = (UCNotificationLine)control;
                        if (notificationLine == line)
                        {
                            flpNotificationList.Controls.Remove(control);
                            control.Dispose();
                            break;
                        }
                    }
                }
            }
        }
        private void NotificationLine_Clicked(object sender, EventArgs e)
        {
            UCNotificationLine line = (UCNotificationLine)sender;

            if (line != null)
            {
                FNotificationDetails fNotificationDetails = new FNotificationDetails(line.GetNotificationMeta);
                fNotificationDetails.ShowDialog();
            }

            //UCNotificationLine line = sender as UCNotificationLine;

            //if (line != null)
            //{
            //    this.notificationClicked = line.GetNotification;
            //    OnNotificationLineClicked(EventArgs.Empty);
            //}
        }
        private void OnNotificationLineClicked(EventArgs e)
        {
            NotificationJump?.Invoke(this.notificationClicked, e);
        }
        private void gGradientButtonMarkAll_Click(object sender, EventArgs e)
        {
            foreach (UCNotificationLine line in flpNotificationList.Controls)
            {
                line.PerformClicked();
            }
        }
    }
}
