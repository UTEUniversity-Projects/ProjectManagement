using ProjectManagement.Enums;
using ProjectManagement.Models;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Mappers.Implement
{
    internal class GiveUpMapper : IRowMapper<GiveUp>
    {
        public GiveUp MapRow(DataRow row)
        {
            string projectId = row["projectId"].ToString();
            string userId = row["userId"].ToString();
            string reason = row["reason"].ToString();
            DateTime createdAt = DateTime.Parse(row["createdAt"].ToString());
            EGiveUpStatus status = EnumUtil.GetEnumFromDisplayName<EGiveUpStatus>(row["status"].ToString());

            GiveUp giveUp = new GiveUp(projectId, userId, reason, createdAt, status);
            return giveUp;
        }
    }
}
