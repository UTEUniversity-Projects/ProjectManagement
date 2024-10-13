using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Mappers.Implement
{
    internal class TechnologyMapper : IRowMapper<Technology>
    {
        public Technology MapRow(DataRow row)
        {
            string technologyId = row["technologyId"].ToString();
            string name = row["name"].ToString();

            Technology technology = new Technology(technologyId, name);

            return technology;
        }
    }
}
