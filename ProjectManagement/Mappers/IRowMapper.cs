using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Mappers
{
    public interface IRowMapper<T>
    {
        T MapRow(DataRow row);
    }
}
