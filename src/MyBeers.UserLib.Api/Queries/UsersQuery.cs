using MyBeers.Common.CommonInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBeers.UserLib.Api.Queries
{
    public class UsersQuery : IQuery<IEnumerable<UsersQuery.User>>
    {
        public class User
        {
            public string Id { get; set; }
            public string Username { get; set; }
            public string AvatarUrl { get; set; }
            public string Email { get; set; }
        }
    }
}
