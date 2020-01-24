using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBeers.Api.Dtos;
using MyBeers.Api.Services;

namespace MyBeers.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class BeerController : Controller
    {
        private readonly IBeerService _beerService;
        public BeerController(IBeerService beerService)
        {
            _beerService = beerService;
        }
        [HttpGet]
        public async Task<IActionResult> BeersAsync()
        {
            var user = HttpContext.User.Identity.Name;
            var beers = await _beerService.GetBeersAsync(user);
            return Ok(beers);
        }

        [HttpPost]
        public async Task<IActionResult> SaveBeerAsync([FromBody]SystemetDto systemetDto)
        {
            var user = HttpContext.User.Identity.Name;
            var beer = await _beerService.SaveBeerAsync(systemetDto, user);
            return Ok(beer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFavAsync(string id)
        {
            var beer = await _beerService.UpdateFavouriteAsync(id);
            if (beer == null)
                return BadRequest("not found");
            return Ok(beer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var result = await _beerService.DeleteAsync(id);
            if (!result.IsAcknowledged)
                return BadRequest("not deleted");
            if (result == null)
                return BadRequest("not found");
            return Ok();
        }
    }
}