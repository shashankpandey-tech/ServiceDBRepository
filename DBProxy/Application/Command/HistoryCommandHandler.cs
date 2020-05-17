using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DBProxy.DBInfraRepository;
using DBProxy.DomainModel;
using DBProxy.Enumrations;
using MediatR;
namespace DBProxy.Application.Command
{
    public class HistoryCommandHandler : IRequestHandler<HistoryCommand, IEnumerable<History>>
    {
        private readonly Func<SqlConnectionType, IDbConnection> _connection;
        private readonly IRepository<History> _repository;
        public HistoryCommandHandler(Func<SqlConnectionType, IDbConnection> connection, IRepository<History> repository)
        {
            _repository = repository;
            _connection = connection;
        }
        public async Task<IEnumerable<History>> Handle(HistoryCommand request, CancellationToken cancellationToken)
        {
            List<SqlParameter> sqlparm = new List<SqlParameter>();
            sqlparm.Add(new SqlParameter() { ParameterName = "@PshoneNumber", Value = request.Number, SqlDbType = SqlDbType.VarChar });

            return await _repository.ExecuteProcedure("ProcName", _connection(SqlConnectionType.connectionstringRead), sqlparm);
        }
    }
}
