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
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class RatingController : Controller
    {
        private readonly IBeerService _beerService;
        private readonly IUserService _userService;
        private readonly IRatingService _ratingService;
        public RatingController(
            IBeerService beerService, 
            IUserService userService,
            IRatingService ratingService)
        {
            _beerService = beerService;
            _userService = userService;
            _ratingService = ratingService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRating(CreateRatingDto ratingDto)
        {
            try
            {
                var createRatingCommand = new CreateRatingCommand
                {
                    Beer = await _beerService.GetBeerByIdAsync(ratingDto.BeerId),
                    OverallRating = ratingDto.Rating,
                    User = await _userService.GetByIdAsync(HttpContext.User.Identity.Name)
                };

                await _ratingService.CreateRatingAsync(createRatingCommand);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> AllRatings()
        {
            return Ok();
        }

    }
}