using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IRatingService _ratingService;
        public BeerController(
            IBeerService beerService, 
            IUserService userService, 
            IRatingService ratingService,
            IMapper mapper)
        {
            _beerService = beerService;
            _userService = userService;
            _ratingService = ratingService;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> BeersByUSerAsync()
        {
            var userId = HttpContext.User.Identity.Name;

            var user = await _userService.GetByIdAsync(userId);

            var beers = await _beerService.GetUsersBeerAsync(user.BeerIds);

            var beersDto = _mapper.Map<List<BeerQueryDto>>(beers);

            var userRatings = await _ratingService.GetRatingsByUserId(user.Id);

            foreach (var beer in beersDto)
            {
                beer.Rating = _mapper.Map<RatingQueryDto>(userRatings.FirstOrDefault(f => f.BeerId == beer.Id));
            }

            return Ok(beersDto);
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