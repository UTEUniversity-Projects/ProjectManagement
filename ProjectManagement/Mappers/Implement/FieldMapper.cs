using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Models;

namespace ProjectManagement.Mappers.Implement
{
    internal class FieldMapper : IRowMapper<Field>
    {
        public Field MapRow(DataRow row)
        {
            string fieldId = row["fieldId"].ToString();
            string name = row["name"].ToString();

            Field field = new Field(fieldId, name);

            return field;
        }
    }
}
