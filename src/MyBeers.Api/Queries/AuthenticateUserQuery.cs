using MyBeers.Common.CommonInterfaces;

namespace MyBeers.Api.Queries
{
    public class AuthenticateUserQuery : IQuery<AuthenticateUserQuery.Authentication>
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public class Authentication
        {
            public string Id { get; set; }
            public string Username { get; set; }
            public string Token { get; set; }
        }
    }
}
