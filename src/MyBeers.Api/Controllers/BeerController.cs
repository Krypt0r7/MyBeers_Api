using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBeers.Api.Data;
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
        private readonly IUserService _userService;
        public BeerController(IBeerService beerService, IUserService userService)
        {
            _beerService = beerService;
            _userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> BeersAsync()
        {
            var userId = HttpContext.User.Identity.Name;
            var user = await _userService.GetByIdAsync(userId);
            var beers = await _beerService.GetUsersBeerAsync(user);
            return Ok(beers);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewBeer(int productNumber)
        {
            var beer = await _beerService.SaveBeerProdNumberAsync(productNumber);
            if (beer == null)
                return BadRequest("ProductId yielded no result");

            return Ok(beer);
        }
   
    }
}