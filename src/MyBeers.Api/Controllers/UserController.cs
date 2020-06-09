using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBeers.Api.Base;
using MyBeers.Api.Exceptions;
using MyBeers.Api.Queries;
using MyBeers.Common.Dispatchers;
using MyBeers.UserLib.Api.Commands;
using MyBeers.UserLib.Api.Queries;
using MyBeers.UserLib.Domain;

namespace MyBeers.Api.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : BaseController
    {
        public UserController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher) : base(queryDispatcher, commandDispatcher)
        { 
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]CreateUserCommand createUserCommand)
        {
            try
            {
                await CommandDispatcher.DispatchAsync(createUserCommand);
                return CreatedAtAction(nameof(Register), new { username = createUserCommand.Username, version = 1 }, null);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody]AuthenticateUserQuery authenticateUser)
        {
            try
            {
                var user = await QueryDispatcher.DispatchAsync<AuthenticateUserQuery, AuthenticateUserQuery.Authentication>(authenticateUser);
                return Ok(user);
            }
            catch (UserException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetUser([FromQuery]UserQuery query)
        {
            try
            {
                var user = await QueryDispatcher.DispatchAsync<UserQuery, UserQuery.User>(query);
                if (user == null)
                    return BadRequest("User not found");
                return Ok(user);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
                
            }
        }

        [HttpGet]
        public async Task<IActionResult> AllUsers()
        {
            try
            {
                var users = await QueryDispatcher.DispatchAsync<UsersQuery, IEnumerable<UsersQuery.User>>(new UsersQuery());
                return Ok(users);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> AllData([FromQuery]UserAllDataQuery userAllDataQuery)
        {
            try
            {
                var userData = await QueryDispatcher.DispatchAsync<UserAllDataQuery, UserAllDataQuery.User>(userAllDataQuery);
                return Ok(userData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }



        //[HttpGet("by-name")]
        //public async Task<IActionResult> ByUserName(string userName)
        //{
        //    var user = await _userService.GetByUserName(userName);

        //    if (user == null)
        //        return BadRequest("Username not found");

        //    var ratings = await _ratingService.GetRatingsByUserId(user.Id);

        //    var userDto = new UserFullDataDto
        //    {
        //        Id = user.Id,
        //        AvatarUrl = user.AvatarUrl,
        //        Email = user.Email,
        //        Username = user.Username,
        //        Ratings = new List<RatingBeerDto>(),
        //        BestRatedBeer = await _beerService.GetTopOrBottomRatedBeerAsync(user.Id, true),
        //        WorstRatedBeer = await _beerService.GetTopOrBottomRatedBeerAsync(user.Id, false)
        //    };

        //    foreach (var rating in ratings)
        //    {
        //        var ratingDto = new RatingBeerDto
        //        {
        //            AfterTaste = rating.AfterTaste,
        //            Chugability = rating.Chugability,
        //            CreatedTime = rating.CreatedTime,
        //            Description = rating.Description,
        //            FirstImpression = rating.FirstImpression,
        //            OverallRating = rating.OverallRating,
        //            Taste = rating.Taste,
        //            Value = rating.Value,
        //            Beer = _mapper.Map<BeerDto>(await _beerService.GetBeerByIdAsync(rating.BeerId))
        //        };
        //        userDto.Ratings.Add(ratingDto);
        //    };

        //    return Ok(userDto);
        //}

        [HttpPost]
        public async Task<IActionResult> UpdatePassword([FromBody]UpdatePasswordCommand updatePasswordCommand)
        {
            try
            {
                await CommandDispatcher.DispatchAsync(updatePasswordCommand);
                return AcceptedAtAction(nameof(UpdatePassword), new { user = updatePasswordCommand.Id, status = "success" } );
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody]UpdateUserCommand updateUserCommand)
        {
            try
            {
                await CommandDispatcher.DispatchAsync(updateUserCommand);
                return AcceptedAtAction(nameof(Update), new {user = updateUserCommand.Id, status = "Success"});

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadAvatar([FromBody]UpdateAvatarImageCommand updateAvatarImageCommand)
        {
            try
            {
                await CommandDispatcher.DispatchAsync(updateAvatarImageCommand);
                return AcceptedAtAction(nameof(UploadAvatar), new { user = updateAvatarImageCommand.Id, status = "success" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
          
        }

    }
}