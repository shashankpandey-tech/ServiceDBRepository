using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DBProxy.DBInfraRepository
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> ExecuteProcedure(string sql, IDbConnection dbConnection, List<SqlParameter> sqlParameters = null);

        Task<IEnumerable<T>> ExecuteProcedure_MultipleResultSet(string sql, IDbConnection dbConnection, int gridIndex, List<SqlParameter> sqlParameters = null);

        Task<IEnumerable<T>> ExecuteProcedure_AllResultSet(string sql, IDbConnection dbConnection, int gridIndex, List<SqlParameter> sqlParameters = null);

        Task<IEnumerable<T>> ExecuteQuery(string sql, IDbConnection dbConnection);
    }
}
