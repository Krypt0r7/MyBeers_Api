using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBeers.Common.Dispatchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.Api.Base
{
    public class BaseController : Controller
    {
        protected IQueryDispatcher QueryDispatcher { get; }
        protected ICommandDispatcher CommandDispatcher { get; }
        public BaseController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            QueryDispatcher = queryDispatcher;
            CommandDispatcher = commandDispatcher;
        }

    }
}
