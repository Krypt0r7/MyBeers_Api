using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBeers.Api.Base;
using MyBeers.Common.Dispatchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.Api.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class QueryController<T> : BaseController where T : class
    {
        public QueryController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher) : base (queryDispatcher, commandDispatcher){}

        [HttpGet]
        [Authorize]
        public object Query([FromQuery]T value) => QueryAnonymous(value);

        [HttpGet]
        public object QueryAnonymous(T value)
        {
            try
            {
                return new { };
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
