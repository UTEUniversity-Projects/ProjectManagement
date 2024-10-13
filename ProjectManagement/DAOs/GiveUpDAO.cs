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
using System.Data.SqlClient;

namespace ProjectManagement.DAOs
{
    internal class GiveUpDAO : DBConnection
    {
        public static GiveUp SelectFollowProject(string projectId)
        {
            return DBGetModel.GetModel(DBTableNames.GiveUp, "projectId", projectId, new GiveUpMapper());

        }
        public static void Insert(GiveUp giveUp)
        {
            DBExecution.Insert(giveUp, DBTableNames.GiveUp);
        }
    }
}
