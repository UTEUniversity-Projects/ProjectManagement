using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementModel.Models
{
    internal class Technology
    {
        private string technologyId;
        private string name;
        public Technology()
        {
            technologyId = string.Empty;
            name = string.Empty;
        }
        public Technology(string technologyId, string name)
        {
            this.technologyId = technologyId;
            this.name = name;
        }
        public string TechnologyId
        {
            get { return technologyId; }
            set { technologyId = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
