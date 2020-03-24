using System;
using System.Collections.Generic;
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
                return Ok("User created");

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveUser(string id)
        {
            var result = await _userService.RemoveUser(id);
            if (result.IsAcknowledged)
            {
                return Ok("User was deleted");
            }
            return BadRequest();
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
            if (result.IsAcknowledged)
                return Ok();
            return BadRequest("Unable to update beerlist");
        }


        [HttpPost("{id}/password")]
        public async Task<IActionResult> UpdateUserPassword(string id, [FromBody]UpdatePasswordCommandDto updateDto)
        {
            var result = await _userService.UpdateUsersPasswordAsync(id, updateDto.Password);
            if (result.IsAcknowledged)
                return Ok("Password updated");
            return BadRequest("Error updating password");
        }

        [HttpPost("{id}/update")]
        public async Task<IActionResult> UpdateUserData(string id, [FromBody]UpdateUserCommandDto updateUserCommandDto)
        {
            var result = await _userService.UpdateUserDataAsync(id, updateUserCommandDto);
            if (result.IsAcknowledged)
                return Ok(await _userService.GetByIdAsync(id));
            return BadRequest("Error updating user data");
        }

        [HttpPost("{id}/uploadImage")]
        public async Task<IActionResult> UploadAvatar([FromBody]AvatarUploadDto file, string id)
        {
            //var base64Data = Regex.Match(data, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
            var bytes = Convert.FromBase64String(file.File);
            return Ok();
        }

    }
}