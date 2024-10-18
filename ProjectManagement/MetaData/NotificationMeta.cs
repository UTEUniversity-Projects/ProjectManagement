using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.MetaData
{
    public class NotificationMeta
    {
        private Notification notification;
        private bool isSaw;

        public NotificationMeta()
        {
            this.notification = new Notification();
            this.isSaw = false;
        }
        public NotificationMeta(Notification notification, bool isSaw)
        {
            this.notification = notification;
            this.isSaw = isSaw;
        }

        public Notification Notification { get { return this.notification; } }
        public bool IsSaw { 
            get { return this.isSaw; }
            set { this.isSaw = value; }
        }
    }
}
