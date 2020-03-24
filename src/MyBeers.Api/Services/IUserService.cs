using MongoDB.Driver;
using MyBeers.Api.Data;
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
        Task<List<User>> GetAsync();
        Task<User> GetByIdAsync(string id);
        Task<User> CreateAsync(UserRegisterDto userDto);
        Task<UpdateResult> AddBeerToUserAsync(string id, int productId);
        Task<UpdateResult> RemoveBeerFromUserAsync(string id, string beerId);
        Task<DeleteResult> RemoveUser(string id);
        Task<UpdateResult> UpdateUsersPasswordAsync(string id, string password);
        Task<UpdateResult> UpdateUserDataAsync(string id, UpdateUserCommandDto updateUserCommandDto);
    }
}
