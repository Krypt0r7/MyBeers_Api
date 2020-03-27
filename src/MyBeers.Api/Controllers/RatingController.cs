using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
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
            var userId = HttpContext.User.Identity.Name;
            try
            {
                var createRatingCommand = new CreateRatingCommand
                {
                    Beer = await _beerService.GetBeerByIdAsync(ratingDto.BeerId),
                    OverallRating = ratingDto.OverallRating,
                    AfterTaste = ratingDto.AfterTaste,
                    Chugability = ratingDto.Chugability,
                    FirstImpression = ratingDto.FirstImpression,
                    Taste = ratingDto.Taste,
                    Value = ratingDto.Value,
                    Description = ratingDto.Description,
                    UserId = userId
                };
                await _ratingService.CreateRatingAsync(createRatingCommand);

                var ratings = await _ratingService.GetRatingsByUserId(userId);
                var rating = ratings.FirstOrDefault(x => x.BeerId == ratingDto.BeerId);
                var ratingNewDto = new RatingAndUsersQueryDto
                {
                    Description = rating.Description,
                    OverallRating = rating.OverallRating,
                    AfterTaste = rating.AfterTaste,
                    Chugability = rating.Chugability,
                    FirstImpression = rating.FirstImpression,
                    Taste = rating.Taste,
                    CreatedTime = rating.CreatedTime,
                    Id = rating.Id,
                };
                ratingNewDto.User = _mapper.Map<UserDto>(await _userService.GetByIdAsync(rating.UserId));

                return Ok(ratingNewDto);
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
                    AfterTaste = rating.AfterTaste,
                    Chugability = rating.Chugability,
                    FirstImpression = rating.FirstImpression,
                    Taste = rating.Taste,
                    Value = rating.Value,
                    Description = rating.Description,
                    User = _mapper.Map<UserDto>(users.First(x => x.Id == rating.UserId))
                });
            }

            return Ok(ratingList);
        }


        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateRatingAsync(UpdateRatingCommand updateRatingCommand, [FromRoute]string id)
        {
            var result = await _ratingService.UpdateRatingAsync(id, updateRatingCommand);
            if (result.IsAcknowledged)
            {
                var rating = await _ratingService.GetRatingAsync(id);
                var ratingDto = new RatingAndUsersQueryDto
                {
                    Description = rating.Description,
                    OverallRating = rating.OverallRating,
                    AfterTaste = rating.AfterTaste,
                    Chugability = rating.Chugability,
                    FirstImpression = rating.FirstImpression,
                    Taste = rating.Taste,
                    Value = rating.Value,
                    CreatedTime = rating.CreatedTime,
                    Id = rating.Id,
                    User = _mapper.Map<UserDto>(await _userService.GetByIdAsync(rating.UserId))
                };
                return Ok(ratingDto);
            }
            return BadRequest("Update failed");
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRating(string id)
        {
            var result = await _ratingService.DeleteRatingAsync(id);
            if (result.IsAcknowledged)
                return Ok("Rating removed");
            return BadRequest("Deletion faulty");
        }

    }
}