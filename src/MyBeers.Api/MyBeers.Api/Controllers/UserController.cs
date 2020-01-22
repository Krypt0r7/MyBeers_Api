using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBeers.Api.Dtos;
using MyBeers.Api.Exceptions;
using MyBeers.Api.Services;

namespace MyBeers.Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody]UserRegisterDto userRegisterDto)
        {
            try
            {
                await _userService.CreateAsync(userRegisterDto);
                return Ok();

            }
            catch (UserException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateUserAsync([FromBody]UserAuthenticateDto userAuthenticateDto)
        {
            try
            {
                var userDto = await _userService.AuthenticateAsync(userAuthenticateDto);
                return Ok(userDto);
            }
            catch (UserException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> UserByIdAsync(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return BadRequest("not found");
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> AllUsersAsync()
        {
            return Ok(await _userService.GetAsync());
        }

    }
}