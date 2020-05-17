using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBProxy.Application.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DBProxy.Controllers
{
    [ApiController]
    [Route("service")]
    public class DBServiceController : ControllerBase
    {
        private readonly ILogger<DBServiceController> _logger;
        private readonly IQuery _query;

        public DBServiceController(ILogger<DBServiceController> logger, IQuery query)
        {
            _logger = logger;
            _query = query;
        }

        [Route("History/{phoneNumber}")]
        [HttpGet]
        public async Task<IActionResult> Get(string phoneNumber)
        {
            return Ok(await _query.GetHistoryByNumber(phoneNumber));
        }
    }
}
