using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBProxy.DomainModel;
using MediatR;
namespace DBProxy.Application.Command
{
    public class HistoryCommand : IRequest<IEnumerable<History>>
    {
        public string Number { get;private set; }
        public HistoryCommand(string number)
        {
            Number = number;
        }
    }
}
