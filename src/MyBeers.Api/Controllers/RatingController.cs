using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;
        public RatingController(
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
            var ratings = await _ratingService.GetRatingsAsync();
            var beers = await _beerService.GetAllBeersAsync();
            var users = await _userService.GetAsync();

            var ratingList = new List<RaingUserAndBeerDto>();

            foreach (var rating in ratings)
            {
                ratingList.Add(new RaingUserAndBeerDto
                {
                    Beer = _mapper.Map<BeerDto>(beers.First(x => x.Id == rating.BeerId)),
                    CreatedTime = rating.CreatedTime,
                    OverallRating = rating.OverallRating,
                    User = _mapper.Map<UserDto>(users.First(x => x.Id == rating.UserId))
                });
            }

            return Ok(ratingList.OrderByDescending(x => x.CreatedTime));
        }

    }
}