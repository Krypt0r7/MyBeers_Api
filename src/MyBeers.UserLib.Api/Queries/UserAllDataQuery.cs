using MyBeers.Common.CommonInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBeers.UserLib.Api.Queries
{
    public class UserAllDataQuery : IQuery<UserAllDataQuery.User>
    {
        public string Id { get; set; }
        public class User
        {
            public string Id { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public IEnumerable<Rating> Ratings { get; set; }
        }

        public class Rating
        {
            public double OverallRating { get; set; }
            public int Taste { get; set; }
            public int AfterTaste { get; set; }
            public int Chugability { get; set; }
            public int Value { get; set; }
            public int FirstImpression { get; set; }
            public string Description { get; set; }
            public Beer Beer { get; set; }
        }

        public class Beer
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Producer { get; set; }
            public double Alcohol { get; set; }
            public double Price { get; set; }
            public double Volume { get; set; }
        }
    }
}
