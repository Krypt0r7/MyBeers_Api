using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using MyBeers.Api.Data;
using MyBeers.Api.Dtos;
using MyBeers.Api.Exceptions;
using MyBeers.Api.Utils;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyBeers.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _user;
        readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly IBeerService _beerService;

        public UserService(
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            IDBSettings mongoSettings,
            IBeerService beerService)
        {
            var client = new MongoClient(mongoSettings.ConnectionString);
            var database = client.GetDatabase(mongoSettings.DatabaseName);
            _user = database.GetCollection<User>(mongoSettings.UserCollection);
            _appSettings = appSettings.Value;
            _mapper = mapper;
            _beerService = beerService;
        }


        public async Task<UserDto> AuthenticateAsync(UserAuthenticateDto authenticateDto)
        {
            var user = await _user.Find(c => c.Username == authenticateDto.Username).FirstOrDefaultAsync();

            if (user == null || !VerifyPasswordHash(authenticateDto.Password, user.PasswordHash, user.PasswordSalt))
                throw new UserException("Password or username incorrect");

            var userDto = _mapper.Map<UserDto>(user);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id),
                }),
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            userDto.Token = tokenString;

            return userDto;
        }

        public async Task<User> CreateAsync(UserRegisterDto userDto)
        {
            var user = _mapper.Map<User>(userDto);

            if (string.IsNullOrWhiteSpace(userDto.Password))
                throw new UserException("Password is required");

            if (await _user.Find(x => x.Username == user.Username).FirstOrDefaultAsync() != null)
                throw new UserException("Username \"" + user.Username + "\" already exists");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(userDto.Password, out passwordHash, out passwordSalt);
            user.Id = null;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _user.InsertOne(user);
            var createdUserDto = _mapper.Map<UserDto>(user);

            return user;
        }

        public async Task<List<User>> GetAsync()
        {
            var users = await _user.Find(x => true).ToListAsync();
            return users;
        }


        public async Task<User> GetByIdAsync(string id) =>
            await _user.Find(x => x.Id == id).FirstOrDefaultAsync();

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }
            return true;
        }

        public async Task<UpdateResult> AddBeerToUserAsync(string id, int productId)
        {
            var user = await _user.Find(f => f.Id == id).FirstOrDefaultAsync();

            var beer = await _beerService.SaveBeerProdNumberAsync(productId);

            if (user.BeerIds == null)
                user.BeerIds = new List<string>();

            if (user.BeerIds.Contains(beer.Id))
                throw new UserException("Beer already added");
            
            user.BeerIds.Add(beer.Id);

            var userDto = _mapper.Map<UserDto>(user);

            var filter = Builders<User>.Filter.Eq(x => x.Id, id);
            var update = Builders<User>.Update
                .Set(x => x.BeerIds, user.BeerIds);
            var result = await _user.UpdateOneAsync(filter, update);
            
            return result;
        }

        public async Task<UpdateResult> RemoveBeerFromUserAsync(string id, string beerId)
        {
            var user = await _user.Find(f => f.Id == id).FirstOrDefaultAsync();

            var beerList = user.BeerIds;
            beerList.Remove(beerId);

            var filter = Builders<User>.Filter.Eq(x => x.Id, id);
            var update = Builders<User>.Update.Set(x => x.BeerIds, beerList);
            var updateResult = await _user.UpdateOneAsync(filter, update);
            return updateResult;
        }

        public async Task<DeleteResult> RemoveUser(string id)
        {
            var result = await _user.DeleteOneAsync(f => f.Id == id);
            return result;
        }

        public async Task<UpdateResult> UpdateUsersPasswordAsync(string id, string password)
        {
            var user = await _user.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (user == null)
                return null;

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.Id = null;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            var filter = Builders<User>.Filter.Eq(x => x.Id, id);
            var update = Builders<User>.Update
                .Set(x => x.PasswordHash, passwordHash)
                .Set(x => x.PasswordSalt, passwordSalt);

            var result = await _user.UpdateOneAsync(filter, update);

            return result;
        }

        public async Task<UpdateResult> UpdateUserDataAsync(string id, UpdateUserCommandDto updateUserCommandDto)
        {
            var user = await _user.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (user == null)
                return null;

            var filter = Builders<User>.Filter.Eq(x => x.Id, id);
            var update = Builders<User>.Update
                .Set(x => x.AvatarUrl, updateUserCommandDto.AvatarUrl != null ? updateUserCommandDto.AvatarUrl : user.AvatarUrl)
                .Set(x => x.Username, updateUserCommandDto.Username != null ? updateUserCommandDto.Username : user.Username)
                .Set(x => x.Email, updateUserCommandDto.Email != null ? updateUserCommandDto.Email : user.Email);

            var result = await _user.UpdateOneAsync(filter, update);

            return result;
        }


        private async Task UpdateAvatar()
        {

        }
    }
}
