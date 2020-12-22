using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBeers.Api.Base;
using MyBeers.Api.Queries;
using MyBeers.Common.Dispatchers;
using MyBeers.UserLib;
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
                return BadRequest(ex.Message);
                
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

                return BadRequest(ex.Message);
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
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Usernames([FromQuery]UsernamesQuery usernamesQuery)
        {
            try
            {
                var usernames = await QueryDispatcher.DispatchAsync<UsernamesQuery, IEnumerable<UsernamesQuery.User>>(usernamesQuery);
                return Ok(usernames);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }

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

        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserCommand deleteUserCommand)
        {
            try
            {
                await CommandDispatcher.DispatchAsync(deleteUserCommand);
                return AcceptedAtAction(nameof(DeleteUser), new { user = deleteUserCommand.Id, status = "success" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}