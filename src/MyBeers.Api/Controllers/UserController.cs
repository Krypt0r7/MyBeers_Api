using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
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
            return Ok(_mapper.Map<UserDto>(user));
        }

        [HttpGet]
        public async Task<IActionResult> AllUsersAsync()
        {
            var users = await _userService.GetAsync();
            return Ok(_mapper.Map<List<UserDto>>(users));
        }

        [HttpGet("me")]
        public async Task<IActionResult> LoggedInUser()
        {
            var user = await _userService.GetByIdAsync(HttpContext.User.Identity.Name);
            return Ok(_mapper.Map<UserDto>(user));
        }

        [HttpPost("add-beer")]
        public async Task<IActionResult> AddBeerToUser(int productNumber)
        {
            try
            {
                var result = await _userService.AddBeerToUserAsync(HttpContext.User.Identity.Name, productNumber);
                if (result.IsAcknowledged)
                    return Ok();
                return BadRequest("Unable to update beerlist");
            }
            catch (UserException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("remove-beer")]
        public async Task<IActionResult> RemoveBeerFromUser(string beerId)
        {
            
            var result = await _userService.RemoveBeerFromUserAsync(HttpContext.User.Identity.Name, beerId);
            if(result.IsAcknowledged)
                return Ok();
            return BadRequest("Unable to update beerlist");
        }


    }
}