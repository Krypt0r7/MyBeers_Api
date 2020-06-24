using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBeers.Api.Base;
using MyBeers.Common.Dispatchers;
using MyBeers.RatingLib.Api.Commands;
using MyBeers.RatingLib.Api.Queries;

namespace MyBeers.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class RatingController : BaseController
    {
        public RatingController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher) : base(queryDispatcher, commandDispatcher)
        {
        }

        [HttpPost]
        public async Task<IActionResult> CreateUpdate([FromBody]CreateUpdateRatingCommand createRatingCommand)
        {
            try
            {
                await CommandDispatcher.DispatchAsync(createRatingCommand);

                return AcceptedAtAction(nameof(CreateUpdate), new { beerId = createRatingCommand.BeerId }, null);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        public IActionResult Ratings([FromQuery]RatingsQuery ratingsQuery)
        {
            try
            {
                var ratings = QueryDispatcher.Dispatch<RatingsQuery, IEnumerable<RatingsQuery.Rating>>(ratingsQuery);
                return Ok(ratings.ToList());
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        [HttpGet]
        public IActionResult Rating([FromQuery]RatingQuery ratingQuery)
        {
            try
            {
                var rating = QueryDispatcher.Dispatch<RatingQuery, RatingQuery.Rating>(ratingQuery);
                return Ok(rating);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public IActionResult ByBeer([FromQuery]RatingsByBeerQuery ratingsByBeerQuery)
        {
            try
            {
                var ratings = QueryDispatcher.Dispatch<RatingsByBeerQuery, IEnumerable<RatingsByBeerQuery.Rating>>(ratingsByBeerQuery);
                return Ok(ratings);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }




    }
}