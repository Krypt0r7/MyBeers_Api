using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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

        public UserService(
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            IMongoSettings mongoSettings)
        {
            var client = new MongoClient(mongoSettings.ConnectionString);
            var database = client.GetDatabase(mongoSettings.DatabaseName);
            _user = database.GetCollection<User>(mongoSettings.UserCollection);
            _appSettings = appSettings.Value;
            _mapper = mapper;
        }


        public async Task<UserDto> AuthenticateAsync(UserAuthenticateDto authenticateDto)
        {
            if (string.IsNullOrEmpty(authenticateDto.Username) || string.IsNullOrEmpty(authenticateDto.Password))
                throw new UserException("taken");

            var user = await _user.Find(c => c.Username == authenticateDto.Username).FirstOrDefaultAsync();
            if (user == null)
                throw new UserException("not found");

            var userDto = _mapper.Map<UserDto>(user);

            if (!VerifyPasswordHash(authenticateDto.Password, user.PasswordHash, user.PasswordSalt))
                throw new UserException("password");

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

        public async Task<UserDto> CreateAsync(UserRegisterDto userDto)
        {
            var user = _mapper.Map<User>(userDto);

            if (string.IsNullOrWhiteSpace(userDto.Password))
                throw new UserException("Password is required");

            if (await _user.Find(x => x.Email == user.Email).FirstOrDefaultAsync() != null)
                throw new UserException("Email \"" + user.Email + "\" already exists");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(userDto.Password, out passwordHash, out passwordSalt);
            user.Id = null;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _user.InsertOne(user);
            var createdUserDto = _mapper.Map<UserDto>(user);

            return createdUserDto;
        }

        public async Task<List<UserDto>> GetAsync()
        {
            var users = await _user.Find(x => true).ToListAsync();
            var userDtos = _mapper.Map<List<UserDto>>(users);
            return userDtos;
        }

        public async Task<UserDto> GetByIdAsync(string id) =>
            _mapper.Map<UserDto>(await _user.Find(x => x.Id == id).FirstOrDefaultAsync());

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
    }
}
