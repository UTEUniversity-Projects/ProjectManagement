using ProjectManagement.Database;
using ProjectManagement.Mappers.Implement;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.DAOs
{
    internal class FieldDAO
    {
        public static Field SelectOnlyById(string fieldId)
        {
            return DBGetModel.GetModel(DBTableNames.Field, "fieldId", fieldId, new FieldMapper());
        }
        public static List<Field> SelectList()
        {
            string sqlStr = string.Format("SELECT * FROM {0}", DBTableNames.Field);

            return DBGetModel.GetModelList(sqlStr, new List<SqlParameter>(), new FieldMapper());
        }
    }
}
