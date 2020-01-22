using MyBeers.Api.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.Api.Services
{
    public interface IUserService
    {
        Task<UserDto> AuthenticateAsync(UserAuthenticateDto authenticateDto);
        Task<List<UserDto>> GetAsync();
        Task<UserDto> GetByIdAsync(string id);
        Task<UserDto> CreateAsync(UserRegisterDto userDto);
    }
}
