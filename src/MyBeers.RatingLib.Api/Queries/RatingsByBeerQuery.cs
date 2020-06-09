using MyBeers.Common.CommonInterfaces;
using System;
using System.Collections.Generic;

namespace MyBeers.RatingLib.Api.Queries
{
    public class RatingsByBeerQuery : IQuery<IEnumerable<RatingsByBeerQuery.Rating>>
    {

        public string BeerId { get; set; }
        public class Rating
        {
            public string Id { get; set; }
            public DateTime Created { get; set; }
            public double OverallRating { get; set; }
            public int Taste { get; set; }
            public int AfterTaste { get; set; }
            public int Chugability { get; set; }
            public int Value { get; set; }
            public int FirstImpression { get; set; }
            public string Description { get; set; }
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
