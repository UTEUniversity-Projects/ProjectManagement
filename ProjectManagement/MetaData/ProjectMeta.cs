using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.MetaData
{
    public class ProjectMeta
    {
        private Project project;
        private bool isFavorite;

        public ProjectMeta()
        {
            this.project = new Project();
            this.isFavorite = false;
        }
        public ProjectMeta(Project project, bool isFavorite)
        {
            this.project = project;
            this.isFavorite = isFavorite;
        }

        public Project Project { get { return project; } }
        public bool IsFavorite { get { return isFavorite; } }
    }
}
