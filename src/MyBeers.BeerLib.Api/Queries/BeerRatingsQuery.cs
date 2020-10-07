using MyBeers.Common.CommonInterfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MyBeers.BeerLib.Api.Queries
{
    public class BeerRatingsQuery : IQuery<BeerRatingsQuery.Beer>
    {
        public string Id { get; set; }

        public class Beer
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Producer { get; set; }
            public string ImageUrl { get; set; }
            public IEnumerable<Rating> Ratings { get; set; }
        }

        public class Rating
        {
            public double OverallRating { get; set; }
            public string Description { get; set; }
            public int Taste { get; set; }
            public int AfterTaste { get; set; }
            public int FirstImpression { get; set; }
            public int Chugability { get; set; }
            public int Value { get; set; }
            public User User { get; set; }
        }

        public class User
        {
            public string Id { get; set; }
            public string Username { get; set; }
            public string AvatarUrl { get; set; }
        }
    }
}
