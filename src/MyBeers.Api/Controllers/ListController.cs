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
    [Route("[controller]/[action]")]
    [ApiController]
    public class ListController : BaseController
    {
        public ListController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher) : base(queryDispatcher, commandDispatcher)
        {
        }

        [HttpGet]
        public async Task<IActionResult> ListsFromUser([FromQuery]ListsByUserIdQuery listsByUserIdQuery)
        { 
            var lists = await QueryDispatcher.DispatchAsync<ListsByUserIdQuery, IEnumerable<ListsByUserIdQuery.ListByUserId>>(listsByUserIdQuery);
            return Ok(lists);
        }

        [HttpGet]
        public async Task<IActionResult> ListFromId([FromQuery]ListFromIdQuery query)
        {
            var list = await QueryDispatcher.DispatchAsync<ListFromIdQuery, ListFromIdQuery.List>(query);

            return Ok(list);
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
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateListInfo([FromBody]UpdateListInfoCommand updateListCommand)
        {
            try
            {
                await CommandDispatcher.DispatchAsync(updateListCommand);
                return AcceptedAtAction(nameof(UpdateListInfo));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateBeers([FromBody]UpdateListCommand updateListCommand)
        {
            try
            {
                await CommandDispatcher.DispatchAsync(updateListCommand);
                return Accepted();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteList([FromBody]DeleteListCommand deleteListCommand)
        {
            try
            {
                await CommandDispatcher.DispatchAsync(deleteListCommand);
                return AcceptedAtAction(nameof(DeleteList));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
