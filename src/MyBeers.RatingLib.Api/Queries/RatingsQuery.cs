using MyBeers.Common.CommonInterfaces;
using System.Collections.Generic;

namespace MyBeers.RatingLib.Api.Queries
{
    public class RatingsQuery : IQuery<IEnumerable<RatingsQuery.Rating>>
    {
        public class Rating
        {
            public string Id { get; set; }
            public double OverallRating { get; set; }
            public int Taste { get; set; }
            public int AfterTaste { get; set; }
            public int Chugability { get; set; }
            public int Value { get; set; }
            public int FirstImpression { get; set; }
            public string Description { get; set; }
            public Beer Beer { get; set; }
            public User User { get; set; }
        }

        public class User
        {
            public string Id { get; set; }
            public string Username { get; set; }
            public string AvatarUrl { get; set; }
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
