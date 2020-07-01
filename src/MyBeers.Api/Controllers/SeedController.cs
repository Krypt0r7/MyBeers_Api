using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBeers.Api.Base;
using MyBeers.BeerLib.Api.Commands;
using MyBeers.Common.Dispatchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SeedController : BaseController
    {
        
        public SeedController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher) : base(queryDispatcher, commandDispatcher)
        {
        }

        [HttpPost("beer")]
        public async Task<IActionResult> SeedBeerAsync(SeedBeerCommand seedBeerCommand)
        {
            try
            {
                await CommandDispatcher.DispatchAsync(seedBeerCommand);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

           
        }

        [HttpPost("addImages")]
        public async Task<IActionResult> AddImagesToBeer(AddImageToBeerCommand addImageToBeerCommand)
        {
            try
            {   
                await CommandDispatcher.DispatchAsync<AddImageToBeerCommand>(addImageToBeerCommand);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }



    }
}
