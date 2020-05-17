using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace DBProxy.DBInfraRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public async Task<IEnumerable<T>> ExecuteProcedure(string sql, IDbConnection dbConnection, List<SqlParameter> sqlParameters = null)
        {
            using (dbConnection)
            {
                DynamicParameters dp = new DynamicParameters();
                if (sqlParameters != null)
                {

                    foreach (var sp in sqlParameters)
                    {
                        dp.Add(sp.ParameterName, sp.SqlValue == null ? sp.Value : sp.SqlValue);
                    }
                }
                dbConnection.Open();
                return await dbConnection.QueryAsync<T>(sql, dp, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<T>> ExecuteProcedure_AllResultSet(string sql, IDbConnection dbConnection, int gridIndex, List<SqlParameter> sqlParameters = null)
        {
            using (dbConnection)
            {
                List<T> response = new List<T>();
                DynamicParameters dp = new DynamicParameters();
                if (sqlParameters != null)
                {
                    foreach (var sp in sqlParameters)
                    {
                        dp.Add(sp.ParameterName, sp.SqlValue == null ? sp.Value : sp.SqlValue);
                    }
                }
                dbConnection.Open();
                var result = await dbConnection.QueryMultipleAsync(sql, dp, commandType: CommandType.StoredProcedure);

                for (int i = 0; i < gridIndex; i++)
                {
                    response.AddRange(result.Read<T>());
                }
                return response;
            }
        }

        public async Task<IEnumerable<T>> ExecuteProcedure_MultipleResultSet(string sql, IDbConnection dbConnection, int gridIndex, List<SqlParameter> sqlParameters = null)
        {
            using (dbConnection)
            {
                DynamicParameters dp = new DynamicParameters();
                if (sqlParameters != null)
                {

                    foreach (var sp in sqlParameters)
                    {
                        dp.Add(sp.ParameterName, sp.SqlValue == null ? sp.Value : sp.SqlValue);
                    }
                }
                dbConnection.Open();
                var result = await dbConnection.QueryMultipleAsync(sql, dp, commandType: CommandType.StoredProcedure);
                dynamic response = null;
                for (int i = 0; i < gridIndex; i++)
                {
                    response = result.Read<T>();
                }
                return response;
            }
        }

        public async Task<IEnumerable<T>> ExecuteQuery(string sql, IDbConnection dbConnection)
        {
            using (dbConnection)
            {
                dbConnection.Open();
                return await dbConnection.QueryAsync<T>(sql, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
