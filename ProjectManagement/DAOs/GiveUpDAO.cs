using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Database;
using ProjectManagement.Models;
using ProjectManagement.Mappers.Implement;
using ProjectManagement.Utils;
using System.Data.SqlClient;
using ProjectManagement.Enums;

namespace ProjectManagement.DAOs
{
    internal class GiveUpDAO : DBConnection
    {
        #region CHECK INFORMATIONS
        public static bool CheckIsNotEmpty(string input, string fieldName)
        {
            string sqlStr = "SELECT * FROM dbo.FUNC_IsNotEmpty(@Input, @FieldName)";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
            new SqlParameter("@Input", input),
            new SqlParameter("@FieldName", fieldName)
            };
            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);
            return dataTable.Rows.Count > 0 && Convert.ToBoolean(dataTable.Rows[0]["IsValid"]);
        }
        #endregion
        public static GiveUp SelectFollowProject(string projectId)
        {
            string sqlStr = "SELECT * FROM FUNC_SelectGiveUpByProjectId(@ProjectId)";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ProjectId", projectId)
            };
            DataTable dataTable = DBExecution.SQLExecuteQuery(sqlStr, parameters, string.Empty);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                GiveUpMapper giveUpMapper = new GiveUpMapper();
                return giveUpMapper.MapRow(dataTable.Rows[0]);
            }
            return null;
        }
        public static void UpdateStatus(string projectId, EGiveUpStatus newStatus, EGiveUpStatus oldStatus)
        {
            string sqlStr = "EXEC PROC_UpdateGiveUpStatus @ProjectId, @NewStatus, @OldStatus";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@NewStatus", EnumUtil.GetDisplayName(newStatus)),
                new SqlParameter("@OldStatus", EnumUtil.GetDisplayName(oldStatus))
            };

            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }
        public static void Insert(GiveUp giveUp)
        {
            string sqlStr = "EXEC PROC_InsertGiveUp @ProjectId, @UserId, @Reason, @CreatedAt, @Status";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ProjectId", giveUp.ProjectId),
                new SqlParameter("@UserId", giveUp.UserId),
                new SqlParameter("@Reason", giveUp.Reason),
                new SqlParameter("@CreatedAt", giveUp.CreatedAt),
                new SqlParameter("@Status", EnumUtil.GetDisplayName(giveUp.Status))
            };
            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, "Insert GiveUp");
        }
        public static void Delete(string projectId)
        {
            string sqlStr = "EXEC PROC_DeleteGiveUpByProjectId @ProjectId";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ProjectId", projectId)
            };
            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }
    }
}
