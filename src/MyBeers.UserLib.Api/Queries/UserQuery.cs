using MyBeers.Common.CommonInterfaces;

namespace MyBeers.UserLib.Api.Queries
{
    public class UserQuery : IQuery<UserQuery.User>
    {
        public string Id { get; set; }

        public class User
        {
            public string Id { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public string AvatarUrl { get; set; }
        }
    }
}
