using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.MetaData
{
    public class TaskMeta
    {
        private Tasks task;
        private bool isFavorite;

        public TaskMeta()
        {
            this.task = new Tasks();
            this.isFavorite = false;
        }
        public TaskMeta(Tasks task, bool isFavorite)
        {
            this.task = task;
            this.isFavorite = isFavorite;
        }

        public Tasks Task 
        { 
            get { return this.task; } 
            set { this.task = value; }
        }
        public bool IsFavorite
        {
            get { return this.isFavorite; }
            set { this.isFavorite = value; }
        }
    }
}
