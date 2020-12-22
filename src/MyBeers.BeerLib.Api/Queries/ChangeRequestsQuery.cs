using MyBeers.Common.CommonInterfaces;
using System;
using System.Collections.Generic;

namespace MyBeers.BeerLib.Api.Queries
{
    public class ChangeRequestsQuery : IQuery<IEnumerable<ChangeRequestsQuery.ChangeRequest>>
    {
        public class ChangeRequest
        {
            public string Id { get; set; }
            public User User { get; set; }
            public string Status { get; set; }
            public DateTime DateCreated { get; set; }
            public Models.Beer OldBeer { get; set; }
            public Models.Beer NewBeer { get; set; }
        }

        public class User
        {
            public string Id { get; set; }
            public string Username { get; set; }
            public string AvatarUrl { get; set; }
            public string Email { get; set; }
        }
    }

}
