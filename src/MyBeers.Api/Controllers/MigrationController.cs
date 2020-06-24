using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBeers.Api.Base;
using MyBeers.Common.Dispatchers;
using MyBeers.Migration.Api.Commands;

namespace MyBeers.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class MigrationController : BaseController
    {
        public MigrationController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher) : base(queryDispatcher, commandDispatcher)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Migrating([FromBody]MigrateDataCommand migrateDataCommand)
        {
            try
            {
                await CommandDispatcher.DispatchAsync(migrateDataCommand);
                return AcceptedAtAction(nameof(Migrating));
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

    }
}