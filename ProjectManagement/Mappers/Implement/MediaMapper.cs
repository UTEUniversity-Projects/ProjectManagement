using ProjectManagement.DAOs;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Mappers.Implement
{
    internal class MediaMapper : IRowMapper<Media>
    {
        public Media MapRow(DataRow row)
        {
            string mediaId = row["mediaId"].ToString();
            string saveCode = row["saveCode"].ToString();
            string type = row["type"].ToString();
            DateTime createdAt = DateTime.Parse(row["createdAt"].ToString());

            Media media = new Media(mediaId, saveCode, type, createdAt);

            return media;
        }
    }
}
