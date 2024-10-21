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
        public static GiveUp SelectFollowProject(string projectId)
        {
            return DBGetModel.GetModel(DBTableNames.GiveUp, "projectId", projectId, new GiveUpMapper());

        }
        public static void UpdateStatus(string projectId, EGiveUpStatus newStatus, EGiveUpStatus oldStatus)
        {
            string sqlStr = string.Format("UPDATE {0} SET status = @NewStatus WHERE projectId = @ProjectId and status = @OldStatus",
                DBTableNames.GiveUp);
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@NewStatus", EnumUtil.GetDisplayName(newStatus)),
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@OldStatus", EnumUtil.GetDisplayName(oldStatus))
            };
            DBExecution.SQLExecuteNonQuery(sqlStr, parameters, string.Empty);
        }
        public static void Insert(GiveUp giveUp)
        {
            DBExecution.Insert(giveUp, DBTableNames.GiveUp);
        }
        public static void Delete(string projectId)
        {
            DBExecution.Delete(DBTableNames.GiveUp, "projectId", projectId);
        }
    }
}
