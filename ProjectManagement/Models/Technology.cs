using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Models
{
    public class Technology
    {

        #region TECHNOLOGY ATTRIBUTES

        private string technologyId;
        private string name;

        #endregion

        #region TECHNOLOGY CONSTRUCTORS

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

        #endregion

        #region TECHNOLOGY PROPERTIES

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

        #endregion

    }
}
