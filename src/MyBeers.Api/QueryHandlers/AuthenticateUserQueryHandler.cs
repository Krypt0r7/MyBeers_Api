using MyBeers.Api.Queries;
using MyBeers.Common;
using System;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using MyBeers.Common.Bases;
using MyBeers.Common.MongoSettings;
using MyBeers.UserLib.Domain;
using MyBeers.Common.Dispatchers;
using MyBeers.Utilities;
using Microsoft.Extensions.Options;

namespace MyBeers.Api.QueryHandlers
{
    public class AuthenticateUserQueryHandler : BaseQueryHandler<UserLib.Domain.User, AuthenticateUserQuery, AuthenticateUserQuery.Authentication>
    {
        private readonly AppSettings _appSettings;
        public AuthenticateUserQueryHandler(IOptions<AppSettings> appsettings, IMongoRepository<User> repository, IQueryDispatcher queryDispatcher) : base(repository, queryDispatcher)
        {
            _appSettings = appsettings.Value;
        }
        public override AuthenticateUserQuery.Authentication Handle(AuthenticateUserQuery query)
        {
            var user = Repository.FindOne(filter => filter.Username.ToLower() == query.Username.ToLower());
            var passwordMatch = VerifyPasswordHash(query.Password, user.PasswordHash, user.PasswordSalt);
            if (!passwordMatch || user == null)
                throw new Exception("Username or password where incorrect");

            string token = AuthenticateUser(user.Id.ToString());

            return new AuthenticateUserQuery.Authentication
            {
                Token = token,
                Username = user.Username,
                Id = user.Id.ToString()
            };
        }

        private string AuthenticateUser(string id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, id),
                }),
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
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
