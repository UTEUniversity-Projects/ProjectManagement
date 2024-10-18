using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Models
{
    public class Field
    {

        #region FIELD ATTRIBUTES

        private string fieldId;
        private string name;

        #endregion

        #region FIELD CONSTRUCTORS

        public Field()
        {
            fieldId = string.Empty;
            name = string.Empty;
        }
        public Field(string fieldId, string name)
        {
            this.fieldId = fieldId;
            this.name = name;
        }

        #endregion

        #region FIELD PROPERTIES

        public string FieldId
        {
            get { return fieldId; }
            set { fieldId = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        #endregion

    }
}
