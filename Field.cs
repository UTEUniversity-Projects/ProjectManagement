using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementModel.Models
{
    internal class Field
    {
        private string fieldId;
        private string name;
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
        public string FieldId 
        { 
            get {  return fieldId; } 
            set { fieldId = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
