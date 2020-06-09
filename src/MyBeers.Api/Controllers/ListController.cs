using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyBeers.ListLib.Api.Commands;
using MyBeers.Common.Dispatchers;
using MyBeers.ListLib.Api.Queries;
using MyBeers.Api.Base;

namespace MyBeers.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ListController : BaseController
    {
        public ListController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher) : base(queryDispatcher, commandDispatcher)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Lists([FromQuery]ListsByUserIdQuery listsByUserIdQuery)
        { 
            var lists = await QueryDispatcher.DispatchAsync<ListsByUserIdQuery, IEnumerable<ListsByUserIdQuery.ListByUserId>>(listsByUserIdQuery);
            return Ok(lists);
        }

        [HttpPost]
        public async Task<IActionResult> CreateList([FromBody]CreateListCommand createListCommand)
        {
            try
            {
                await CommandDispatcher.DispatchAsync(createListCommand);
                return CreatedAtAction(nameof(CreateList), new { userId = createListCommand.UserId, listName = createListCommand.Name, version = 1 }, null);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> AddBeerToList([FromBody]UpdateListCommand updateListCommand)
        {
            try
            {
                await CommandDispatcher.DispatchAsync(updateListCommand);
                return Accepted();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
