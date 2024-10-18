using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Parameters
{
    public interface ISqlParameter<T>
    {
        List<SqlParameter> GetSqlParameters(T model);
    }
}
