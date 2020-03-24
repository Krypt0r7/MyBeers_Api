using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBeers.Api.Services;

namespace MyBeers.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SystemetController : Controller
    {
        private readonly ISystemetService _systemetService;
        public SystemetController(ISystemetService systemetService)
        {
            _systemetService = systemetService;
        }
        [HttpGet]
        public async Task<IActionResult> Search(string search)
        {
            return Ok(await _systemetService.SearchSystemetAsync(search));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> SearchBeer(int id)
        {
            return Ok(await _systemetService.SearchSingleBeer(id));
        }
        [HttpGet("news")]
        public async Task<IActionResult> NewsAsync(string region)
        {
            var beers = await _systemetService.GetNews(region);
            return Ok(beers);
        }
    }
}