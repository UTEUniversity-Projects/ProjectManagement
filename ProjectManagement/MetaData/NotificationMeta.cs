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
        private bool isFavorite;

        public NotificationMeta()
        {
            this.notification = new Notification();
            this.isSaw = false;
            this.isFavorite = false;
        }
        public NotificationMeta(Notification notification, bool isSaw, bool isFavorite)
        {
            this.notification = notification;
            this.isSaw = isSaw;
            this.isFavorite = isFavorite;
        }

        public Notification Notification { get { return this.notification; } }
        public bool IsSaw 
        { 
            get { return this.isSaw; }
            set { this.isSaw = value; }
        }
        public bool IsFavorite
        {
            get { return this.isFavorite; }
            set { this.isFavorite = value; }
        }
    }
}
