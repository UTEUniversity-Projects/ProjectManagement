using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Models
{
    internal class Media
    {

        #region MEDIA ATTRIBUTES

        private string mediaId;
        private string saveCode;
        private string type;
        private DateTime createdAt;

        #endregion

        #region MEDIA CONSTRUCTORS

        public Media()
        {
            mediaId = string.Empty;
            saveCode = string.Empty;
            type = string.Empty;
            createdAt = DateTime.Now;
        }
        public Media(string mediaId, string saveCode, string type, DateTime createdAt)
        {
            this.mediaId = mediaId;
            this.saveCode = saveCode;
            this.type = type;
            this.createdAt = createdAt;
        }

        #endregion

        #region MEDIA PROPERTIES

        public string MediaId
        {
            get { return this.mediaId; }
            set { this.mediaId = value; }
        }
        public string SaveCode
        {
            get { return this.saveCode; }
            set { this.saveCode = value; }
        }
        public string Type
        {
            get { return this.type; }
            set { this.type = value; }
        }
        public DateTime CreatedAt
        {
            get { return this.createdAt; }
            set { this.createdAt = value; }
        }

        #endregion

    }
}
