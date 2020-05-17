using DBProxy.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using DBProxy.Application.Command;

namespace DBProxy.Application.Queries
{
    public class Query : IQuery
    {
        private readonly IMediator _mediator;
        public Query(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IEnumerable<History>> GetHistoryByNumber(string number)
        {
            return await _mediator.Send(new HistoryCommand(number));
        }
    }
}
