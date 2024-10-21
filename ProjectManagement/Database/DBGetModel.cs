using ProjectManagement.Mappers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Database
{
    internal class DBGetModel
    {
        #region GET ONE MODEL

        public static T GetModel<T>(string sqlStr, List<SqlParameter> parameters, IRowMapper<T> mapper)
        {
            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);

            if (dataTable == null || dataTable.Rows.Count == 0) return default;

            return GetModelFromDataRow(dataTable.Rows[0], mapper);
        }
        public static T GetModel<T>(string tableName, string primaryKeyName, string primaryKeyValue, IRowMapper<T> mapper)
        {
            string sqlStr = string.Format("SELECT * FROM {0} WHERE {1} = @Value", tableName, primaryKeyName);

            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@Value", primaryKeyValue) };

            return GetModel(sqlStr, parameters, mapper);
        }

        #endregion

        #region GET MODEL LIST

        public static List<T> GetModelList<T>(string sqlStr, List<SqlParameter> parameters, IRowMapper<T> mapper)
        {
            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);
            List<T> models = new List<T>();

            foreach (DataRow row in dataTable.Rows)
            {
                models.Add(GetModelFromDataRow(row, mapper));
            }
            
            return models;
        }
        public static List<T> GetModelList<T>(string tableName, string primaryKeyName, string primaryKeyValue, IRowMapper<T> mapper)
        {
            string sqlStr = string.Format("SELECT * FROM {0} WHERE {1} = @Value", tableName, primaryKeyName);

            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@Value", primaryKeyValue) };

            return GetModelList(sqlStr, parameters, mapper);
        }

        #endregion

        #region GET FROM DATA-ROW

        public static T GetModelFromDataRow<T>(DataRow row, IRowMapper<T> mapper)
        {
            return mapper.MapRow(row);
        }

        #endregion

    }
}
