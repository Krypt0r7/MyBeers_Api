using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MyBeers.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ListController : Controller
    {
        private readonly IListService _listservice;
        public ListController(IListService listservice)
        {
            _listservice = listservice;
        }


        public async Task<IActionResult> Lists(string userId)
        {

            var lists = await _listservice.GetListsAsync(userId);
            foreach (var list in lists)
            {
                
            }
            return Ok(lists);
        }

    }
}
