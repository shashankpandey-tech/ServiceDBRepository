using DBProxy.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBProxy.Application.Queries
{
    public interface IQuery
    {
        Task<IEnumerable<History>> GetHistoryByNumber(string number);
    }
}
